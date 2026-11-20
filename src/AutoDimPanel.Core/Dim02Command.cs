using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoDimPanel.Core
{
    /// <summary>
    /// DIM02 - do tam thep theo bien dang OUT (xem skills/DIM02.skill.md).
    /// Nhan du lieu thuan cua MOT tam, tra ve danh sach DimSpec. Khong nem exception khi thieu du lieu.
    /// </summary>
    public static class Dim02Command
    {
        public const string Code = "DIM02";

        public static CmdResult<List<DimSpec>> Run(Dim02Input input, Dim02Config cfg)
        {
            if (input == null) return CmdResult<List<DimSpec>>.Fail("Khong co du lieu dau vao");
            if (cfg == null) cfg = new Dim02Config();
            Builder b = new Builder(input, cfg);
            string err = b.Build();
            if (err != null) return CmdResult<List<DimSpec>>.Fail(err);
            return CmdResult<List<DimSpec>>.Success(b.Result);
        }

        /// <summary>Ten layer dim = tien to + ma chi tiet bo hau to so seri (GV41 -> "check GV"). null neu ma rong.</summary>
        public static string PartLayer(string partCode, Dim02Config cfg)
        {
            if (cfg == null) cfg = new Dim02Config();
            if (partCode == null) return null;
            string s = partCode.Trim();
            int n = s.Length;
            while (n > 0 && s[n - 1] >= '0' && s[n - 1] <= '9') n--;
            s = s.Substring(0, n).Trim();
            if (s.Length == 0) return null;
            return cfg.LayerPrefix + s;
        }

        // ------------------------------------------------------------------
        /// <summary>Canh cua tam: doan thang hoac cung.</summary>
        private sealed class E
        {
            public bool IsArc;
            public Pt P, Q, C;
            public double R, A0, A1;

            public double DX { get { return Q.X - P.X; } }
            public double DY { get { return Q.Y - P.Y; } }
            public double Len { get { return Math.Sqrt(DX * DX + DY * DY); } }

            /// <summary>Goc voi truc x, 0..180 do.</summary>
            public double Ang()
            {
                double a = Math.Atan2(DY, DX) * 180.0 / Math.PI;
                a = a % 180.0;
                if (a < 0) a += 180.0;
                return a;
            }

            public bool IsH(double ax)
            {
                if (IsArc) return false;
                double a = Ang();
                return Math.Min(a, 180.0 - a) <= ax;
            }

            public bool IsV(double ax)
            {
                return !IsArc && Math.Abs(Ang() - 90.0) <= ax;
            }

            public bool IsSlant(double ax)
            {
                return !IsArc && !IsH(ax) && !IsV(ax);
            }

            public bool Touches(Pt p, double tol)
            {
                return Pt.Dist(P, p) <= tol || Pt.Dist(Q, p) <= tol;
            }
        }

        private sealed class Builder
        {
            private readonly Dim02Input _in;
            private readonly Dim02Config c;
            public readonly List<DimSpec> Result = new List<DimSpec>();

            private string layer;
            private double xmin, xmax, ymin, ymax, cx, cy;
            private int tL, tR, tT, tB;   // so tang da dung o moi phia

            public Builder(Dim02Input input, Dim02Config cfg)
            {
                _in = input;
                c = cfg;
            }

            private static bool Eq(string a, string b)
            {
                return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
            }

            // ---- ghi dim ----
            private void AddLin(Pt p1, Pt p2, double rot, Pt lp, string tag)
            {
                Result.Add(new DimSpec
                {
                    Kind = DimKind.Rotated, Layer = layer, Style = c.Style, P1 = p1, P2 = p2,
                    LinePoint = lp, RotationDeg = rot, Tag = tag
                });
            }

            private void AddRad(ArcSeg a, E e)
            {
                double a1 = e.A1 > e.A0 ? e.A1 : e.A1 + 360.0;
                double mid = (e.A0 + a1) / 2.0;
                Result.Add(new DimSpec
                {
                    Kind = DimKind.Radial, Layer = layer, Style = c.Style,
                    Center = a.Center, ChordPoint = a.At(mid), Tag = "rad"
                });
            }

            /// <summary>Dim goc giua hai duong; diem cung nam ve phia trong tam (huong ve trong tam cac dinh).</summary>
            private void AddAng(Pt a1, Pt a2, Pt b1, Pt b2, string tag)
            {
                double ux = a2.X - a1.X, uy = a2.Y - a1.Y;
                double vx = b2.X - b1.X, vy = b2.Y - b1.Y;
                double den = ux * vy - uy * vx;
                if (Math.Abs(den) < 1e-9) return;
                double t = ((b1.X - a1.X) * vy - (b1.Y - a1.Y) * vx) / den;
                double ix = a1.X + t * ux, iy = a1.Y + t * uy;
                double wx = cx - ix, wy = cy - iy;
                double n = Math.Sqrt(wx * wx + wy * wy);
                if (n < 1e-9) return;
                Pt arc = new Pt(ix + wx / n * c.AngleArcRadius, iy + wy / n * c.AngleArcRadius);
                Result.Add(new DimSpec
                {
                    Kind = DimKind.Angular, Layer = layer, Style = c.Style,
                    A1 = a1, A2 = a2, B1 = b1, B2 = b2, ArcPoint = arc, Tag = tag
                });
            }

            // ---- duong dim o phia 'T','B','L','R' (tang tiep theo) ----
            private double LineAt(char side)
            {
                int k;
                switch (side)
                {
                    case 'L': k = ++tL; break;
                    case 'R': k = ++tR; break;
                    case 'T': k = ++tT; break;
                    default: k = ++tB; break;
                }
                double off = c.Tier1 + (k - 1) * c.TierStep;
                switch (side)
                {
                    case 'L': return xmin - off;
                    case 'R': return xmax + off;
                    case 'T': return ymax + off;
                    default: return ymin - off;
                }
            }

            private Pt AtY(Pt p, double y) { return new Pt(p.X, y); }
            private Pt AtX(Pt p, double x) { return new Pt(x, p.Y); }

            /// <summary>Chuoi dim dung: goc duoi -> moi muc y cua tam lo (lo dau tien trong 'order' o moi muc, y tang dan) -> goc tren.</summary>
            private void AddHoleChain(List<Pt> order, Pt first, Pt last, double lineX, double tol)
            {
                List<Pt> levels = new List<Pt>();
                foreach (Pt p in order)
                {
                    if (!levels.Any(l => Math.Abs(l.Y - p.Y) <= tol)) levels.Add(p);
                }
                levels = levels.OrderBy(p => p.Y).ToList();
                List<Pt> vp = new List<Pt>();
                vp.Add(first);
                vp.AddRange(levels);
                vp.Add(last);
                for (int i = 0; i + 1 < vp.Count; i++)
                    AddLin(vp[i], vp[i + 1], 90, new Pt(lineX, vp[i + 1].Y), "hole-v");
            }

            public string Build()
            {
                layer = PartLayer(_in.PartCode, c);
                if (layer == null) return "Khong thay ma chi tiet (text P=...) trong khung";

                List<E> edges = new List<E>();
                if (_in.Lines != null)
                {
                    foreach (Seg s in _in.Lines)
                    {
                        if (s == null || !Eq(s.Layer, c.LayerOut)) continue;
                        edges.Add(new E { P = s.A, Q = s.B });
                    }
                }
                List<ArcSeg> arcs = new List<ArcSeg>();
                List<E> arcEdges = new List<E>();
                if (_in.Arcs != null)
                {
                    foreach (ArcSeg a in _in.Arcs)
                    {
                        if (a == null) continue;
                        arcs.Add(a);
                        E ne = new E { IsArc = true, P = a.Start, Q = a.End, C = a.Center, R = a.Radius, A0 = a.StartDeg, A1 = a.EndDeg };
                        edges.Add(ne);
                        arcEdges.Add(ne);
                    }
                }
                if (edges.Count == 0) return "Khong thay canh layer " + c.LayerOut + " trong khung";

                List<Pt> verts = new List<Pt>();
                foreach (E e in edges)
                {
                    verts.Add(e.P);
                    verts.Add(e.Q);
                }
                xmin = verts.Min(v => v.X);
                xmax = verts.Max(v => v.X);
                ymin = verts.Min(v => v.Y);
                ymax = verts.Max(v => v.Y);
                cx = verts.Average(v => v.X);
                cy = verts.Average(v => v.Y);

                List<E> lines = edges.Where(e => !e.IsArc).ToList();
                List<E> hor = lines.Where(e => e.IsH(c.AxisTolDeg)).ToList();
                if (hor.Count == 0) return "Khong thay canh ngang trong tam";
                E longest = hor.OrderByDescending(e => e.Len).First();
                char wside = Math.Abs(longest.P.Y - ymax) <= c.Tol ? 'T' : 'B';
                double tol = c.Tol;

                // diem do + phia dat dim rong tong: dim dat ra phia canh duoc do
                List<Pt> lefts = verts.Where(v => v.X <= xmin + tol).ToList();
                List<Pt> rights = verts.Where(v => v.X >= xmax - tol).ToList();
                Pt pl, pr;
                if (wside == 'T')
                {
                    pl = lefts.OrderByDescending(v => v.Y).First();
                    pr = rights.OrderByDescending(v => v.Y).First();
                }
                else
                {
                    pl = lefts.OrderBy(v => v.Y).First();
                    pr = rights.OrderBy(v => v.Y).First();
                }
                char dside = wside;
                // 2 goc trai/phai co cung scallop: do giua hai tam cung, dim dat ve phia chua hai tam cung
                List<ArcSeg> leftArcs = arcs.Where(a => a.Center.X < cx).ToList();
                List<ArcSeg> rightArcs = arcs.Where(a => a.Center.X > cx).ToList();
                if (leftArcs.Count > 0 && rightArcs.Count > 0)
                {
                    pl = leftArcs.OrderBy(a => a.Center.X).First().Center;
                    pr = rightArcs.OrderByDescending(a => a.Center.X).First().Center;
                    dside = (pl.Y + pr.Y) / 2.0 < cy ? 'B' : 'T';
                }

                // 1) chan vat: canh xien ngan -> 1 dim ngang + 1 dim dung
                foreach (E e in lines)
                {
                    if (!e.IsSlant(c.AxisTolDeg) || e.Len > c.ChamferMaxLength) continue;
                    double midY = (e.P.Y + e.Q.Y) / 2.0;
                    double midX = (e.P.X + e.Q.X) / 2.0;
                    char hs = (Math.Abs(e.P.Y - ymin) <= tol + Math.Abs(e.DY) && midY < cy) ? 'B' : 'T';
                    char vs = midX < cx ? 'L' : 'R';
                    AddLin(e.P, e.Q, 0, new Pt(e.P.X, LineAt(hs)), "chamfer-h");
                    AddLin(e.P, e.Q, 90, new Pt(LineAt(vs), e.P.Y), "chamfer-v");
                }

                // 2) canh xien lon: goc voi canh ngang dai nhat + canh dung chung dau mut
                foreach (E e in lines)
                {
                    if (!e.IsSlant(c.AxisTolDeg) || e.Len <= c.ChamferMaxLength) continue;
                    AddAng(longest.P, longest.Q, e.P, e.Q, "slant-angle");
                    foreach (E v in lines)
                    {
                        if (!v.IsV(c.AxisTolDeg)) continue;
                        if (!(v.Touches(e.P, tol) || v.Touches(e.Q, tol))) continue;
                        char side = (v.P.X + v.Q.X) / 2.0 < cx ? 'L' : 'R';
                        AddLin(v.P, v.Q, 90, new Pt(LineAt(side), v.P.Y), "short-v");
                    }
                }

                // 3) cung: ban kinh; canh dung chung dau mut: goc voi canh ngang chung dau mut kia + lech ngang
                for (int ai = 0; ai < arcs.Count; ai++)
                {
                    ArcSeg a = arcs[ai];
                    E ae = arcEdges[ai];
                    AddRad(a, ae);
                    foreach (E v in lines)
                    {
                        if (!v.IsV(c.AxisTolDeg)) continue;
                        bool tp = v.Touches(ae.P, tol);
                        bool tq = v.Touches(ae.Q, tol);
                        if (!tp && !tq) continue;
                        Pt other = tp ? ae.Q : ae.P;
                        foreach (E h in hor)
                        {
                            if (!h.Touches(other, tol)) continue;
                            AddAng(h.P, h.Q, v.P, v.Q, "arc-angle");
                            Pt far = Pt.Dist(v.P, ae.C) < Pt.Dist(v.Q, ae.C) ? v.Q : v.P;
                            if (Math.Abs(far.X - ae.C.X) > tol)
                                AddLin(far, ae.C, 0, new Pt(far.X, LineAt(wside == 'T' ? 'B' : 'T')), "arc-offset");
                        }
                    }
                }

                // 4) lo bu long: chuoi ngang (phia doi dien dim rong tong) toi dau phai;
                //    chuoi dung chi o MOT ben: ben co lo gan canh hon (trai -> lo trai nhat moi muc y, phai -> lo phai nhat)
                if (_in.Holes != null && _in.Holes.Count > 0)
                {
                    List<Pt> cs = _in.Holes.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
                    List<Pt> leftVerts = verts.Where(v => v.X <= xmin + tol).ToList();
                    List<Pt> rightVerts = verts.Where(v => v.X >= xmax - tol).ToList();
                    Pt bl = leftVerts.OrderBy(v => v.Y).First();
                    Pt tl = leftVerts.OrderByDescending(v => v.Y).First();
                    Pt br = rightVerts.OrderBy(v => v.Y).First();
                    Pt tr = rightVerts.OrderByDescending(v => v.Y).First();
                    char hside = dside == 'T' ? 'B' : 'T';
                    double ly = LineAt(hside);
                    List<Pt> pts = new List<Pt>();
                    pts.Add(hside == 'B' ? bl : tl);
                    pts.AddRange(cs);
                    pts.Add(hside == 'B' ? br : tr);
                    for (int i = 0; i + 1 < pts.Count; i++)
                        AddLin(pts[i], pts[i + 1], 0, new Pt(pts[i + 1].X, ly), "hole-h");
                    double gapL = cs.Min(p => p.X) - xmin;
                    double gapR = xmax - cs.Max(p => p.X);
                    if (gapL <= gapR)
                    {
                        AddHoleChain(cs, bl, tl, LineAt('L'), tol);
                    }
                    else
                    {
                        List<Pt> reversed = new List<Pt>(cs);
                        reversed.Reverse();
                        AddHoleChain(reversed, br, tr, LineAt('R'), tol);
                    }
                }

                // 5) kich thuoc tong (sau cung, tang ngoai)
                AddLin(pr, pl, 0, new Pt(pr.X, LineAt(dside)), "overall-w");
                Pt pt = verts.Where(v => v.Y >= ymax - tol).OrderBy(v => v.X).First();
                Pt pb = verts.Where(v => v.Y <= ymin + tol).OrderBy(v => v.X).First();
                AddLin(pt, pb, 90, new Pt(LineAt('L'), pt.Y), "overall-h");
                return null;
            }
        }
    }
}
