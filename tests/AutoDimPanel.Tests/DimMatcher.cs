using System;
using System.Collections.Generic;
using System.Linq;
using AutoDimPanel.Core;

namespace AutoDimPanel.Tests
{
    public sealed class MatchResult
    {
        public ExpectedDim Expected;
        public DimSpec Found;     // null neu khong khop
        public double Error;      // sai so do (mm) cua hai diem goc theo huong do
        public double PosError;   // chenh lech vi tri duong dim (mm)
    }

    /// <summary>
    /// Doi chieu dim sinh ra voi dim tay (dap an). Khop khi cung layer, kieu, dimstyle,
    /// goc quay, hai diem goc sai so &lt; tol theo huong do (khong xet toa do vuong goc voi
    /// huong do cua dim xoay vi no chi anh huong do dai duong gioi). Vi tri duong dim tinh rieng (PosError).
    /// </summary>
    public static class DimMatcher
    {
        public static List<MatchResult> Match(IList<ExpectedDim> expected, IList<DimSpec> generated, double tol)
        {
            bool[] used = new bool[generated.Count];
            List<MatchResult> res = new List<MatchResult>();
            foreach (ExpectedDim e in expected)
            {
                int best = -1;
                double bestScore = double.MaxValue, bestErr = 0, bestPos = 0;
                for (int i = 0; i < generated.Count; i++)
                {
                    DimSpec g = generated[i];
                    if (used[i] || g.Layer != e.Layer || g.Kind != e.Kind || g.Style != e.Style) continue;
                    double err, pos;
                    if (!Compare(e, g, out err, out pos)) continue;
                    double score = err + pos * 0.001;
                    if (score < bestScore)
                    {
                        bestScore = score;
                        best = i;
                        bestErr = err;
                        bestPos = pos;
                    }
                }
                MatchResult r = new MatchResult { Expected = e };
                if (best >= 0 && bestErr < tol)
                {
                    used[best] = true;
                    r.Found = generated[best];
                    r.Error = bestErr;
                    r.PosError = bestPos;
                }
                res.Add(r);
            }
            return res;
        }

        private static bool Compare(ExpectedDim e, DimSpec g, out double err, out double pos)
        {
            err = 0;
            pos = 0;
            if (e.Kind == DimKind.Aligned)
            {
                Pt[] pe = Sorted(e.P1, e.P2);
                Pt[] pg = Sorted(g.P1, g.P2);
                err = Math.Max(Pt.Dist(pe[0], pg[0]), Pt.Dist(pe[1], pg[1]));
                pos = Math.Abs(Math.Abs(Offset(e.P1, e.P2, e.LinePoint)) - Math.Abs(Offset(g.P1, g.P2, g.LinePoint)));
                return true;
            }
            double d = ((e.RotationDeg - g.RotationDeg + 90) % 180 + 180) % 180 - 90;
            if (Math.Abs(d) > 0.1) return false;
            double a = e.RotationDeg * Math.PI / 180.0;
            double ux = Math.Cos(a), uy = Math.Sin(a);
            double[] te = { e.P1.X * ux + e.P1.Y * uy, e.P2.X * ux + e.P2.Y * uy };
            double[] tg = { g.P1.X * ux + g.P1.Y * uy, g.P2.X * ux + g.P2.Y * uy };
            Array.Sort(te);
            Array.Sort(tg);
            err = Math.Max(Math.Abs(te[0] - tg[0]), Math.Abs(te[1] - tg[1]));
            double ne = -uy * e.LinePoint.X + ux * e.LinePoint.Y;
            double ng = -uy * g.LinePoint.X + ux * g.LinePoint.Y;
            pos = Math.Abs(ne - ng);
            return true;
        }

        private static Pt[] Sorted(Pt a, Pt b)
        {
            if (a.X < b.X || (a.X == b.X && a.Y <= b.Y)) return new[] { a, b };
            return new[] { b, a };
        }

        private static double Offset(Pt p1, Pt p2, Pt lp)
        {
            double dx = p2.X - p1.X, dy = p2.Y - p1.Y;
            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len < 1e-9) return 0;
            return ((lp.X - p1.X) * (-dy) + (lp.Y - p1.Y) * dx) / len;
        }
    }
}
