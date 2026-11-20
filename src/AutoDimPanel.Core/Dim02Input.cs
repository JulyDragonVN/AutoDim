using System;
using System.Collections.Generic;

namespace AutoDimPanel.Core
{
    /// <summary>Cung tron layer OUT. Goc tinh bang do, nguoc chieu kim dong ho tu Start den End.</summary>
    public sealed class ArcSeg
    {
        public readonly Pt Center;
        public readonly double Radius;
        public readonly double StartDeg;
        public readonly double EndDeg;

        public ArcSeg(Pt center, double radius, double startDeg, double endDeg)
        {
            Center = center;
            Radius = radius;
            StartDeg = startDeg;
            EndDeg = endDeg;
        }

        public Pt Start { get { return At(StartDeg); } }
        public Pt End { get { return At(EndDeg); } }

        public Pt At(double deg)
        {
            double r = deg * Math.PI / 180.0;
            return new Pt(Center.X + Radius * Math.Cos(r), Center.Y + Radius * Math.Sin(r));
        }
    }

    /// <summary>Du lieu thuan cua mot tam thep, do Plugin doc tu ban ve (trong khung BORDER).</summary>
    public sealed class Dim02Input
    {
        /// <summary>Cac line layer OUT.</summary>
        public List<Seg> Lines = new List<Seg>();
        /// <summary>Cac arc layer OUT.</summary>
        public List<ArcSeg> Arcs = new List<ArcSeg>();
        /// <summary>Tam cac lo bu long (circle).</summary>
        public List<Pt> Holes = new List<Pt>();
        /// <summary>Ma chi tiet lay tu text "P=..." trong khung, vi du "GV41". Rong neu khong co.</summary>
        public string PartCode = "";
    }
}
