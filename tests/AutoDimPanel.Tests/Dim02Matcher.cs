using System;
using System.Collections.Generic;
using AutoDimPanel.Core;

namespace AutoDimPanel.Tests
{
    /// <summary>
    /// Doi chieu dim DIM02 sinh ra voi dim tay (dap an). Khop khi cung loai, cung huong (dim xoay),
    /// cac diem goc sai so &lt;= tol (khong xet thu tu). Vi tri duong dim khong dung de khop (dim tay keo khac nhau).
    /// </summary>
    public static class Dim02Matcher
    {
        /// <summary>Tra ve, voi moi dim mong doi, chi so dim sinh ra khop (hoac -1).</summary>
        public static int[] Match(IList<Expected02> expected, IList<DimSpec> generated, double tol)
        {
            bool[] used = new bool[generated.Count];
            int[] res = new int[expected.Count];
            for (int k = 0; k < expected.Count; k++)
            {
                res[k] = -1;
                for (int i = 0; i < generated.Count; i++)
                {
                    if (used[i]) continue;
                    if (Same(expected[k], generated[i], tol))
                    {
                        used[i] = true;
                        res[k] = i;
                        break;
                    }
                }
            }
            return res;
        }

        private static bool Near(Pt a, Pt b, double tol) { return Pt.Dist(a, b) <= tol; }

        private static bool SegSame(Pt a1, Pt a2, Pt b1, Pt b2, double tol)
        {
            return (Near(a1, b1, tol) && Near(a2, b2, tol)) || (Near(a1, b2, tol) && Near(a2, b1, tol));
        }

        private static bool Same(Expected02 e, DimSpec g, double tol)
        {
            if (e.Kind != g.Kind) return false;
            if (e.Kind == DimKind.Rotated)
            {
                double d = ((e.RotationDeg - g.RotationDeg + 90) % 180 + 180) % 180 - 90;
                if (Math.Abs(d) > 1.0) return false;
                return SegSame(e.P1, e.P2, g.P1, g.P2, tol);
            }
            if (e.Kind == DimKind.Angular)
            {
                return (SegSame(e.A1, e.A2, g.A1, g.A2, tol) && SegSame(e.B1, e.B2, g.B1, g.B2, tol))
                    || (SegSame(e.A1, e.A2, g.B1, g.B2, tol) && SegSame(e.B1, e.B2, g.A1, g.A2, tol));
            }
            if (e.Kind == DimKind.Radial) return Near(e.Center, g.Center, tol);
            return false;
        }
    }
}
