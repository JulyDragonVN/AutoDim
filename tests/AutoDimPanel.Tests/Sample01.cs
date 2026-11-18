// File sinh tu dong boi tools/gen_sample.py - khong sua tay.
using System.Collections.Generic;
using AutoDimPanel.Core;

namespace AutoDimPanel.Tests
{
    public sealed class ExpectedDim
    {
        public string Handle; public DimKind Kind; public string Layer; public string Style;
        public Pt P1; public Pt P2; public Pt LinePoint; public double RotationDeg; public bool HandPlaced;
    }

    public static class Sample01
    {
        public static Dim01Input Input()
        {
            Dim01Input r = new Dim01Input();
            r.CheckPolylines.Add(new List<Pt> { new Pt(-1752967.185360968, 765806.8023062508), new Pt(-1752467.135360968, 765819.1438062505), new Pt(-1750509.685360968, 765868.5556062507), new Pt(-1749277.185360968, 765900.4557062506), new Pt(-1748394.885360968, 765923.5586062507) });
            r.CheckPolylines.Add(new List<Pt> { new Pt(-1748394.885360968, 764129.1814062507), new Pt(-1749277.185360968, 764106.8104062505), new Pt(-1750509.685360968, 764075.9556062506), new Pt(-1752467.135360968, 764028.0838062506), new Pt(-1752967.185360968, 764016.1175062506) });
            r.Lines.Add(new Seg(new Pt(-1749277.185360968, 764465.5395062506), new Pt(-1748394.885360968, 764488.0569062505), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1749277.185360968, 765541.7267062507), new Pt(-1748394.885360968, 765564.6832062505), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1752967.185360968, 764016.1175062506), new Pt(-1752967.185360968, 765806.8023062508), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1752967.185360968, 764016.1175062506), new Pt(-1752967.185360968, 765806.8023062508), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1752467.135360968, 764028.0838062506), new Pt(-1752467.135360968, 765819.1438062505), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1750509.685360968, 764075.9556062506), new Pt(-1750509.685360968, 765868.5556062507), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1749277.185360968, 764106.8104062505), new Pt(-1749277.185360968, 765900.4557062506), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1752467.135360968, 764386.2958062507), new Pt(-1750509.685360968, 764434.4756062506), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1752867.596760968, 764034.5058062506), new Pt(-1752869.989060968, 764134.4772062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752517.135360968, 765161.5953062506), new Pt(-1752417.135360968, 765161.5953062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752967.185360968, 764374.2545062507), new Pt(-1752467.135360968, 764386.2958062507), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1751441.585360968, 764928.1649062507), new Pt(-1751441.585360968, 764978.1649062504), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1751466.585360968, 764953.1649062507), new Pt(-1751416.585360968, 764953.1649062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1751754.385360968, 764920.5150062506), new Pt(-1751754.385360968, 764970.5150062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1751779.385360968, 764945.5150062506), new Pt(-1751729.385360969, 764945.5150062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752067.185360968, 764912.8651062506), new Pt(-1752067.185360968, 764962.8651062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752092.185360968, 764937.8651062507), new Pt(-1752042.185360968, 764937.8651062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752347.185360968, 764906.0174062507), new Pt(-1752347.185360968, 764956.0174062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752372.185360968, 764931.0174062506), new Pt(-1752322.185360968, 764931.0174062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1751441.585360968, 764328.1649062505), new Pt(-1751441.585360968, 764378.1649062505), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1751466.585360968, 764353.1649062507), new Pt(-1751416.585360968, 764353.1649062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1751754.385360968, 764320.5150062506), new Pt(-1751754.385360968, 764370.5150062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1751779.385360968, 764345.5150062506), new Pt(-1751729.385360969, 764345.5150062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752067.185360968, 764312.8651062507), new Pt(-1752067.185360968, 764362.8651062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752092.185360968, 764337.8651062507), new Pt(-1752042.185360968, 764337.8651062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752347.185360968, 764306.0174062506), new Pt(-1752347.185360968, 764356.0174062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752372.185360968, 764331.0174062506), new Pt(-1752322.185360968, 764331.0174062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750509.685360968, 764434.4756062506), new Pt(-1749277.185360968, 764465.5395062506), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1750841.585360968, 764942.8386062506), new Pt(-1750841.585360968, 764992.8386062505), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750866.585360968, 764967.8386062506), new Pt(-1750816.585360968, 764967.8386062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750841.585360968, 764342.8386062506), new Pt(-1750841.585360968, 764392.8386062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750866.585360968, 764367.8386062505), new Pt(-1750816.585360968, 764367.8386062505), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750241.585360968, 764957.6673062505), new Pt(-1750241.585360968, 765007.6673062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750266.585360968, 764982.6673062507), new Pt(-1750216.585360968, 764982.6673062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750241.585360968, 764357.6673062508), new Pt(-1750241.585360968, 764407.6673062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750266.585360968, 764382.6673062505), new Pt(-1750216.585360968, 764382.6673062505), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749641.585360968, 764972.6879062505), new Pt(-1749641.585360968, 765022.6879062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749666.585360968, 764997.6879062506), new Pt(-1749616.585360968, 764997.6879062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749641.585360968, 764372.6879062507), new Pt(-1749641.585360968, 764422.6879062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749666.585360968, 764397.6879062506), new Pt(-1749616.585360968, 764397.6879062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749041.585360968, 764987.7841062506), new Pt(-1749041.585360968, 765037.7841062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749066.585360969, 765012.7841062506), new Pt(-1749016.585360968, 765012.7841062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749041.585360968, 764387.7841062506), new Pt(-1749041.585360968, 764437.7841062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749066.585360969, 764412.7841062505), new Pt(-1749016.585360968, 764412.7841062505), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752467.135360968, 765460.9318062506), new Pt(-1750509.685360968, 765510.0356062506), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1752867.215760968, 765809.2696062507), new Pt(-1752864.748460968, 765709.3001062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752967.185360968, 765448.6654062506), new Pt(-1752467.135360968, 765460.9318062506), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1750509.685360968, 765510.0356062506), new Pt(-1749277.185360968, 765541.7267062507), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1748394.885360968, 765923.5586062507), new Pt(-1748394.885360968, 764129.1814062507), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1748394.885360968, 764129.1814062507), new Pt(-1748394.885360968, 765923.5586062507), "CHECK"));
            r.Lines.Add(new Seg(new Pt(-1748495.258760968, 764142.6417062506), new Pt(-1748497.793460968, 764242.6096062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1748494.851060968, 765920.9410062508), new Pt(-1748492.233460968, 765820.9753062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750514.185360968, 765868.4420062506), new Pt(-1750514.185360968, 764091.9313062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750505.185360968, 765868.4420062506), new Pt(-1750505.185360968, 764091.9313062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750505.185360968, 765868.4420062506), new Pt(-1750505.185360968, 764091.9313062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752471.635360968, 764444.0464062506), new Pt(-1752471.635360968, 764044.0506062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752462.635360968, 764444.0464062506), new Pt(-1752462.635360968, 764044.0506062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752462.635360968, 764444.0464062506), new Pt(-1752462.635360968, 764044.0506062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752471.635360968, 765819.0328062507), new Pt(-1752471.635360968, 765419.1438062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752462.635360968, 765819.0328062507), new Pt(-1752462.635360968, 765419.1438062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752462.635360968, 765819.0328062507), new Pt(-1752462.635360968, 765419.1438062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752472.135360968, 765409.1438062506), new Pt(-1752472.135360968, 764454.0467062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752462.135360968, 765409.1438062506), new Pt(-1752462.135360968, 764454.0467062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752462.135360968, 765409.1438062506), new Pt(-1752462.135360968, 764454.0467062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752517.135360968, 765409.1438062506), new Pt(-1752417.135360968, 765409.1438062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752517.135360968, 765419.1438062508), new Pt(-1752417.135360968, 765419.1438062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752517.135360968, 765419.1438062508), new Pt(-1752417.135360968, 765419.1438062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749282.185360968, 764122.7892062506), new Pt(-1749282.185360968, 765900.3263062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749272.185360968, 764122.7892062506), new Pt(-1749272.185360968, 765900.3263062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749272.185360968, 764122.7892062506), new Pt(-1749272.185360968, 765900.3263062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752546.890560968, 765449.9727062506), new Pt(-1752956.967660969, 765439.9133062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752547.331960968, 765467.9673062506), new Pt(-1752957.409060968, 765457.9079062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752547.331960968, 765467.9673062506), new Pt(-1752957.409060968, 765457.9079062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750548.986260968, 765501.5474062506), new Pt(-1752386.972460968, 765455.4404062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750549.362460968, 765516.5426062507), new Pt(-1752387.348560968, 765470.4357062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1750549.362460968, 765516.5426062507), new Pt(-1752387.348560968, 765470.4357062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749316.981060968, 765533.2009062506), new Pt(-1750470.004160968, 765503.5534062507), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749317.366560968, 765548.1960062506), new Pt(-1750470.389660968, 765518.5485062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1749317.366560968, 765548.1960062506), new Pt(-1750470.389660968, 765518.5485062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1748404.686860969, 765556.9256062506), new Pt(-1749237.002160968, 765535.2696062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1748405.077060968, 765571.9206062506), new Pt(-1749237.392260968, 765550.2646062505), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1748405.077060968, 765571.9206062506), new Pt(-1749237.392260968, 765550.2646062505), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752517.135360968, 764454.0467062508), new Pt(-1752417.135360968, 764454.0467062508), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752517.135360968, 764444.0467062506), new Pt(-1752417.135360968, 764444.0467062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1752517.135360968, 764444.0467062506), new Pt(-1752417.135360968, 764444.0467062506), "MRK1"));
            r.Lines.Add(new Seg(new Pt(-1748044.685360968, 764138.1196062503), new Pt(-1748044.685360968, 765932.7718062506), "CHECK"));
            r.Bolts.Add(new Pt(-1751441.585360968, 764953.1649062507));
            r.Bolts.Add(new Pt(-1751754.385360968, 764945.5150062506));
            r.Bolts.Add(new Pt(-1752067.185360968, 764937.8651062507));
            r.Bolts.Add(new Pt(-1752347.185360968, 764931.0174062506));
            r.Bolts.Add(new Pt(-1751441.585360968, 764353.1649062507));
            r.Bolts.Add(new Pt(-1751754.385360968, 764345.5150062506));
            r.Bolts.Add(new Pt(-1752067.185360968, 764337.8651062507));
            r.Bolts.Add(new Pt(-1752347.185360968, 764331.0174062506));
            r.Bolts.Add(new Pt(-1750841.585360968, 764967.8386062506));
            r.Bolts.Add(new Pt(-1750841.585360968, 764367.8386062505));
            r.Bolts.Add(new Pt(-1750241.585360968, 764982.6673062507));
            r.Bolts.Add(new Pt(-1750241.585360968, 764382.6673062505));
            r.Bolts.Add(new Pt(-1749641.585360968, 764997.6879062506));
            r.Bolts.Add(new Pt(-1749641.585360968, 764397.6879062506));
            r.Bolts.Add(new Pt(-1749041.585360968, 765012.7841062506));
            r.Bolts.Add(new Pt(-1749041.585360968, 764412.7841062505));
            return r;
        }

        public static List<ExpectedDim> Expected()
        {
            List<ExpectedDim> r = new List<ExpectedDim>();
            r.Add(new ExpectedDim { Handle = "FD", Kind = DimKind.Rotated, Layer = "check v-stiff", Style = "DIM50", P1 = new Pt(-1752967.185360968, 764016.1175062506), P2 = new Pt(-1752467.135360968, 764028.0838062506), LinePoint = new Pt(-1752467.135360968, 763747.1666144677), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "10C", Kind = DimKind.Rotated, Layer = "check v-stiff", Style = "DIM50", P1 = new Pt(-1752467.135360968, 764028.0838062506), P2 = new Pt(-1750509.685360968, 764075.9556062506), LinePoint = new Pt(-1750509.685360968, 763747.1666144677), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "119", Kind = DimKind.Rotated, Layer = "check v-stiff", Style = "DIM50", P1 = new Pt(-1750509.685360968, 764075.9556062506), P2 = new Pt(-1749277.185360968, 764106.8104062505), LinePoint = new Pt(-1749277.185360968, 763747.1666144677), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "126", Kind = DimKind.Rotated, Layer = "check v-stiff", Style = "DIM50", P1 = new Pt(-1749277.185360968, 764106.8104062505), P2 = new Pt(-1748044.685360968, 764138.1196062503), LinePoint = new Pt(-1748044.685360968, 763747.1666144677), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "134", Kind = DimKind.Aligned, Layer = "check out", Style = "DIM50", P1 = new Pt(-1748394.885360968, 764129.1814062507), P2 = new Pt(-1748394.885360968, 765923.5586062507), LinePoint = new Pt(-1748167.146766407, 765923.5586062509), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "142", Kind = DimKind.Aligned, Layer = "check out", Style = "DIM50", P1 = new Pt(-1752967.185360968, 764016.1175062506), P2 = new Pt(-1752967.185360968, 765806.8023062508), LinePoint = new Pt(-1753309.436117263, 765806.8023062509), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "14F", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM50", P1 = new Pt(-1752967.185360968, 765806.8023062508), P2 = new Pt(-1752967.185360968, 765448.6654062506), LinePoint = new Pt(-1753156.235834816, 765448.6654062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "15D", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM50", P1 = new Pt(-1752967.185360968, 765448.6654062506), P2 = new Pt(-1752967.185360968, 764016.1175062506), LinePoint = new Pt(-1753156.235834817, 764016.1175062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "16A", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM50", P1 = new Pt(-1748394.885360968, 765923.5586062507), P2 = new Pt(-1748394.885360968, 765564.6832062505), LinePoint = new Pt(-1748269.84538063, 765564.6832062507), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "177", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM50", P1 = new Pt(-1748394.885360968, 765564.6832062505), P2 = new Pt(-1748394.885360968, 764129.1814062507), LinePoint = new Pt(-1748269.845380631, 764129.1814062508), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "184", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752462.635360968, 765419.1438062508), P2 = new Pt(-1752462.635360968, 765819.0328062507), LinePoint = new Pt(-1752336.986881899, 765819.0328062508), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "192", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752462.635360968, 765419.1438062508), P2 = new Pt(-1752417.135360968, 765409.1438062506), LinePoint = new Pt(-1752336.9868819, 765409.143806251), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "1A4", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752417.135360968, 765409.1438062506), P2 = new Pt(-1752417.135360968, 764454.0467062508), LinePoint = new Pt(-1752336.986881901, 764454.0467062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "1B1", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752417.135360968, 764454.0467062508), P2 = new Pt(-1752417.135360968, 764444.0467062506), LinePoint = new Pt(-1752336.986881902, 764444.0467062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "1C3", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752417.135360968, 764444.0467062506), P2 = new Pt(-1752462.635360968, 764044.0506062506), LinePoint = new Pt(-1752336.986881902, 764044.050606251), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "1D0", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752417.135360968, 765409.1438062506), P2 = new Pt(-1752517.135360968, 765409.1438062506), LinePoint = new Pt(-1752517.135360968, 765361.1833378061), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "1DD", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752417.135360968, 764454.0467062508), P2 = new Pt(-1752517.135360968, 764454.0467062508), LinePoint = new Pt(-1752517.135360968, 764497.6182548411), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "1EA", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1752471.635360968, 764044.0506062506), P2 = new Pt(-1752462.635360968, 764044.0506062506), LinePoint = new Pt(-1752462.635360968, 763975.1051881124), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "1F9", Kind = DimKind.Rotated, Layer = "check crossbeam", Style = "DIM15", P1 = new Pt(-1750514.185360968, 764091.9313062507), P2 = new Pt(-1750505.185360968, 764091.9313062507), LinePoint = new Pt(-1750505.185360968, 764018.0839696843), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "208", Kind = DimKind.Rotated, Layer = "check v-stiff", Style = "DIM15", P1 = new Pt(-1749282.185360968, 764122.7892062506), P2 = new Pt(-1749272.185360968, 764122.7892062506), LinePoint = new Pt(-1749272.185360968, 764025.358634982), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "217", Kind = DimKind.Aligned, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1752956.967660969, 765439.9133062506), P2 = new Pt(-1752957.409060968, 765457.9079062507), LinePoint = new Pt(-1753039.998075022, 765455.8820323372), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "225", Kind = DimKind.Aligned, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1750548.986260968, 765501.5474062506), P2 = new Pt(-1750549.362460968, 765516.5426062507), LinePoint = new Pt(-1750612.160888373, 765514.9671175348), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "234", Kind = DimKind.Aligned, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1750470.004160968, 765503.5534062507), P2 = new Pt(-1750470.389660968, 765518.5485062506), LinePoint = new Pt(-1750425.890193197, 765519.6925162826), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "243", Kind = DimKind.Aligned, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1749237.002160968, 765535.2696062506), P2 = new Pt(-1749237.392260968, 765550.2646062505), LinePoint = new Pt(-1749198.465437439, 765551.2773007398), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "252", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1750509.685360968, 765868.5556062507), P2 = new Pt(-1750509.685360968, 765510.0356062506), LinePoint = new Pt(-1750443.202203294, 765510.0356062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "25F", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1750509.685360968, 765510.0356062506), P2 = new Pt(-1750509.685360968, 764075.9556062506), LinePoint = new Pt(-1750443.202203295, 764075.9556062507), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "26C", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1749282.185360968, 765541.8117469402), P2 = new Pt(-1749317.180310968, 765542.4069562505), LinePoint = new Pt(-1749317.180310968, 765602.2266936452), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "279", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1749272.185360968, 765541.6372561282), P2 = new Pt(-1749237.190960968, 765541.0112562506), LinePoint = new Pt(-1749237.190960968, 765601.7766223039), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "286", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1750549.185360959, 765510.005806251), P2 = new Pt(-1750514.185360959, 765510.0322122124), LinePoint = new Pt(-1750514.185360959, 765554.5239078936), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "293", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1750505.185360959, 765510.034729327), P2 = new Pt(-1750470.185360959, 765510.0279062509), LinePoint = new Pt(-1750470.185360959, 765554.5239078936), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "2A0", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1752517.135360968, 765419.1438062508), P2 = new Pt(-1752547.135260968, 765464.6240062508), LinePoint = new Pt(-1752547.135260968, 765368.808049061), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "2AD", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1752417.135360968, 765419.1438062508), P2 = new Pt(-1752387.135760969, 765464.9852562506), LinePoint = new Pt(-1752387.135760969, 765371.0086610421), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "2BA", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1752957.188360969, 765448.9106062506), P2 = new Pt(-1752967.185360968, 765448.6654062506), LinePoint = new Pt(-1752967.185360968, 765511.7545127517), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "2C9", Kind = DimKind.Aligned, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1752967.185360968, 765806.8023062508), P2 = new Pt(-1752867.215760968, 765809.2696062507), LinePoint = new Pt(-1752871.032125709, 765963.9003591696), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "2D6", Kind = DimKind.Aligned, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1748494.851060968, 765920.9410062508), P2 = new Pt(-1748394.885360968, 765923.5586062507), LinePoint = new Pt(-1748400.954087531, 766155.3222442988), RotationDeg = 0.0, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "2E3", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1752967.185360968, 764016.1175062506), P2 = new Pt(-1752867.596760968, 764034.5058062506), LinePoint = new Pt(-1752864.959311787, 763924.2915827137), RotationDeg = 1.370838230370547, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "2F0", Kind = DimKind.Rotated, Layer = "check h-stiff", Style = "DIM15", P1 = new Pt(-1748495.258760968, 764142.6417062506), P2 = new Pt(-1748394.885360968, 764129.1814062507), LinePoint = new Pt(-1748392.707361862, 764043.2823131664), RotationDeg = 181.4524416986232, HandPlaced = true });
            r.Add(new ExpectedDim { Handle = "2FD", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1752967.185360968, 764016.1175062506), P2 = new Pt(-1752347.185360968, 764931.0174062506), LinePoint = new Pt(-1752347.185360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "30A", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1752347.185360968, 764931.0174062506), P2 = new Pt(-1752067.185360968, 764937.8651062507), LinePoint = new Pt(-1752067.185360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "317", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1752067.185360968, 764937.8651062507), P2 = new Pt(-1751754.385360968, 764945.5150062506), LinePoint = new Pt(-1751754.385360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "324", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1751754.385360968, 764945.5150062506), P2 = new Pt(-1751441.585360968, 764953.1649062507), LinePoint = new Pt(-1751441.585360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "331", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1751441.585360968, 764953.1649062507), P2 = new Pt(-1750841.585360968, 764967.8386062506), LinePoint = new Pt(-1750841.585360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "33E", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1750841.585360968, 764967.8386062506), P2 = new Pt(-1750241.585360968, 764982.6673062507), LinePoint = new Pt(-1750241.585360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "34B", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1750241.585360968, 764982.6673062507), P2 = new Pt(-1749641.585360968, 764997.6879062506), LinePoint = new Pt(-1749641.585360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "358", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1749641.585360968, 764997.6879062506), P2 = new Pt(-1749041.585360968, 765012.7841062506), LinePoint = new Pt(-1749041.585360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "365", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM50", P1 = new Pt(-1749041.585360968, 765012.7841062506), P2 = new Pt(-1748394.885360968, 764129.1814062507), LinePoint = new Pt(-1748394.885360968, 763646.55589543), RotationDeg = 0.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "372", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1752347.185360968, 764931.0174062506), P2 = new Pt(-1752347.185360968, 764331.0174062506), LinePoint = new Pt(-1752260.915794526, 764331.0174062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "37F", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1752347.185360968, 764331.0174062506), P2 = new Pt(-1752347.185360968, 764031.0173281338), LinePoint = new Pt(-1752260.915794527, 764031.0173281342), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "38C", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1752067.185360968, 764937.8651062507), P2 = new Pt(-1752067.185360968, 764337.8651062507), LinePoint = new Pt(-1751980.915794528, 764337.8651062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "399", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1752067.185360968, 764337.8651062507), P2 = new Pt(-1752067.185360968, 764037.8650281338), LinePoint = new Pt(-1751980.915794529, 764037.865028134), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "3A6", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1751754.385360968, 764945.5150062506), P2 = new Pt(-1751754.385360968, 764345.5150062506), LinePoint = new Pt(-1751668.115794528, 764345.5150062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "3B3", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1751754.385360968, 764345.5150062506), P2 = new Pt(-1751754.385360968, 764045.5149281335), LinePoint = new Pt(-1751668.115794529, 764045.5149281338), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "3C0", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1751441.585360968, 764953.1649062507), P2 = new Pt(-1751441.585360968, 764353.1649062507), LinePoint = new Pt(-1751355.315794528, 764353.1649062509), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "3CD", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1751441.585360968, 764353.1649062507), P2 = new Pt(-1751441.585360968, 764053.1648281338), LinePoint = new Pt(-1751355.315794528, 764053.164828134), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "3DA", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1750841.585360968, 764967.8386062506), P2 = new Pt(-1750841.585360968, 764367.8386062505), LinePoint = new Pt(-1750755.315794528, 764367.8386062507), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "3E7", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1750841.585360968, 764367.8386062505), P2 = new Pt(-1750841.585360968, 764067.8385281337), LinePoint = new Pt(-1750755.315794529, 764067.8385281339), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "3F4", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1750241.585360968, 764982.6673062507), P2 = new Pt(-1750241.585360968, 764382.6673062505), LinePoint = new Pt(-1750155.315794527, 764382.6673062507), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "401", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1750241.585360968, 764382.6673062505), P2 = new Pt(-1750241.585360968, 764082.6672281337), LinePoint = new Pt(-1750155.315794528, 764082.6672281339), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "40E", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1749641.585360968, 764997.6879062506), P2 = new Pt(-1749641.585360968, 764397.6879062506), LinePoint = new Pt(-1749555.315794528, 764397.6879062508), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "41B", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1749641.585360968, 764397.6879062506), P2 = new Pt(-1749641.585360968, 764097.6878281337), LinePoint = new Pt(-1749555.315794528, 764097.687828134), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "428", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1749041.585360968, 765012.7841062506), P2 = new Pt(-1749041.585360968, 764412.7841062505), LinePoint = new Pt(-1748955.315794528, 764412.7841062507), RotationDeg = 90.0, HandPlaced = false });
            r.Add(new ExpectedDim { Handle = "435", Kind = DimKind.Rotated, Layer = "check kote", Style = "DIM15", P1 = new Pt(-1749041.585360968, 764412.7841062505), P2 = new Pt(-1749041.585360968, 764112.7840281338), LinePoint = new Pt(-1748955.315794528, 764112.7840281338), RotationDeg = 90.0, HandPlaced = false });
            return r;
        }
    }
}
