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
                Point3d p1 = new Point3d(s.P1.X, s.P1.Y, 0);
                Point3d p2 = new Point3d(s.P2.X, s.P2.Y, 0);
                Point3d lp = new Point3d(s.LinePoint.X, s.LinePoint.Y, 0);
                Dimension dim;
                if (s.Kind == DimKind.Aligned)
                    dim = new AlignedDimension(p1, p2, lp, "", styleId);
                else
                    dim = new RotatedDimension(s.RotationDeg * Math.PI / 180.0, p1, p2, lp, "", styleId);
                dim.Layer = s.Layer;
                ms.AppendEntity(dim);
                tr.AddNewlyCreatedDBObject(dim, true);
                n++;
            }
            return n;
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
