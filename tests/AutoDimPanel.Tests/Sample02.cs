// File sinh tu dong boi tools/gen_sample02.py - khong sua tay.
using System.Collections.Generic;
using AutoDimPanel.Core;

namespace AutoDimPanel.Tests
{
    public sealed class Expected02
    {
        public string Handle; public DimKind Kind; public Pt P1; public Pt P2; public Pt LinePoint; public double RotationDeg;
        public Pt A1; public Pt A2; public Pt B1; public Pt B2; public Pt Center;
    }

    public static class Sample02
    {
        public const int PlateCount = 4;

        public static Dim02Input Input(int plate)
        {
            Dim02Input r = new Dim02Input();
            switch (plate)
            {
                case 0:
                    r.PartCode = "GV41";
                    r.Lines.Add(new Seg(new Pt(-1855249.510796137, 718764.7862033376), new Pt(-1853596.310796137, 718764.7862033376), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1853543.067196137, 718894.7862033376), new Pt(-1855299.510796137, 718894.7862033376), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1855299.510796137, 718894.7862033376), new Pt(-1855299.510796137, 718814.7862033378), "OUT"));
                    r.Arcs.Add(new ArcSeg(new Pt(-1855299.510796137, 718764.7862033376), 50.0, 0.0, 90.0));
                    r.Arcs.Add(new ArcSeg(new Pt(-1853546.310796137, 718764.7862033376), 50.0, 88.57066461573643, 180.0));
                    r.Lines.Add(new Seg(new Pt(-1853545.063696136, 718814.7706033376), new Pt(-1853543.067196137, 718894.7862033376), "OUT"));
                    break;
                case 1:
                    r.PartCode = "GH3";
                    r.Lines.Add(new Seg(new Pt(-1853583.663959817, 720738.4539862762), new Pt(-1852583.279159817, 720738.4539862762), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852323.471559817, 720898.453986276), new Pt(-1853843.471559817, 720898.453986276), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1853843.471559817, 720888.4539862762), new Pt(-1853583.663959817, 720738.4539862762), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1853843.471559817, 720898.453986276), new Pt(-1853843.471559817, 720888.4539862762), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852583.279159817, 720738.4539862762), new Pt(-1852323.471559817, 720888.4539862762), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852323.471559817, 720888.4539862762), new Pt(-1852323.471559817, 720898.453986276), "OUT"));
                    break;
                case 2:
                    r.PartCode = "GH2";
                    r.Lines.Add(new Seg(new Pt(-1855298.471559817, 720318.4539862764), new Pt(-1855088.279159817, 720318.4539862764), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1854828.471559817, 720478.453986276), new Pt(-1855298.471559817, 720478.453986276), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1855298.471559817, 720478.453986276), new Pt(-1855298.471559817, 720318.4539862764), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1855088.279159817, 720318.4539862764), new Pt(-1854828.471559817, 720468.4539862764), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1854828.471559817, 720468.4539862764), new Pt(-1854828.471559817, 720478.453986276), "OUT"));
                    r.Holes.Add(new Pt(-1855258.721609817, 720363.453986276));
                    r.Holes.Add(new Pt(-1855138.721609817, 720363.453986276));
                    r.Holes.Add(new Pt(-1855198.721609817, 720398.4539862763));
                    r.Holes.Add(new Pt(-1855078.721609817, 720398.4539862763));
                    break;
                case 3:
                    r.PartCode = "GV9";
                    r.Lines.Add(new Seg(new Pt(-1852047.151481785, 718280.0450472425), new Pt(-1852307.151485866, 718540.0450633155), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852897.557663294, 718540.0450072503), new Pt(-1852897.172714045, 718267.880927624), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852882.151498046, 718252.8809412629), new Pt(-1852047.151498046, 718252.880865281), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852047.151498046, 718252.880865281), new Pt(-1852047.151481785, 718280.0450472425), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852897.172714045, 718267.880927624), new Pt(-1852882.151498046, 718252.8809412629), "OUT"));
                    r.Lines.Add(new Seg(new Pt(-1852307.151485866, 718540.0450633155), new Pt(-1852897.557663294, 718540.0450072503), "OUT"));
                    break;
            }
            return r;
        }

        public static List<Expected02> Expected(int plate)
        {
            List<Expected02> r = new List<Expected02>();
            switch (plate)
            {
                case 0:
                    r.Add(new Expected02 { Handle = "19B", Kind = DimKind.Rotated, P1 = new Pt(-1855299.510796137, 718894.7862033377), P2 = new Pt(-1853543.067196137, 718894.7862033377), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "1A8", Kind = DimKind.Rotated, P1 = new Pt(-1855299.510796137, 718894.7862033377), P2 = new Pt(-1855249.510796137, 718764.7862033377), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "1B5", Kind = DimKind.Radial, Center = new Pt(-1855299.510796136, 718764.7862033373) });
                    r.Add(new Expected02 { Handle = "1C0", Kind = DimKind.Radial, Center = new Pt(-1853546.310796138, 718764.7862033383) });
                    r.Add(new Expected02 { Handle = "1CA", Kind = DimKind.Rotated, P1 = new Pt(-1853543.067196137, 718894.7862033377), P2 = new Pt(-1853546.310796138, 718764.7862033383), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "1D9", Kind = DimKind.Angular, A1 = new Pt(-1853545.063696137, 718814.7706033376), A2 = new Pt(-1853543.067196137, 718894.7862033377), B1 = new Pt(-1855249.510796137, 718764.7862033377), B2 = new Pt(-1853596.310796137, 718764.7862033377) });
                    r.Add(new Expected02 { Handle = "23A", Kind = DimKind.Angular, A1 = new Pt(-1855249.510796137, 718764.7862033377), A2 = new Pt(-1853596.310796137, 718764.7862033377), B1 = new Pt(-1855299.510796137, 718894.7862033377), B2 = new Pt(-1855299.510796137, 718814.7862033378) });
                    break;
                case 1:
                    r.Add(new Expected02 { Handle = "130", Kind = DimKind.Angular, A1 = new Pt(-1852323.471559817, 720898.453986276), A2 = new Pt(-1853843.471559817, 720898.453986276), B1 = new Pt(-1853843.471559817, 720888.4539862762), B2 = new Pt(-1853583.663959817, 720738.4539862762) });
                    r.Add(new Expected02 { Handle = "13D", Kind = DimKind.Angular, A1 = new Pt(-1852583.279159817, 720738.4539862762), A2 = new Pt(-1852323.471559817, 720888.4539862762), B1 = new Pt(-1852323.471559817, 720898.453986276), B2 = new Pt(-1853843.471559817, 720898.453986276) });
                    r.Add(new Expected02 { Handle = "14A", Kind = DimKind.Rotated, P1 = new Pt(-1853843.471559817, 720888.4539862762), P2 = new Pt(-1853843.471559817, 720898.453986276), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "159", Kind = DimKind.Rotated, P1 = new Pt(-1853843.471559817, 720898.453986276), P2 = new Pt(-1853583.663959817, 720738.4539862762), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "166", Kind = DimKind.Rotated, P1 = new Pt(-1852323.471559817, 720898.453986276), P2 = new Pt(-1852323.471559817, 720888.4539862762), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "175", Kind = DimKind.Rotated, P1 = new Pt(-1852323.471559817, 720898.453986276), P2 = new Pt(-1853843.471559817, 720898.453986276), RotationDeg = 0.0 });
                    break;
                case 2:
                    r.Add(new Expected02 { Handle = "AA", Kind = DimKind.Rotated, P1 = new Pt(-1855298.471559817, 720318.4539862762), P2 = new Pt(-1855258.721609817, 720363.453986276), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "B9", Kind = DimKind.Rotated, P1 = new Pt(-1855258.721609817, 720363.453986276), P2 = new Pt(-1855198.721609817, 720398.4539862762), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "C6", Kind = DimKind.Rotated, P1 = new Pt(-1855198.721609817, 720398.4539862762), P2 = new Pt(-1855138.721609817, 720363.453986276), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "D3", Kind = DimKind.Rotated, P1 = new Pt(-1855138.721609817, 720363.453986276), P2 = new Pt(-1855078.721609817, 720398.4539862762), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "E0", Kind = DimKind.Rotated, P1 = new Pt(-1855298.471559817, 720318.4539862762), P2 = new Pt(-1855258.721609817, 720363.453986276), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "ED", Kind = DimKind.Rotated, P1 = new Pt(-1855258.721609817, 720363.453986276), P2 = new Pt(-1855198.721609817, 720398.4539862762), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "FA", Kind = DimKind.Rotated, P1 = new Pt(-1855198.721609817, 720398.4539862762), P2 = new Pt(-1855298.471559817, 720478.453986276), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "107", Kind = DimKind.Rotated, P1 = new Pt(-1854828.471559817, 720478.453986276), P2 = new Pt(-1855298.471559817, 720478.453986276), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "114", Kind = DimKind.Rotated, P1 = new Pt(-1854828.471559817, 720478.453986276), P2 = new Pt(-1854828.471559817, 720468.4539862762), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "123", Kind = DimKind.Angular, A1 = new Pt(-1854828.471559817, 720478.453986276), A2 = new Pt(-1855298.471559817, 720478.453986276), B1 = new Pt(-1855088.279159817, 720318.4539862762), B2 = new Pt(-1854828.471559817, 720468.4539862762) });
                    r.Add(new Expected02 { Handle = "182", Kind = DimKind.Rotated, P1 = new Pt(-1855298.471559817, 720478.453986276), P2 = new Pt(-1855298.471559817, 720318.4539862762), RotationDeg = 90.0 });
                    break;
                case 3:
                    r.Add(new Expected02 { Handle = "1E7", Kind = DimKind.Rotated, P1 = new Pt(-1852882.151498046, 718252.880941263), P2 = new Pt(-1852897.172714045, 718267.880927624), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "1F6", Kind = DimKind.Rotated, P1 = new Pt(-1852882.151498046, 718252.880941263), P2 = new Pt(-1852897.172714045, 718267.880927624), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "205", Kind = DimKind.Rotated, P1 = new Pt(-1852882.151498046, 718252.880941263), P2 = new Pt(-1852897.557663294, 718540.0450072502), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "212", Kind = DimKind.Rotated, P1 = new Pt(-1852897.172714045, 718267.880927624), P2 = new Pt(-1852047.151498046, 718252.880865281), RotationDeg = 0.0 });
                    r.Add(new Expected02 { Handle = "21F", Kind = DimKind.Rotated, P1 = new Pt(-1852047.151481785, 718280.0450472425), P2 = new Pt(-1852047.151498046, 718252.880865281), RotationDeg = 90.0 });
                    r.Add(new Expected02 { Handle = "22D", Kind = DimKind.Angular, A1 = new Pt(-1852882.151498046, 718252.880941263), A2 = new Pt(-1852047.151498046, 718252.880865281), B1 = new Pt(-1852047.151481785, 718280.0450472425), B2 = new Pt(-1852307.151485866, 718540.0450633155) });
                    break;
            }
            return r;
        }
    }
}
