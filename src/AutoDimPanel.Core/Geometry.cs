using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AutoDimPanel.Core
{
    /// <summary>Diem 2D (WCS, mm). Khong tham chieu AutoCAD.</summary>
    public struct Pt
    {
        public readonly double X;
        public readonly double Y;

        public Pt(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static double Dist(Pt a, Pt b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString()
        {
            return X.ToString("0.###", CultureInfo.InvariantCulture) + "," + Y.ToString("0.###", CultureInfo.InvariantCulture);
        }
    }

    /// <summary>Doan thang kem ten layer.</summary>
    public sealed class Seg
    {
        public readonly Pt A;
        public readonly Pt B;
        public readonly string Layer;

        public Seg(Pt a, Pt b, string layer)
        {
            A = a;
            B = b;
            Layer = layer;
        }

        public double DX { get { return B.X - A.X; } }
        public double DY { get { return B.Y - A.Y; } }
        public double Length { get { return Pt.Dist(A, B); } }

        /// <summary>Tra ve doan co A.X &lt;= B.X (doi dau neu can).</summary>
        public Seg Ordered()
        {
            if (A.X <= B.X) return this;
            return new Seg(B, A, Layer);
        }
    }

    public static class Geo
    {
        public static List<Pt> SortedByX(IEnumerable<Pt> pts)
        {
            return pts.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();
        }

        /// <summary>Noi suy Y tai X tren polyline da sap theo X. false neu X ngoai pham vi.</summary>
        public static bool TryInterpY(IList<Pt> poly, double x, out Pt p)
        {
            p = new Pt(x, 0);
            for (int i = 0; i + 1 < poly.Count; i++)
            {
                Pt a = poly[i];
                Pt b = poly[i + 1];
                if (x >= a.X - 1e-6 && x <= b.X + 1e-6)
                {
                    double dx = b.X - a.X;
                    if (Math.Abs(dx) < 1e-9)
                    {
                        p = new Pt(x, a.Y);
                        return true;
                    }
                    double t = (x - a.X) / dx;
                    p = new Pt(x, a.Y + t * (b.Y - a.Y));
                    return true;
                }
            }
            return false;
        }
    }
}
