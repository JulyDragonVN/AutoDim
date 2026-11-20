namespace AutoDimPanel.Core
{
    /// <summary>Tham so DIM02. Gia tri mac dinh lay theo sample 19/11/2026 (4 tam GH2, GH3, GV9, GV41).</summary>
    public sealed class Dim02Config
    {
        // doc vao
        public string LayerOut = "OUT";
        /// <summary>Layer khung tam; lenh chon nhieu khung bang selection loc theo layer nay.</summary>
        public string LayerBorder = "BORDER";
        /// <summary>Layer lo bu long (ghi bang ma Unicode de khong loi code page): ten layer tieng Nhat trong sample va BOLT.</summary>
        public string[] LayerBolts = { "\u30dc\u30eb\u30c8\u5b54", "BOLT" };
        /// <summary>Text chua ma chi tiet trong khung, vi du "P=GV41".</summary>
        public string PartTextPrefix = "P=";

        // ghi ra
        public string LayerPrefix = "check ";
        public string Style = "DIM15";

        // khoang dat duong dim tinh tu bbox moi phia (mm): tang 1, moi tang sau cong them TierStep
        public double Tier1 = 50.0;
        public double TierStep = 45.0;

        // hinh hoc
        /// <summary>Ban kinh dat cung cua dim goc (mm).</summary>
        public double AngleArcRadius = 150.0;
        /// <summary>Canh xien dai &lt;= gia tri nay coi la chan vat (mm).</summary>
        public double ChamferMaxLength = 50.0;
        public double Tol = 1.0;
        /// <summary>Canh lech truc &lt;= gia tri nay (do) coi la ngang/dung.</summary>
        public double AxisTolDeg = 3.0;
    }
}
