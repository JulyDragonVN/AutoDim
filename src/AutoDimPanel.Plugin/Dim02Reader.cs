using System;
using AutoDimPanel.Core;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoDimPanel.Plugin
{
    /// <summary>Doc cac doi tuong trong khung BORDER thanh Dim02Input (du lieu thuan) cho mot tam thep.</summary>
    public static class Dim02Reader
    {
        public static Dim02Input Read(Transaction tr, Database db, Extents3d frame, Dim02Config c)
        {
            Dim02Input input = new Dim02Input();
            BlockTableRecord ms = (BlockTableRecord)tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead);
            foreach (ObjectId id in ms)
            {
                Entity ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                if (ent == null || ent is Dimension) continue;

                // text ma chi tiet "P=GV41": kiem tra theo diem chen, khong can extents
                DBText tx = ent as DBText;
                if (tx != null)
                {
                    ReadPartCode(input, c, tx.TextString, tx.Position.X, tx.Position.Y, frame);
                    continue;
                }
                MText mt = ent as MText;
                if (mt != null)
                {
                    ReadPartCode(input, c, mt.Contents, mt.Location.X, mt.Location.Y, frame);
                    continue;
                }

                bool isOut = Eq(ent.Layer, c.LayerOut);
                bool isBolt = IsBoltLayer(ent.Layer, c);
                if (!isOut && !isBolt) continue;

                Extents3d ext;
                try { ext = ent.GeometricExtents; }
                catch (System.Exception) { continue; }
                if (!Inside(frame, ext)) continue;

                Line ln = ent as Line;
                if (ln != null && isOut)
                {
                    input.Lines.Add(new Seg(new Pt(ln.StartPoint.X, ln.StartPoint.Y), new Pt(ln.EndPoint.X, ln.EndPoint.Y), ent.Layer));
                    continue;
                }
                Arc ar = ent as Arc;
                if (ar != null && isOut)
                {
                    input.Arcs.Add(new ArcSeg(new Pt(ar.Center.X, ar.Center.Y), ar.Radius,
                        ar.StartAngle * 180.0 / Math.PI, ar.EndAngle * 180.0 / Math.PI));
                    continue;
                }
                Circle ci = ent as Circle;
                if (ci != null && isBolt)
                    input.Holes.Add(new Pt(ci.Center.X, ci.Center.Y));
            }
            return input;
        }

        private static void ReadPartCode(Dim02Input input, Dim02Config c, string text, double x, double y, Extents3d frame)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (x < frame.MinPoint.X || x > frame.MaxPoint.X || y < frame.MinPoint.Y || y > frame.MaxPoint.Y) return;
            string t = text.Trim();
            if (!t.StartsWith(c.PartTextPrefix, StringComparison.OrdinalIgnoreCase)) return;
            input.PartCode = t.Substring(c.PartTextPrefix.Length).Trim();
        }

        private static bool IsBoltLayer(string layer, Dim02Config c)
        {
            if (c.LayerBolts == null) return false;
            foreach (string b in c.LayerBolts)
                if (Eq(layer, b)) return true;
            return false;
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
