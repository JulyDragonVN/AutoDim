using System;
using System.Collections.Generic;
using AutoDimPanel.Core;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(AutoDimPanel.Plugin.Commands))]

namespace AutoDimPanel.Plugin
{
    public class Commands
    {
        /// <summary>Layer khung tam dung cho DIM01 (DIM02 lay tu Dim02Config.LayerBorder).</summary>
        private const string Dim01BorderLayer = "BORDER";

        /// <summary>Chon nhieu khung (LWPOLYLINE) tren layer chi dinh bang selection. null neu huy/khong chon.</summary>
        private static ObjectId[] SelectBorders(Editor ed, string layer, string prompt)
        {
            SelectionFilter filter = new SelectionFilter(new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start, "LWPOLYLINE"),
                new TypedValue((int)DxfCode.LayerName, layer)
            });
            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding = prompt;
            PromptSelectionResult psr = ed.GetSelection(pso, filter);
            if (psr.Status != PromptStatus.OK) return null;
            return psr.Value.GetObjectIds();
        }

        [CommandMethod("DIM01")]
        public void Dim01()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            Dim01Config cfg = new Dim01Config();

            ObjectId[] ids = SelectBorders(ed, Dim01BorderLayer, "\nChon cac khung " + Dim01BorderLayer + " cua tam panel: ");
            if (ids == null) return;

            try
            {
                using (DocumentLock dl = doc.LockDocument())
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    int plates = 0, dims = 0, failed = 0;
                    for (int i = 0; i < ids.Length; i++)
                    {
                        Autodesk.AutoCAD.DatabaseServices.Polyline border =
                            tr.GetObject(ids[i], OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (border == null) continue;
                        Dim01Input input = Dim01Reader.Read(tr, db, border.GeometricExtents, cfg);
                        CmdResult<List<DimSpec>> result = Dim01Command.Run(input, cfg);
                        if (!result.Ok)
                        {
                            failed++;
                            ed.WriteMessage("\nDIM01 khung " + (i + 1) + "/" + ids.Length + " loi: " + result.Message);
                            continue;
                        }
                        int n = DimWriter.Write(tr, db, result.Value);
                        plates++;
                        dims += n;
                        ed.WriteMessage("\nDIM01 khung " + (i + 1) + "/" + ids.Length + ": " + n + " dim");
                    }
                    tr.Commit();
                    ed.WriteMessage("\nDIM01: xong " + plates + " tam, " + dims + " dim" + (failed > 0 ? ", " + failed + " khung loi" : "") + ".");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nDIM01 loi he thong: " + ex.Message);
            }
        }

        [CommandMethod("DIM02")]
        public void Dim02()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            Dim02Config cfg = new Dim02Config();

            ObjectId[] ids = SelectBorders(ed, cfg.LayerBorder, "\nChon cac khung " + cfg.LayerBorder + " cua tam thep: ");
            if (ids == null) return;

            try
            {
                using (DocumentLock dl = doc.LockDocument())
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    int plates = 0, dims = 0, failed = 0;
                    for (int i = 0; i < ids.Length; i++)
                    {
                        Autodesk.AutoCAD.DatabaseServices.Polyline border =
                            tr.GetObject(ids[i], OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (border == null) continue;
                        Dim02Input input = Dim02Reader.Read(tr, db, border.GeometricExtents, cfg);
                        CmdResult<List<DimSpec>> result = Dim02Command.Run(input, cfg);
                        string label = string.IsNullOrEmpty(input.PartCode) ? "?" : input.PartCode;
                        if (!result.Ok)
                        {
                            failed++;
                            ed.WriteMessage("\nDIM02 khung " + (i + 1) + "/" + ids.Length + " (" + label + ") loi: " + result.Message);
                            continue;
                        }
                        int n = DimWriter.Write(tr, db, result.Value);
                        plates++;
                        dims += n;
                        ed.WriteMessage("\nDIM02 khung " + (i + 1) + "/" + ids.Length + " (" + label + "): " + n + " dim, layer " + result.Value[0].Layer);
                    }
                    tr.Commit();
                    ed.WriteMessage("\nDIM02: xong " + plates + " tam, " + dims + " dim" + (failed > 0 ? ", " + failed + " khung loi" : "") + ".");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nDIM02 loi he thong: " + ex.Message);
            }
        }

        [CommandMethod("DIMPAL")]
        public void DimPal()
        {
            PaletteHost.Show();
        }

        [CommandMethod("DIMLIST")]
        public void DimList()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            doc.Editor.WriteMessage("\nDanh sach lenh do:");
            foreach (AutoDimPanel.Core.CommandInfo ci in CommandRegistry.All)
                doc.Editor.WriteMessage("\n  " + ci.Code + " - " + ci.Name + ": " + ci.Description);
        }
    }
}
