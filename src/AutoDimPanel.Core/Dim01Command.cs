using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoDimPanel.Core
{
    /// <summary>
    /// DIM01 - do tam panel (xem skills/DIM01.skill.md).
    /// Nhan du lieu thuan, tra ve danh sach DimSpec. Khong nem exception khi thieu du lieu.
    /// </summary>
    public static class Dim01Command
    {
        public const string Code = "DIM01";

        public static CmdResult<List<DimSpec>> Run(Dim01Input input, Dim01Config cfg)
        {
            if (input == null) return CmdResult<List<DimSpec>>.Fail("Khong co du lieu dau vao");
            if (cfg == null) cfg = new Dim01Config();
            Builder b = new Builder(input, cfg);
            string err = b.Build();
            if (err != null) return CmdResult<List<DimSpec>>.Fail(err);
            return CmdResult<List<DimSpec>>.Success(b.Result);
        }

        // ------------------------------------------------------------------
        private sealed class Plate
        {
            public double X0, X1;
            public List<Seg> Lines = new List<Seg>();
            public Pt LoL, HiL, LoR, HiR;   // dau trai/phai cua mat duoi (Lo) va mat tren (Hi)
            public double XmL, YmL, XmR, YmR; // trung diem dau trai/phai
        }

        private sealed class Member
        {
            public double XRef;
            public double Xc;
            public List<Seg> Lines = new List<Seg>();
            public List<double> Gus = new List<double>(); // cao do cac mat ban gusset ngang
            public double Bottom;
            public double TopY;
            public List<double> Bx = new List<double>();   // x hai mat o day thanh
            public double W;
        }

        private sealed class Nb
        {
            public int Kind; // 0 = khong co, 1 = canh tam, 2 = thanh dung
            public Member M;
        }

        private sealed class Builder
        {
            private readonly Dim01Input _in;
            private readonly Dim01Config c;
            public readonly List<DimSpec> Result = new List<DimSpec>();

            private List<Pt> bot, top;
            private double xl, xr, ymin;
            private List<Seg> chk, mrk;
            private List<Plate> plates;
            private List<Seg> row;
            private List<Pt> rowPoly;
            private List<Member> mem;

            public Builder(Dim01Input input, Dim01Config cfg)
            {
                _in = input;
                c = cfg;
            }

            private static bool Eq(string a, string b)
            {
                return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
            }

            private void Add(DimKind kind, string layer, string style, Pt p1, Pt p2, Pt lp, double rot, string tag)
            {
                Result.Add(new DimSpec
                {
                    Kind = kind, Layer = layer, Style = style, P1 = p1, P2 = p2,
                    LinePoint = lp, RotationDeg = rot, Tag = tag
                });
            }

            public string Build()
            {
                if (_in.CheckPolylines == null || _in.CheckPolylines.Count < 2)
                    return "Can 2 polyline CHECK (canh duoi va canh tren cua tam)";
                List<List<Pt>> polys = _in.CheckPolylines
                    .Where(p => p != null && p.Count >= 2)
                    .Select(p => Geo.SortedByX(p))
                    .OrderBy(p => p.Average(q => q.Y))
                    .ToList();
                if (polys.Count < 2) return "Can 2 polyline CHECK (canh duoi va canh tren cua tam)";
                bot = polys[0];
                top = polys[polys.Count - 1];
                xl = bot[0].X;
                xr = bot[bot.Count - 1].X;
                ymin = bot.Min(p => p.Y);

                List<Seg> lines = _in.Lines ?? new List<Seg>();
                chk = lines.Where(s => Eq(s.Layer, c.LayerCheck)).ToList();
                mrk = lines.Where(s => Eq(s.Layer, c.LayerMark)).ToList();

                string err;
                AddOut();
                err = AddVStiffChain();
                if (err != null) return err;
                err = AddKote();
                if (err != null) return err;
                err = FindPlates();
                if (err != null) return err;
                err = FindRow();
                if (err != null) return err;
                FindMembers();
                AddHStiffEdges();
                AddMemberThickness();
                err = AddPlateDims();
                if (err != null) return err;
                AddVChain();
                AddCrossbeam();
                AddCorners();
                return null;
            }

            // ---- kich thuoc ngoai (chieu cao tam o hai canh) ----
            private void AddOut()
            {
                Add(DimKind.Aligned, c.LayerOut, c.StyleBig, bot[0], top[0],
                    new Pt(xl - c.OutLeft, top[0].Y), 0, "out");
                Pt bl = bot[bot.Count - 1];
                Pt tl = top[top.Count - 1];
                Add(DimKind.Aligned, c.LayerOut, c.StyleBig, bl, tl,
                    new Pt(xr + c.OutRight, tl.Y), 0, "out");
            }

            // ---- chuoi dim v-stiff doc canh duoi ----
            private string AddVStiffChain()
            {
                List<Seg> vert = chk.Where(s => Math.Abs(s.DX) < 0.5 && Math.Abs(s.DY) > 1000).ToList();
                if (vert.Count == 0) return "Khong thay line dung layer CHECK (v-stiff)";
                List<Seg> sorted = vert.OrderBy(s => s.A.X).ToList();
                List<List<Seg>> groups = new List<List<Seg>>();
                foreach (Seg s in sorted)
                {
                    if (groups.Count > 0 && Math.Abs(groups[groups.Count - 1][0].A.X - s.A.X) < 1.0)
                        groups[groups.Count - 1].Add(s);
                    else
                        groups.Add(new List<Seg> { s });
                }
                List<Pt> pts = new List<Pt>();
                foreach (List<Seg> g in groups)
                {
                    if (Math.Abs(g[0].A.X - xr) < 1.0) continue; // bo canh phai cua tam
                    double lo = g.Min(s => Math.Min(s.A.Y, s.B.Y));
                    pts.Add(new Pt(g[0].A.X, lo));
                }
                for (int i = 0; i + 1 < pts.Count; i++)
                {
                    Pt a = pts[i];
                    Pt b = pts[i + 1];
                    Add(DimKind.Rotated, c.LayerVStiff, c.StyleBig, a, b,
                        new Pt(b.X, ymin - c.VStiffRowBelow), 0, "vs");
                }
                return null;
            }

            // ---- bu long (kote) ----
            private string AddKote()
            {
                List<Pt> bolts = _in.Bolts ?? new List<Pt>();
                if (bolts.Count == 0) return "Khong thay bu long (layer BOLT) trong khung";
                List<Pt> sorted = bolts.OrderBy(p => p.X).ToList();
                List<List<Pt>> cols = new List<List<Pt>>();
                foreach (Pt p in sorted)
                {
                    if (cols.Count > 0 && Math.Abs(cols[cols.Count - 1][0].X - p.X) < 1.0)
                        cols[cols.Count - 1].Add(p);
                    else
                        cols.Add(new List<Pt> { p });
                }
                foreach (List<Pt> col in cols)
                {
                    if (col.Count != 2)
                        return "Cot bu long tai x=" + col[0].X.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)
                               + " co " + col.Count + " bu long (can dung 2: hang tren va hang duoi)";
                }
                List<Pt> uppers = cols.Select(col => col.OrderByDescending(p => p.Y).First()).ToList();
                List<Pt> chain = new List<Pt>();
                chain.Add(bot[0]);
                chain.AddRange(uppers);
                chain.Add(bot[bot.Count - 1]);
                for (int i = 0; i + 1 < chain.Count; i++)
                {
                    Pt a = chain[i];
                    Pt b = chain[i + 1];
                    Add(DimKind.Rotated, c.LayerKote, c.StyleBig, a, b,
                        new Pt(b.X, ymin - c.KoteRowBelow), 0, "kote-h");
                }
                foreach (List<Pt> col in cols)
                {
                    List<Pt> ud = col.OrderByDescending(p => p.Y).ToList();
                    Pt u = ud[0];
                    Pt l = ud[1];
                    Pt f;
                    if (!Geo.TryInterpY(bot, u.X, out f))
                        return "Bu long tai x=" + u.X.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)
                               + " nam ngoai pham vi canh duoi";
                    Add(DimKind.Rotated, c.LayerKote, c.StyleSmall, u, l,
                        new Pt(u.X + c.KoteColumnOffset, l.Y), 90, "kote-v");
                    Add(DimKind.Rotated, c.LayerKote, c.StyleSmall, l, f,
                        new Pt(l.X + c.KoteColumnOffset, f.Y), 90, "kote-v");
                }
                return null;
            }

            // ---- tam h-stiff (cac line MRK1 xien/ngang dai) ----
            private string FindPlates()
            {
                List<Seg> pl = mrk.Where(s => s.Length > 300 && Math.Abs(s.DY) < 0.1 * Math.Abs(s.DX))
                    .Select(s => s.Ordered()).ToList();
                plates = new List<Plate>();
                foreach (Seg s in pl)
                {
                    Plate hit = null;
                    foreach (Plate p in plates)
                    {
                        if (Math.Abs(p.X0 - s.A.X) < 1.5 && Math.Abs(p.X1 - s.B.X) < 1.5)
                        {
                            hit = p;
                            break;
                        }
                    }
                    if (hit == null)
                    {
                        hit = new Plate { X0 = s.A.X, X1 = s.B.X };
                        plates.Add(hit);
                    }
                    hit.Lines.Add(s);
                }
                if (plates.Count == 0) return "Khong thay tam h-stiff (line MRK1 xien/ngang dai) trong khung";
                plates = plates.OrderBy(p => p.X0).ToList();
                foreach (Plate p in plates)
                {
                    Seg lo = p.Lines.OrderBy(l => l.A.Y).First();
                    Seg hi = p.Lines.OrderBy(l => l.A.Y).Last();
                    p.LoL = lo.A;
                    p.HiL = hi.A;
                    p.LoR = lo.B;
                    p.HiR = hi.B;
                    p.XmL = (lo.A.X + hi.A.X) / 2;
                    p.YmL = (lo.A.Y + hi.A.Y) / 2;
                    p.XmR = (lo.B.X + hi.B.X) / 2;
                    p.YmR = (lo.B.Y + hi.B.Y) / 2;
                }
                return null;
            }

            // ---- hang line CHECK ngang di cung voi tam h-stiff ----
            private string FindRow()
            {
                List<Seg> hl = chk.Select(s => s.Ordered())
                    .Where(s => Math.Abs(s.DX) > 400 && Math.Abs(s.DY) < 0.1 * Math.Abs(s.DX))
                    .ToList();
                List<Seg> cand = hl.Where(l => plates.Any(p => Math.Abs(l.A.Y - p.YmL) < 60))
                    .OrderBy(l => l.A.X).ThenBy(l => l.A.Y).ToList();
                row = new List<Seg>();
                foreach (Seg s in cand)
                {
                    if (row.Count > 0)
                    {
                        Seg last = row[row.Count - 1];
                        if (Math.Abs(last.A.X - s.A.X) < 0.5 && Math.Abs(last.B.X - s.B.X) < 0.5) continue;
                    }
                    row.Add(s);
                }
                if (row.Count == 0) return "Khong thay line CHECK ngang cua h-stiff";
                rowPoly = new List<Pt>();
                rowPoly.Add(row[0].A);
                foreach (Seg s in row) rowPoly.Add(s.B);
                return null;
            }

            // ---- cac thanh dung (v-stiff / crossbeam) tu line MRK1 dung ----
            private void FindMembers()
            {
                List<Seg> vm = mrk.Where(s => Math.Abs(s.DX) < 0.5 && Math.Abs(s.DY) > 300).ToList();
                mem = new List<Member>();
                foreach (Seg s in vm.OrderBy(q => q.A.X))
                {
                    Member hit = null;
                    foreach (Member m in mem)
                    {
                        if (Math.Abs(m.XRef - s.A.X) < 12)
                        {
                            hit = m;
                            break;
                        }
                    }
                    if (hit == null)
                    {
                        hit = new Member { XRef = s.A.X };
                        mem.Add(hit);
                    }
                    hit.Lines.Add(s);
                }
                List<Seg> hz = mrk.Where(s => Math.Abs(s.DY) < 0.1 && Math.Abs(Math.Abs(s.DX) - c.GussetLength) < 1.0).ToList();
                foreach (Member m in mem)
                {
                    double minx = m.Lines.Min(s => s.A.X);
                    double maxx = m.Lines.Max(s => s.A.X);
                    m.Xc = (minx + maxx) / 2;
                    m.Bottom = m.Lines.Min(s => Math.Min(s.A.Y, s.B.Y));
                    m.TopY = m.Lines.Max(s => Math.Max(s.A.Y, s.B.Y));
                    List<double> bx = m.Lines.Where(s => Math.Min(s.A.Y, s.B.Y) - m.Bottom < 0.5)
                        .Select(s => s.A.X).OrderBy(x => x).ToList();
                    m.Bx = Dedupe(bx);
                    m.W = m.Bx[m.Bx.Count - 1] - m.Bx[0];
                    List<double> ys = hz.Where(s => Math.Abs((s.A.X + s.B.X) / 2 - m.Xc) < 12)
                        .Select(s => s.A.Y).OrderBy(y => y).ToList();
                    m.Gus = Dedupe(ys);
                }
                mem = mem.OrderBy(m => m.Xc).ToList();
            }

            private static List<double> Dedupe(List<double> sorted)
            {
                List<double> r = new List<double>();
                foreach (double v in sorted)
                {
                    if (r.Count == 0 || Math.Abs(r[r.Count - 1] - v) > 0.05) r.Add(v);
                }
                return r;
            }

            // cac cap (mat duoi, mat tren) cua ban gusset, cach nhau GussetThickness
            private List<double[]> GusPairs(Member m)
            {
                List<double[]> pairs = new List<double[]>();
                foreach (double y in m.Gus)
                {
                    foreach (double y2 in m.Gus)
                    {
                        if (Math.Abs(y2 - y - c.GussetThickness) < 0.1) pairs.Add(new[] { y, y2 });
                    }
                }
                return pairs;
            }

            // ---- chuoi dim h-stiff doc hai canh trai/phai ----
            private void AddHStiffEdges()
            {
                Pt rs = row[0].A;
                Pt re = row[row.Count - 1].B;
                Pt tr = top[top.Count - 1];
                Pt br = bot[bot.Count - 1];
                double xL = xl - c.HStiffEdgeLeft;
                double xR = xr + c.HStiffEdgeRight;
                Add(DimKind.Rotated, c.LayerHStiff, c.StyleBig, top[0], rs, new Pt(xL, rs.Y), 90, "hs-edge");
                Add(DimKind.Rotated, c.LayerHStiff, c.StyleBig, rs, bot[0], new Pt(xL, bot[0].Y), 90, "hs-edge");
                Add(DimKind.Rotated, c.LayerHStiff, c.StyleBig, tr, re, new Pt(xR, re.Y), 90, "hs-edge");
                Add(DimKind.Rotated, c.LayerHStiff, c.StyleBig, re, br, new Pt(xR, br.Y), 90, "hs-edge");
            }

            // ---- be day cac thanh dung o day tam ----
            private void AddMemberThickness()
            {
                foreach (Member m in mem)
                {
                    bool cb = m.W < c.ThinMemberMaxWidth;
                    string layer = cb ? c.LayerCrossbeam : c.LayerVStiff;
                    double off = cb ? c.MemberThicknessBelowCrossbeam : c.MemberThicknessBelowVStiff;
                    Pt p1 = new Pt(m.Bx[0], m.Bottom);
                    Pt p2 = new Pt(m.Bx[m.Bx.Count - 1], m.Bottom);
                    Add(DimKind.Rotated, layer, c.StyleSmall, p1, p2, new Pt(p2.X, m.Bottom - off), 0, "mem-t");
                }
            }

            private Nb FindNeighbor(double x, int side)
            {
                Member best = null;
                double bestDist = 0;
                foreach (Member m in mem)
                {
                    double f = side < 0 ? m.Bx[m.Bx.Count - 1] : m.Bx[0];
                    if (m.Gus.Count > 0) f = side < 0 ? m.Xc + c.GussetLength / 2 : m.Xc - c.GussetLength / 2;
                    double dist = Math.Abs(x - f);
                    bool onSide = side < 0 ? f <= x + 1 : f >= x - 1;
                    if (dist < 60 && onSide && (best == null || dist < bestDist))
                    {
                        best = m;
                        bestDist = dist;
                    }
                }
                if (best != null) return new Nb { Kind = 2, M = best };
                double edge = side < 0 ? xl : xr;
                if (Math.Abs(x - edge) < 60) return new Nb { Kind = 1 };
                return new Nb { Kind = 0 };
            }

            private static bool Plain(Nb n)
            {
                return n.Kind == 2 && n.M.Gus.Count == 0;
            }

            // ---- be day tam h-stiff va khe ho o hai dau ----
            private string AddPlateDims()
            {
                foreach (Plate p in plates)
                {
                    Nb nl = FindNeighbor(p.XmL, -1);
                    Nb nr = FindNeighbor(p.XmR, +1);
                    bool left;
                    if (Plain(nl)) left = true;
                    else if (Plain(nr)) left = false;
                    else left = true;
                    Pt a = left ? p.LoL : p.LoR;
                    Pt b = left ? p.HiL : p.HiR;
                    Nb nb = left ? nl : nr;
                    double xm = (a.X + b.X) / 2;
                    double ym = (a.Y + b.Y) / 2;
                    double away = left ? -1 : 1;
                    double sgn = nb.Kind == 1 ? away : -away;
                    Add(DimKind.Aligned, c.LayerHStiff, c.StyleSmall, a, b,
                        new Pt(xm + sgn * c.PlateThicknessOffset, ym), 0, "pl-t");

                    for (int side = 0; side < 2; side++)
                    {
                        bool isLeft = side == 0;
                        Nb n = isLeft ? nl : nr;
                        if (n.Kind == 0) continue;
                        if (n.Kind == 1 && !isLeft && !c.GapAtRightEdge) continue;
                        double xe = isLeft ? p.XmL : p.XmR;
                        double ye = isLeft ? p.YmL : p.YmR;
                        if (n.Kind == 1)
                        {
                            Pt pe = new Pt(isLeft ? xl : xr, isLeft ? row[0].A.Y : row[row.Count - 1].B.Y);
                            Add(DimKind.Rotated, c.LayerHStiff, c.StyleSmall, new Pt(xe, ye), pe,
                                new Pt(xe, ye + c.GapEdgeAbove), 0, "gap");
                        }
                        else if (n.M.Gus.Count > 0)
                        {
                            List<double[]> pairs = GusPairs(n.M);
                            if (pairs.Count == 0) continue;
                            double gy = pairs.Max(q => q[1]);
                            double gx = n.M.Xc + (isLeft ? c.GussetLength / 2 : -c.GussetLength / 2);
                            Pt q1 = new Pt(gx, gy);
                            Pt q2 = new Pt(xe, ye);
                            if (q2.X < q1.X) { Pt t = q1; q1 = q2; q2 = t; }
                            Add(DimKind.Rotated, c.LayerHStiff, c.StyleSmall, q1, q2,
                                new Pt(q1.X, gy - c.GapGussetBelow), 0, "gap");
                        }
                        else
                        {
                            double fx = isLeft ? n.M.Bx[n.M.Bx.Count - 1] : n.M.Bx[0];
                            Pt ry;
                            if (!Geo.TryInterpY(rowPoly, fx, out ry)) continue;
                            Pt q1 = new Pt(fx, ry.Y);
                            Pt q2 = new Pt(xe, ry.Y);
                            if (q2.X < q1.X) { Pt t = q1; q1 = q2; q2 = t; }
                            Add(DimKind.Rotated, c.LayerHStiff, c.StyleSmall, q1, q2,
                                new Pt(q1.X, ry.Y + c.GapMemberAbove), 0, "gap");
                        }
                    }
                }
                return null;
            }

            // ---- thanh dung khong co gusset: chia doan theo hang h-stiff ----
            private void AddVChain()
            {
                foreach (Member m in mem)
                {
                    if (!(m.W < c.ThinMemberMaxWidth) || m.Gus.Count > 0) continue;
                    Pt t, b, h;
                    if (!Geo.TryInterpY(top, m.Xc, out t)) continue;
                    if (!Geo.TryInterpY(bot, m.Xc, out b)) continue;
                    if (!Geo.TryInterpY(rowPoly, m.Xc, out h)) continue;
                    Add(DimKind.Rotated, c.LayerHStiff, c.StyleSmall, t, h, new Pt(m.Xc + c.VChainOffset, h.Y), 90, "vchain");
                    Add(DimKind.Rotated, c.LayerHStiff, c.StyleSmall, h, b, new Pt(m.Xc + c.VChainOffset, b.Y), 90, "vchain");
                }
            }

            // ---- thanh co gusset ngang (crossbeam) ----
            private void AddCrossbeam()
            {
                foreach (Member m in mem)
                {
                    if (m.Gus.Count == 0) continue;
                    List<double[]> pairs = GusPairs(m);
                    if (pairs.Count == 0) continue;
                    double xg = m.Xc + c.GussetLength / 2;
                    List<double> ys = new List<double>();
                    ys.Add(m.TopY);
                    foreach (double[] pr in pairs.OrderByDescending(q => q[0]))
                    {
                        ys.Add(pr[1]);
                        ys.Add(pr[0]);
                    }
                    ys.Add(m.Bottom);
                    for (int i = 0; i + 1 < ys.Count; i++)
                    {
                        Add(DimKind.Rotated, c.LayerCrossbeam, c.StyleSmall, new Pt(xg, ys[i]), new Pt(xg, ys[i + 1]),
                            new Pt(xg + c.CrossbeamChainOffset, ys[i + 1]), 90, "cb-v");
                    }
                    double[] lo = pairs.OrderBy(q => q[0]).First();
                    double[] hi = pairs.OrderBy(q => q[0]).Last();
                    Add(DimKind.Rotated, c.LayerCrossbeam, c.StyleSmall, new Pt(xg, lo[1]), new Pt(m.Xc - c.GussetLength / 2, lo[1]),
                        new Pt(xg, lo[1] + c.GussetWidthInside), 0, "cb-w");
                    Add(DimKind.Rotated, c.LayerCrossbeam, c.StyleSmall, new Pt(xg, hi[0]), new Pt(m.Xc - c.GussetLength / 2, hi[0]),
                        new Pt(xg, hi[0] - c.GussetWidthInside), 0, "cb-w");
                }
            }

            // ---- 4 goc tam: vach MRK1 dai 100 vuong goc canh ----
            private void AddCorners()
            {
                double topAvg = top.Average(p => p.Y);
                foreach (Seg s in mrk)
                {
                    if (Math.Abs(s.Length - 100) >= 1.0 || Math.Abs(s.DX) >= 10) continue;
                    bool topish = Math.Max(s.A.Y, s.B.Y) > topAvg - 300;
                    Pt pt = topish
                        ? (s.A.Y >= s.B.Y ? s.A : s.B)
                        : (s.A.Y <= s.B.Y ? s.A : s.B);
                    bool left = pt.X < (xl + xr) / 2;
                    if (topish)
                    {
                        Pt cn = left ? top[0] : top[top.Count - 1];
                        Pt p1 = left ? cn : pt;
                        Pt p2 = left ? pt : cn;
                        Add(DimKind.Aligned, c.LayerHStiff, c.StyleSmall, p1, p2,
                            new Pt((p1.X + p2.X) / 2, Math.Max(p1.Y, p2.Y) + c.CornerOffset), 0, "corner");
                    }
                    else
                    {
                        Pt cn = left ? bot[0] : bot[bot.Count - 1];
                        Pt sa = left ? bot[0] : bot[bot.Count - 2];
                        Pt sb = left ? bot[1] : bot[bot.Count - 1];
                        double ang = Math.Atan2(sb.Y - sa.Y, sb.X - sa.X) * 180.0 / Math.PI + (left ? 0 : 180);
                        Pt p1 = left ? cn : pt;
                        Pt p2 = left ? pt : cn;
                        Add(DimKind.Rotated, c.LayerHStiff, c.StyleSmall, p1, p2,
                            new Pt((p1.X + p2.X) / 2, Math.Min(p1.Y, p2.Y) - c.CornerOffset), ang, "corner");
                    }
                }
            }
        }
    }
}
