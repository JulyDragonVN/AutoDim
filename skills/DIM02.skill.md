# DIM02 – Đo tấm thép theo biên dạng OUT (nguyên mẫu)

## Mục đích
Đặt dim cho từng tấm thép: kích thước tổng, chân vát, cạnh xiên (góc + cạnh đứng ngắn), cung (bán kính, góc), lỗ bulong.

## Đầu vào mong đợi
- Tách tấm theo khung `BORDER` (LWPOLYLINE chữ nhật). Mỗi tấm: LINE/ARC layer `OUT` + CIRCLE (layer lỗ bulong) nằm trong khung.
- Cách chọn: người dùng **chọn nhiều khung `BORDER`** bằng selection (lọc LWPOLYLINE layer BORDER); lệnh xử lý lần lượt từng khung, khung lỗi thì báo và bỏ qua, các khung còn lại vẫn đo.
- Mã chi tiết lấy từ TEXT `P=...` (layer BUZAI-INFO) nằm trong khung, ví dụ `P=GV41`.

## Quy tắc đo (mỗi tấm; TOL = 1 mm, cạnh ngang/đứng = lệch trục ≤ 3°)
Định nghĩa: bbox của các đỉnh OUT; "cạnh ngang dài nhất" L; phía rộng tổng W = phía (trên/dưới) chứa L.
1. **Chân vát** (cạnh xiên dài ≤ 50 mm): 1 dim ngang + 1 dim đứng, bám hai đầu mút cạnh vát.
2. **Cạnh xiên lớn** (không ngang/đứng, dài > 50):
   - dim góc giữa L và cạnh xiên (điểm cung nằm trong tấm, về phía trọng tâm);
   - dim dài cho cạnh đứng chung đầu mút với cạnh xiên, đặt phía trái/phải theo vị trí cạnh.
3. **Cung** (mọi ARC trên OUT đều đo): dim bán kính (điểm chọn ở giữa cung); nếu có cạnh đứng chung đầu mút: dim góc giữa cạnh đứng đó và cạnh ngang chung đầu mút kia của cung; nếu đầu xa của cạnh đứng lệch ngang với tâm cung > 1 mm: dim ngang (đầu xa → tâm cung) ở phía đối diện W.
4. **Lỗ bulong** (chỉ khi tấm có lỗ; không có lỗ thì không có chuỗi): chuỗi ngang ở phía đối diện với phía đặt dim rộng tổng: góc trái-dưới (hoặc trái-trên) → từng tâm lỗ theo x tăng dần → đầu phải. Chuỗi đứng chỉ đo **một bên**, bên có lỗ gần cạnh hơn (khoảng cách từ lỗ trái nhất tới cạnh trái ≤ khoảng cách từ lỗ phải nhất tới cạnh phải → bên trái, ngược lại → bên phải):
   - bên trái: góc trái-dưới → mỗi mức y của tâm lỗ (lấy lỗ trái nhất mỗi mức, y tăng dần) → góc trái-trên;
   - bên phải: góc phải-dưới → mỗi mức y của tâm lỗ (lấy lỗ phải nhất mỗi mức, y tăng dần) → góc phải-trên.
5. **Kích thước tổng** (đặt sau cùng, tầng ngoài):
   - Rộng: giữa đỉnh trái nhất và phải nhất, chọn đỉnh gần phía W; đặt phía W. Nếu 2 góc trái phải có cung scallop thì đo giữa 2 tâm cung, và dim đặt ra phía outline chứa hai tâm cung (cạnh được đo), không đặt phía W.
   - Cao: ở bên trái, từ đỉnh trên cùng trái nhất xuống đỉnh dưới cùng trái nhất.

## Cách đặt
- Layer dim = `check ` + mã chi tiết bỏ hậu tố số seri (GV9, GV41 → `check GV`; GH2, GH3 → `check GH`); thiếu thì tạo. Thiếu text `P=` thì báo lỗi. Dimstyle `DIM15`, loại: thẳng/xoay (góc 0 hoặc 90), góc 2 đường, bán kính; text tự đo.
- Khoảng cách đường dim tính từ bbox theo từng phía (T/B/L/R): tầng 1 = 50 mm, mỗi tầng sau +45 mm (chỉnh qua DIMCFG).
- Góc quay 90 và 270 coi như nhau (mẫu có cả hai, chuỗi trái của tấm có lỗ dùng 270).

## Điều kiện/giả định
WCS, mm, tấm nằm ngang; cạnh ngang/đứng cho phép lệch ≤ 3° (mẫu có cạnh lệch 0,4 mm và 1,4°).

## Chưa làm / bỏ qua
Không dim đường kính lỗ; không dim cạnh đứng dài không chung đầu mút với cạnh xiên; không dim đáy ngắn khi không phải cạnh dài nhất.

## Điểm đang đoán (cần xác nhận)
1. Vị trí đường dim: mẫu đặt tay lệch nhau 45–124 mm, **không có quy tắc**; dùng 50 + 45/tầng (lệch 1–74 mm so với mẫu).
2. Lỗ bulong chỉ có ở 1 tấm → phía đặt chuỗi và "lỗ trái nhất mỗi mức y" là suy từ 1 ví dụ.
3. Cung nào cũng có dim bán kính (đã chốt); dim góc/ngang lệch kèm theo chỉ khi có cạnh đứng chung đầu mút (suy từ 1 tấm).
4. Ngưỡng chân vát ≤ 50 mm (đã chốt).
5. (Đã chốt) Layer theo mã chi tiết; mẫu đặt tất cả vào `check h-stiff` nên layer này khác mẫu có chủ đích.
6. Chọn bên chuỗi đứng theo "lỗ gần cạnh nào": so khoảng cách lỗ trái nhất–cạnh trái với lỗ phải nhất–cạnh phải (bằng nhau → trái). Góc phải-dưới/phải-trên = đỉnh có x lớn nhất, thấp nhất/cao nhất. Chưa có mẫu tấm bulong gần cạnh phải để đối chiếu.
7. "Scallop" = có ít nhất một cung có tâm nằm bên trái và một cung bên phải trọng tâm tấm; lấy cung có tâm x nhỏ nhất và x lớn nhất.
8. Điểm chọn của dim bán kính và vị trí cung của dim góc: mẫu đặt tay, không quy tắc.

## Kiểm chứng
Mẫu: origin.dxf + dim.dxf (4 tấm GH2, GH3, GV9, GV41; 30 dim, dimstyle DIM15).
- Nguyên mẫu Python `prototypes/dim02_proto.py`: **29/30 khớp** theo quy tắc mới (19B đổi sang đo giữa 2 tâm cung), thêm 2 dim mới (dim tổng GV41 và dim tới đầu phải của GH2); 6/6 dim góc đo đúng giá trị (30, 30, 30, 45, 90, 91,43°).
- Core C# (`Dim02Command`) biên dịch bằng Mono `mcs` và chạy với tọa độ thật: 29/30 khớp dim tay + đúng 2 dim mới như dự kiến (dim tổng GV41 đổi, dim tới đầu phải của GH2); 17 test (bản test xUnit chạy qua lớp giả lập Assert, chưa chạy bằng xUnit thật). Layer sinh ra: check GV / check GH.
- Không khớp: vị trí đường dim (xem điểm đoán 1) và layer (đổi có chủ đích theo mã chi tiết).
- Plugin (Dim02Reader, DimWriter, Commands) **chưa biên dịch** (không có AutoCAD API). Cần xác nhận khi build: `LineAngularDimension2`, `RadialDimension` đúng chữ ký; dim góc ra đúng 30° (không phải 150°).

## Lịch sử thay đổi
- 19/11/2026: tạo từ mẫu 4 tấm (chưa có code C#).
- 19/11/2026: viết Core C# + test + Plugin.
- 20/11/2026: chuỗi đứng của lỗ chỉ đo 1 bên (bên lỗ gần cạnh); dim rộng tổng scallop đặt ra phía cạnh chứa 2 tâm cung; DIM01 cũng chọn nhiều BORDER.
- 20/11/2026: chọn nhiều BORDER; sửa mục 4 (chuỗi tới đầu phải, thêm chuỗi đứng bên phải) và mục 5 (rộng tổng đo giữa 2 tâm cung scallop).
- 19/11/2026: chốt chọn khung BORDER, layer theo mã chi tiết, mọi cung đều đo, ngưỡng vát 50.
