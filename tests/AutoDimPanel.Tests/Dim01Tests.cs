using System.Collections.Generic;
using System.Linq;
using AutoDimPanel.Core;
using Xunit;

namespace AutoDimPanel.Tests
{
    public class Dim01Tests
    {
        private static List<DimSpec> RunSample()
        {
            CmdResult<List<DimSpec>> r = Dim01Command.Run(Sample01.Input(), new Dim01Config());
            Assert.True(r.Ok, r.Message);
            return r.Value;
        }

        [Fact]
        public void Sample01_Generates62Dims()
        {
            Assert.Equal(62, RunSample().Count);
        }

        [Fact]
        public void Sample01_CountPerLayer()
        {
            List<DimSpec> d = RunSample();
            Assert.Equal(2, d.Count(x => x.Layer == "check out"));
            Assert.Equal(5, d.Count(x => x.Layer == "check v-stiff"));
            Assert.Equal(21, d.Count(x => x.Layer == "check h-stiff"));
            Assert.Equal(25, d.Count(x => x.Layer == "check kote"));
            Assert.Equal(9, d.Count(x => x.Layer == "check crossbeam"));
        }

        [Fact]
        public void Sample01_EveryHandDimIsReproduced()
        {
            List<MatchResult> m = DimMatcher.Match(Sample01.Expected(), RunSample(), 1.0);
            string missing = string.Join(",", m.Where(x => x.Found == null).Select(x => x.Expected.Handle));
            Assert.Equal("", missing);
        }

        [Fact]
        public void Sample01_NonHandPlacedLinePointsWithin1mm()
        {
            List<MatchResult> m = DimMatcher.Match(Sample01.Expected(), RunSample(), 1.0);
            string bad = string.Join(",", m.Where(x => x.Found != null && !x.Expected.HandPlaced && x.PosError > 1.0)
                .Select(x => x.Expected.Handle));
            Assert.Equal("", bad);
        }

        [Fact]
        public void Sample01_HandPlacedLinePointsWithin200mm()
        {
            // 15 dim ma nguoi dung keo tay voi khoang cach khac nhau; chi kiem tra khong lech qua xa
            List<MatchResult> m = DimMatcher.Match(Sample01.Expected(), RunSample(), 1.0);
            Assert.True(m.Where(x => x.Found != null && x.Expected.HandPlaced).All(x => x.PosError < 200.0));
        }

        [Fact]
        public void Sample01_KoteHorizontalChainStartsAtBottomLeftCorner()
        {
            Dim01Input input = Sample01.Input();
            List<Pt> bottom = input.CheckPolylines.Select(p => Geo.SortedByX(p)).OrderBy(p => p.Average(q => q.Y)).First();
            List<DimSpec> kote = RunSample().Where(x => x.Tag == "kote-h").OrderBy(x => x.P1.X).ToList();
            Assert.Equal(9, kote.Count);
            Assert.True(Pt.Dist(kote[0].P1, bottom[0]) < 0.01);
            Assert.True(Pt.Dist(kote[8].P2, bottom[bottom.Count - 1]) < 0.01);
            Assert.All(kote, x => Assert.Equal("DIM50", x.Style));
        }

        [Fact]
        public void Sample01_VStiffRowIsBelowLowestBottomPoint()
        {
            List<DimSpec> vs = RunSample().Where(x => x.Tag == "vs").ToList();
            Assert.Equal(4, vs.Count);
            double y0 = vs[0].LinePoint.Y;
            Assert.All(vs, x => Assert.Equal(y0, x.LinePoint.Y, 6));
            Assert.True(y0 < vs.Min(x => x.P1.Y));
        }

        [Fact]
        public void Sample01_CornerBottomAnglesFollowEdge()
        {
            List<DimSpec> c = RunSample().Where(x => x.Tag == "corner" && x.Kind == DimKind.Rotated).OrderBy(x => x.P1.X).ToList();
            Assert.Equal(2, c.Count);
            Assert.Equal(1.3708, c[0].RotationDeg, 3);
            Assert.Equal(181.4524, c[1].RotationDeg, 3);
        }

        [Fact]
        public void NullInput_ReturnsFail()
        {
            Assert.False(Dim01Command.Run(null, new Dim01Config()).Ok);
        }

        [Fact]
        public void MissingCheckPolylines_ReturnsFailNotException()
        {
            Dim01Input input = Sample01.Input();
            input.CheckPolylines.Clear();
            CmdResult<List<DimSpec>> r = Dim01Command.Run(input, new Dim01Config());
            Assert.False(r.Ok);
            Assert.False(string.IsNullOrEmpty(r.Message));
        }

        [Fact]
        public void NoBolts_ReturnsFail()
        {
            Dim01Input input = Sample01.Input();
            input.Bolts.Clear();
            Assert.False(Dim01Command.Run(input, new Dim01Config()).Ok);
        }

        [Fact]
        public void BoltColumnWithOneBolt_ReturnsFail()
        {
            Dim01Input input = Sample01.Input();
            input.Bolts.RemoveAt(0);
            Assert.False(Dim01Command.Run(input, new Dim01Config()).Ok);
        }

        [Fact]
        public void NoStiffenerPlates_ReturnsFail()
        {
            Dim01Input input = Sample01.Input();
            input.Lines.Clear();
            Assert.False(Dim01Command.Run(input, new Dim01Config()).Ok);
        }

        [Fact]
        public void Registry_ContainsDim01()
        {
            Assert.Contains(CommandRegistry.All, x => x.Code == "DIM01");
        }
    }
}
