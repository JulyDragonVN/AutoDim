using System;
using System.Drawing;
using System.Windows.Forms;
using AutoDimPanel.Core;

namespace AutoDimPanel.Plugin
{
    public static class PaletteHost
    {
        private static Autodesk.AutoCAD.Windows.PaletteSet _ps;

        public static void Show()
        {
            if (_ps == null)
            {
                _ps = new Autodesk.AutoCAD.Windows.PaletteSet("AutoDimPanel", new Guid("6E0B1B0E-6B1B-4F6E-9D3B-5A3F2C7D8E11"));
                _ps.Style = Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowAutoHideButton | Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowCloseButton;
                _ps.MinimumSize = new Size(220, 160);
                _ps.Add("Lenh do", new PalettePanel());
            }
            _ps.Visible = true;
        }
    }

    /// <summary>Moi lenh trong CommandRegistry mot nut; bam se gui lenh cho AutoCAD.</summary>
    public sealed class PalettePanel : UserControl
    {
        public PalettePanel()
        {
            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.FlowDirection = FlowDirection.TopDown;
            flow.WrapContents = false;
            flow.AutoScroll = true;
            flow.Padding = new Padding(6);
            ToolTip tip = new ToolTip();
            foreach (AutoDimPanel.Core.CommandInfo ci in CommandRegistry.All)
            {
                Button b = new Button();
                b.Text = ci.Code + "  " + ci.Name;
                b.Width = 200;
                b.Height = 36;
                b.TextAlign = ContentAlignment.MiddleLeft;
                string code = ci.Code;
                b.Click += delegate
                {
                    Autodesk.AutoCAD.ApplicationServices.Document doc =
                        Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    if (doc != null) doc.SendStringToExecute(code + " ", true, false, false);
                };
                tip.SetToolTip(b, ci.Description);
                flow.Controls.Add(b);
            }
            Controls.Add(flow);
        }
    }
}
