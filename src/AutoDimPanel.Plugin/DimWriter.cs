using System;
using System.Collections.Generic;
using AutoDimPanel.Core;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace AutoDimPanel.Plugin
{
    /// <summary>Ghi cac DimSpec vao Model space. Thieu layer thi tao; thieu dimstyle thi dung style hien hanh.</summary>
    public static class DimWriter
    {
        public static int Write(Transaction tr, Database db, IEnumerable<DimSpec> specs)
        {
            BlockTableRecord ms = (BlockTableRecord)tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite);
            LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
            DimStyleTable dst = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForRead);
            int n = 0;
            foreach (DimSpec s in specs)
            {
                EnsureLayer(tr, lt, s.Layer);
                ObjectId styleId = dst.Has(s.Style) ? dst[s.Style] : db.Dimstyle;
                Dimension dim;
                if (s.Kind == DimKind.Aligned)
                    dim = new AlignedDimension(Pt3(s.P1), Pt3(s.P2), Pt3(s.LinePoint), "", styleId);
                else if (s.Kind == DimKind.Angular)
                    dim = new LineAngularDimension2(Pt3(s.A1), Pt3(s.A2), Pt3(s.B1), Pt3(s.B2), Pt3(s.ArcPoint), "", styleId);
                else if (s.Kind == DimKind.Radial)
                    dim = new RadialDimension(Pt3(s.Center), Pt3(s.ChordPoint), 0.0, "", styleId);
                else
                    dim = new RotatedDimension(s.RotationDeg * Math.PI / 180.0, Pt3(s.P1), Pt3(s.P2), Pt3(s.LinePoint), "", styleId);
                dim.Layer = s.Layer;
                ms.AppendEntity(dim);
                tr.AddNewlyCreatedDBObject(dim, true);
                n++;
            }
            return n;
        }

        private static Point3d Pt3(Pt p)
        {
            return new Point3d(p.X, p.Y, 0);
        }

        private static void EnsureLayer(Transaction tr, LayerTable lt, string name)
        {
            if (lt.Has(name)) return;
            if (!lt.IsWriteEnabled) lt.UpgradeOpen();
            LayerTableRecord rec = new LayerTableRecord();
            rec.Name = name;
            lt.Add(rec);
            tr.AddNewlyCreatedDBObject(rec, true);
        }
    }
}
