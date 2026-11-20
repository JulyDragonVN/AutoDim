using System.Collections.Generic;

namespace AutoDimPanel.Core
{
    public enum DimKind
    {
        /// <summary>Dim thang/xoay (RotatedDimension), goc quay = RotationDeg.</summary>
        Rotated,
        /// <summary>Dim aligned (AlignedDimension).</summary>
        Aligned,
        /// <summary>Dim goc giua hai duong (LineAngularDimension2): A1-A2, B1-B2, ArcPoint.</summary>
        Angular,
        /// <summary>Dim ban kinh (RadialDimension): Center, ChordPoint.</summary>
        Radial
    }

    /// <summary>Mo ta mot dim can tao. Plugin doi thanh doi tuong AutoCAD.</summary>
    public sealed class DimSpec
    {
        public DimKind Kind;
        public string Layer;
        public string Style;
        public Pt P1;
        public Pt P2;
        /// <summary>Diem dat duong dim (DXF code 10).</summary>
        public Pt LinePoint;
        /// <summary>Goc quay (do) - chi dung cho Rotated.</summary>
        public double RotationDeg;
        /// <summary>Angular: duong thu nhat A1-A2, duong thu hai B1-B2, diem dat cung.</summary>
        public Pt A1;
        public Pt A2;
        public Pt B1;
        public Pt B2;
        public Pt ArcPoint;
        /// <summary>Radial: tam cung va diem tren cung.</summary>
        public Pt Center;
        public Pt ChordPoint;
        /// <summary>Nhan nhom dim (de test/debug), khong ghi vao ban ve.</summary>
        public string Tag;
    }

    public sealed class CmdResult<T>
    {
        public bool Ok { get; private set; }
        public string Message { get; private set; }
        public T Value { get; private set; }

        public static CmdResult<T> Success(T value)
        {
            return new CmdResult<T> { Ok = true, Value = value, Message = "" };
        }

        public static CmdResult<T> Fail(string message)
        {
            return new CmdResult<T> { Ok = false, Message = message };
        }
    }

    public sealed class CommandInfo
    {
        public readonly string Code;
        public readonly string Name;
        public readonly string Description;

        public CommandInfo(string code, string name, string description)
        {
            Code = code;
            Name = name;
            Description = description;
        }
    }

    /// <summary>Registry lenh do: palette va DIMLIST tu sinh tu day.</summary>
    public static class CommandRegistry
    {
        private static readonly List<CommandInfo> _all = new List<CommandInfo>
        {
            new CommandInfo("DIM01", "Do tam panel",
                "Do tam panel: bu long, v-stiff, h-stiff, crossbeam, kich thuoc ngoai (chon khung BORDER)"),
            new CommandInfo("DIM02", "Do tam thep OUT",
                "Do tam thep theo bien dang OUT: kich thuoc tong, chan vat, canh xien, cung, lo bu long (chon khung BORDER)")
        };

        public static IList<CommandInfo> All { get { return _all; } }
    }
}
