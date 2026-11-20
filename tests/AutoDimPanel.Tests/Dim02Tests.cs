using System.Collections.Generic;
using System.Linq;
using AutoDimPanel.Core;
using Xunit;

namespace AutoDimPanel.Tests
{
    public class Dim02Tests
    {
        private static List<DimSpec> Run(int plate)
        {
            CmdResult<List<DimSpec>> r = Dim02Command.Run(Sample02.Input(plate), new Dim02Config());
            Assert.True(r.Ok, r.Message);
            return r.Value;
        }

        // thu tu tam trong sample: 0 = GV41, 1 = GH3, 2 = GH2 (co 4 bu long), 3 = GV9
        [Fact]
        public void Sample02_DimCountPerPlate()
        {
            Assert.Equal(7, Run(0).Count);
            Assert.Equal(6, Run(1).Count);
            Assert.Equal(12, Run(2).Count);
            Assert.Equal(6, Run(3).Count);
        }

        // Quy tac moi (mục 4, 5 skill) lam khac dim tay o 2 cho: dim tong 19B (tam GV41) do giua 2 tam cung;
        // tam GH2 them dim toi dau phai. Moi cho khac con lai phai khop dim tay.
        [Fact]
        public void Sample02_HandDimsReproduced_ExceptKnownRuleChanges()
        {
            for (int p = 0; p < Sample02.PlateCount; p++)
            {
                List<DimSpec> gen = Run(p);
                List<Expected02> exp = Sample02.Expected(p);
                int[] m = Dim02Matcher.Match(exp, gen, 1.0);
                string missing = string.Join(",", Enumerable.Range(0, exp.Count).Where(i => m[i] < 0).Select(i => exp[i].Handle));
                Assert.Equal(p == 0 ? "19B" : "", missing);
                string extra = string.Join(",", Enumerable.Range(0, gen.Count).Where(i => !m.Contains(i)).Select(i => gen[i].Tag).OrderBy(t => t));
                if (p == 0) Assert.Equal("overall-w", extra);
                else if (p == 2) Assert.Equal("hole-h", extra);
                else Assert.Equal("", extra);
            }
        }

        [Fact]
        public void Sample02_TotalHandDimsIs30()
        {
            int n = 0;
            for (int p = 0; p < Sample02.PlateCount; p++) n += Sample02.Expected(p).Count;
            Assert.Equal(30, n);
        }

        [Fact]
        public void Sample02_LayerFollowsPartCodeWithoutSerial()
        {
            Assert.All(Run(0), x => Assert.Equal("check GV", x.Layer));
            Assert.All(Run(1), x => Assert.Equal("check GH", x.Layer));
            Assert.All(Run(2), x => Assert.Equal("check GH", x.Layer));
            Assert.All(Run(3), x => Assert.Equal("check GV", x.Layer));
            Assert.All(Run(2), x => Assert.Equal("DIM15", x.Style));
        }

        [Fact]
        public void PartLayer_StripsTrailingDigitsOnly()
        {
            Dim02Config c = new Dim02Config();
            Assert.Equal("check GV", Dim02Command.PartLayer("GV41", c));
            Assert.Equal("check GH", Dim02Command.PartLayer("GH2", c));
            Assert.Equal("check GH", Dim02Command.PartLayer(" GH ", c));
            Assert.Null(Dim02Command.PartLayer("123", c));
            Assert.Null(Dim02Command.PartLayer("", c));
            Assert.Null(Dim02Command.PartLayer(null, c));
        }

        [Fact]
        public void Sample02_PlateWithBolts_HasHoleChains()
        {
            List<DimSpec> d = Run(2);
            Assert.Equal(5, d.Count(x => x.Tag == "hole-h"));   // goc trai -> 4 lo -> dau phai
            Assert.Equal(3, d.Count(x => x.Tag == "hole-v"));   // chi chuoi ben trai (lo gan canh trai)
            Assert.Equal(0, Run(0).Count(x => x.Tag == "hole-h" || x.Tag == "hole-v"));
        }

        [Fact]
        public void Sample02_EveryArcGetsRadiusDim()
        {
            List<DimSpec> d = Run(0);
            Assert.Equal(2, d.Count(x => x.Kind == DimKind.Radial));
            Assert.All(d.Where(x => x.Kind == DimKind.Radial), x => Assert.True(Pt.Dist(x.Center, x.ChordPoint) > 49.0 && Pt.Dist(x.Center, x.ChordPoint) < 51.0));
        }

        [Fact]
        public void Sample02_ChamferGivesHorizontalAndVerticalLegs()
        {
            List<DimSpec> d = Run(3);
            DimSpec h = d.Single(x => x.Tag == "chamfer-h");
            DimSpec v = d.Single(x => x.Tag == "chamfer-v");
            Assert.Equal(0.0, h.RotationDeg);
            Assert.Equal(90.0, v.RotationDeg);
            Assert.Equal(15.0, System.Math.Abs(h.P1.X - h.P2.X), 1);
            Assert.Equal(15.0, System.Math.Abs(v.P1.Y - v.P2.Y), 1);
        }

        [Fact]
        public void Sample02_HolesNearLeftEdge_ChainOnlyOnLeft()
        {
            Dim02Input input = Sample02.Input(2);
            double xmin = input.Lines.Min(l => System.Math.Min(l.A.X, l.B.X));
            List<DimSpec> v = Run(2).Where(x => x.Tag == "hole-v").ToList();
            Assert.Equal(3, v.Count);
            Assert.All(v, x => Assert.True(x.LinePoint.X < xmin));
        }

        [Fact]
        public void MirroredPlate_HolesNearRightEdge_ChainOnlyOnRight()
        {
            Dim02Input src = Sample02.Input(2);
            Dim02Input m = new Dim02Input();
            m.PartCode = src.PartCode;
            foreach (Seg l in src.Lines) m.Lines.Add(new Seg(new Pt(-l.A.X, l.A.Y), new Pt(-l.B.X, l.B.Y), l.Layer));
            foreach (Pt h in src.Holes) m.Holes.Add(new Pt(-h.X, h.Y));
            CmdResult<List<DimSpec>> r = Dim02Command.Run(m, new Dim02Config());
            Assert.True(r.Ok, r.Message);
            double xmax = m.Lines.Max(l => System.Math.Max(l.A.X, l.B.X));
            List<DimSpec> v = r.Value.Where(x => x.Tag == "hole-v").ToList();
            Assert.Equal(3, v.Count);
            Assert.All(v, x => Assert.True(x.LinePoint.X > xmax));
        }

        [Fact]
        public void Sample02_ScallopPlate_OverallWidthIsBetweenArcCenters()
        {
            DimSpec w = Run(0).Single(x => x.Tag == "overall-w");
            Dim02Input input = Sample02.Input(0);
            double minCx = input.Arcs.Min(a => a.Center.X);
            double maxCx = input.Arcs.Max(a => a.Center.X);
            Assert.Equal(minCx, System.Math.Min(w.P1.X, w.P2.X), 3);
            Assert.Equal(maxCx, System.Math.Max(w.P1.X, w.P2.X), 3);
            // dim dat ra phia canh duoc do: tam cung nam o canh duoi nen dim nam duoi tam
            double ymin = Sample02.Input(0).Lines.Min(l => System.Math.Min(l.A.Y, l.B.Y));
            Assert.True(w.LinePoint.Y < ymin);
            // tam khong co cung: dim rong tong van nam phia canh ngang dai nhat (tam GH3: canh tren)
            double ymax1 = Sample02.Input(1).Lines.Max(l => System.Math.Max(l.A.Y, l.B.Y));
            Assert.True(Run(1).Single(x => x.Tag == "overall-w").LinePoint.Y > ymax1);
            Assert.Equal(0, Run(1).Count(x => x.Tag == "overall-w" && x.P1.Y < 0));
        }

        [Fact]
        public void Sample02_OverallHeightIsOutsideChainOnLeft()
        {
            List<DimSpec> d = Run(2);
            double chain = d.First(x => x.Tag == "hole-v").LinePoint.X;
            double overall = d.Single(x => x.Tag == "overall-h").LinePoint.X;
            Assert.True(overall < chain);
        }

        [Fact]
        public void NullInput_ReturnsFail()
        {
            Assert.False(Dim02Command.Run(null, new Dim02Config()).Ok);
        }

        [Fact]
        public void MissingPartCode_ReturnsFailNotException()
        {
            Dim02Input input = Sample02.Input(1);
            input.PartCode = "";
            CmdResult<List<DimSpec>> r = Dim02Command.Run(input, new Dim02Config());
            Assert.False(r.Ok);
            Assert.False(string.IsNullOrEmpty(r.Message));
        }

        [Fact]
        public void NoOutlineEdges_ReturnsFail()
        {
            Dim02Input input = Sample02.Input(1);
            input.Lines.Clear();
            input.Arcs.Clear();
            Assert.False(Dim02Command.Run(input, new Dim02Config()).Ok);
        }

        [Fact]
        public void NoHorizontalEdge_ReturnsFail()
        {
            Dim02Input input = new Dim02Input();
            input.PartCode = "GH1";
            input.Lines.Add(new Seg(new Pt(0, 0), new Pt(0, 100), "OUT"));
            Assert.False(Dim02Command.Run(input, new Dim02Config()).Ok);
        }

        [Fact]
        public void Registry_ContainsDim02()
        {
            Assert.Contains(CommandRegistry.All, x => x.Code == "DIM02");
        }
    }
}
