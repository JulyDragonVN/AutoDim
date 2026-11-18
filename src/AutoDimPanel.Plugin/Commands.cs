using System;
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
        [CommandMethod("DIM01")]
        public void Dim01()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            PromptEntityOptions peo = new PromptEntityOptions("\nChon khung BORDER cua tam panel: ");
            peo.SetRejectMessage("\nChi chon polyline.");
            peo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Polyline), true);
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK) return;

            Dim01Config cfg = new Dim01Config();
            try
            {
                using (DocumentLock dl = doc.LockDocument())
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    Autodesk.AutoCAD.DatabaseServices.Polyline border =
                        tr.GetObject(per.ObjectId, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                    if (border == null)
                    {
                        ed.WriteMessage("\nDIM01 loi: doi tuong chon khong phai polyline.");
                        return;
                    }
                    Dim01Input input = Dim01Reader.Read(tr, db, border.GeometricExtents, cfg);
                    CmdResult<System.Collections.Generic.List<DimSpec>> result = Dim01Command.Run(input, cfg);
                    if (!result.Ok)
                    {
                        ed.WriteMessage("\nDIM01 loi: " + result.Message);
                        return;
                    }
                    int n = DimWriter.Write(tr, db, result.Value);
                    tr.Commit();
                    ed.WriteMessage("\nDIM01: da tao " + n + " dim.");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nDIM01 loi he thong: " + ex.Message);
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
