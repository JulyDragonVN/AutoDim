using System;
using AutoDimPanel.Core;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoDimPanel.Plugin
{
    /// <summary>Doc cac doi tuong trong khung BORDER thanh Dim01Input (du lieu thuan).</summary>
    public static class Dim01Reader
    {
        public static Dim01Input Read(Transaction tr, Database db, Extents3d frame, Dim01Config c)
        {
            Dim01Input input = new Dim01Input();
            BlockTableRecord ms = (BlockTableRecord)tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead);
            foreach (ObjectId id in ms)
            {
                Entity ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                if (ent == null || ent is Dimension) continue;
                bool isCheck = Eq(ent.Layer, c.LayerCheck);
                bool isMark = Eq(ent.Layer, c.LayerMark);
                bool isBolt = Eq(ent.Layer, c.LayerBolt);
                if (!isCheck && !isMark && !isBolt) continue;

                Extents3d ext;
                try { ext = ent.GeometricExtents; }
                catch (System.Exception) { continue; }
                if (!Inside(frame, ext)) continue;

                Polyline pl = ent as Polyline;
                if (pl != null)
                {
                    if (isCheck)
                    {
                        System.Collections.Generic.List<Pt> pts = new System.Collections.Generic.List<Pt>();
                        for (int i = 0; i < pl.NumberOfVertices; i++)
                        {
                            Autodesk.AutoCAD.Geometry.Point2d p = pl.GetPoint2dAt(i);
                            pts.Add(new Pt(p.X, p.Y));
                        }
                        input.CheckPolylines.Add(pts);
                    }
                    continue;
                }
                Line ln = ent as Line;
                if (ln != null && (isCheck || isMark))
                {
                    input.Lines.Add(new Seg(new Pt(ln.StartPoint.X, ln.StartPoint.Y), new Pt(ln.EndPoint.X, ln.EndPoint.Y), ent.Layer));
                    continue;
                }
                Circle ci = ent as Circle;
                if (ci != null && isBolt)
                    input.Bolts.Add(new Pt(ci.Center.X, ci.Center.Y));
            }
            return input;
        }

        private static bool Eq(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        private static bool Inside(Extents3d frame, Extents3d e)
        {
            const double tol = 1.0;
            return e.MinPoint.X >= frame.MinPoint.X - tol && e.MaxPoint.X <= frame.MaxPoint.X + tol
                && e.MinPoint.Y >= frame.MinPoint.Y - tol && e.MaxPoint.Y <= frame.MaxPoint.Y + tol;
        }
    }
}
