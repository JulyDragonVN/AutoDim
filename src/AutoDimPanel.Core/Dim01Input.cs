using System.Collections.Generic;

namespace AutoDimPanel.Core
{
    /// <summary>Du lieu thuan cua mot tam panel, do Plugin doc tu ban ve (trong khung BORDER).</summary>
    public sealed class Dim01Input
    {
        /// <summary>Cac polyline layer CHECK (canh duoi, canh tren cua tam).</summary>
        public List<List<Pt>> CheckPolylines = new List<List<Pt>>();
        /// <summary>Cac line layer CHECK va MRK1.</summary>
        public List<Seg> Lines = new List<Seg>();
        /// <summary>Tam cac circle layer BOLT.</summary>
        public List<Pt> Bolts = new List<Pt>();
    }
}
