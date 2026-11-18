namespace AutoDimPanel.Core
{
    /// <summary>Tham so DIM01. Gia tri mac dinh lay theo dim tay trong sample 17/11/2026.</summary>
    public sealed class Dim01Config
    {
        // layer hinh hoc doc vao
        public string LayerCheck = "CHECK";
        public string LayerMark = "MRK1";
        public string LayerBolt = "BOLT";

        // layer dim ghi ra
        public string LayerOut = "check out";
        public string LayerVStiff = "check v-stiff";
        public string LayerHStiff = "check h-stiff";
        public string LayerKote = "check kote";
        public string LayerCrossbeam = "check crossbeam";

        // dimstyle
        public string StyleBig = "DIM50";
        public string StyleSmall = "DIM15";

        // khoang dat duong dim (mm)
        public double OutLeft = 342.2;          // dim tong chieu cao, ben trai canh trai
        public double OutRight = 227.8;         // ben phai canh phai
        public double HStiffEdgeLeft = 189.0;   // chuoi dim h-stiff ben trai
        public double HStiffEdgeRight = 125.1;
        public double VStiffRowBelow = 269.0;   // hang dim v-stiff, duoi diem thap nhat cua canh duoi
        public double KoteRowBelow = 369.6;     // hang dim kote (bu long), duoi diem thap nhat cua canh duoi
        public double KoteColumnOffset = 86.3;  // chuoi dim doc theo cot bu long, lech phai
        public double CrossbeamChainOffset = 80.1; // chuoi dim doc crossbeam, lech phai muc ngoai ban gusset
        public double MemberThicknessBelowCrossbeam = 70.0;
        public double MemberThicknessBelowVStiff = 97.5;
        public double GussetWidthInside = 45.0;
        public double VChainOffset = 66.5;
        public double CornerOffset = 100.0;
        public double PlateThicknessOffset = 60.0;
        public double GapEdgeAbove = 62.8;
        public double GapMemberAbove = 44.5;
        public double GapGussetBelow = 49.0;

        // nguong phan loai
        /// <summary>Thanh dung rong &lt; gia tri nay (mm) coi la loai "crossbeam" (sample: 9 mm), con lai la v-stiff (10 mm).</summary>
        public double ThinMemberMaxWidth = 9.5;
        public double GussetLength = 100.0;
        public double GussetThickness = 10.0;
        /// <summary>Sample khong do khe ho giua dau tam h-stiff cuoi cung va canh phai.</summary>
        public bool GapAtRightEdge = false;
    }
}
