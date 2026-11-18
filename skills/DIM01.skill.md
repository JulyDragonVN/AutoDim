# DIM01 – Đo tấm panel

## Mục đích
Đặt toàn bộ dim kiểm tra (layer `check …`) cho một tấm panel trong bản vẽ chế tạo: kích thước ngoài, v-stiff, h-stiff, crossbeam và bu lông (kote). Sample 17/11/2026: tấm `G1-B7WR` (T=16, SM490YA), **62 dim** đặt tay.

Nhãn palette: **DIM01 – Đo tấm panel**.

## Đầu vào mong đợi
Người dùng chọn **một polyline layer `BORDER`** (khung tấm). Lệnh đọc các đối tượng nằm trong khung:

| Layer | Loại | Ý nghĩa trong sample |
|---|---|---|
| `CHECK` | 2 polyline mở, 5 đỉnh | cạnh dưới và cạnh trên của tấm (gãy khúc theo độ dốc) |
| `CHECK` | line dọc, dài > 1000 | vị trí v-stiff: x = 832.3, 1332.4, 3289.8, 4522.3 và 5404.6 (cạnh phải, 2 line chồng nhau), 5754.8 (nằm ngoài tấm, kéo dài độ dốc cạnh dưới) |
| `CHECK` | line ngang/xiên dài > 400 | hàng đường tâm h-stiff; có 2 hàng (trên, dưới), mỗi hàng 4 đoạn |
| `MRK1` | line xiên dài > 300 | tấm h-stiff: 3 line/tấm (mặt trên vẽ 2 lần, mặt dưới 1 lần); sample có 4 tấm (hàng trên) |
| `MRK1` | line dọc dài > 300 | thanh đứng v-stiff/crossbeam: 3 line (2 mặt + 1 lần chồng) |
| `MRK1` | line ngang dài 100 | bản gusset của crossbeam (tại x = 1332.4), từng cặp cách nhau 10 mm |
| `MRK1` | line dài 100 gần thẳng đứng | 4 vạch ở 4 góc tấm, vuông góc cạnh tấm |
| `BOLT` | circle | 8 cột × 2 hàng bu lông |

## Quy tắc đo
Ký hiệu: *dưới/trên* = polyline CHECK dưới/trên; *hàng h-stiff* = hàng line CHECK ngang nằm gần các tấm MRK1; *cột* = các bu lông cùng x.

**check out (2 dim, DIM50, aligned)**: cạnh trái (góc dưới trái → góc trên trái) và cạnh phải (góc dưới phải → góc trên phải), đường dim lệch ra ngoài.

**check v-stiff (5 dim)**
- Chuỗi 4 dim DIM50 xoay 0° dọc theo đáy: các điểm là đầu dưới của line CHECK dọc, x tăng dần, **bỏ nhóm ở cạnh phải của tấm (5404.6) nhưng giữ line ngoài tấm (5754.8)**. Đường dim cùng một độ cao, thấp hơn điểm thấp nhất của cạnh dưới.
- 1 dim DIM15 bề dày thanh đứng loại 10 mm (x = 4522.3): hai mặt ở đầu dưới thanh.

**check kote (25 dim)**
- Chuỗi ngang 9 dim DIM50 xoay 0°: góc dưới trái → bu lông **hàng trên** của từng cột (x tăng) → góc dưới phải của cạnh dưới. Đường dim cùng độ cao, thấp hơn hàng v-stiff.
- Mỗi cột 2 dim DIM15 xoay 90°: bu lông trên → bu lông dưới (600) → chân vuông góc lên cạnh dưới (nội suy polyline tại x của bu lông, ở sample đúng 300 dưới bu lông dưới). Đường dim lệch phải 86.3 so với cột.

**check h-stiff (21 dim)**
- 4 dim DIM50 xoay 90° ở hai cạnh: [góc trên → điểm hàng h-stiff chạm cạnh → góc dưới]; trái và phải.
- 4 dim DIM15 aligned bề dày tấm h-stiff: ở **một đầu** mỗi tấm (hai điểm = đầu mặt dưới và đầu mặt trên).
- 7 dim DIM15 xoay 0° khe hở giữa đầu tấm h-stiff và vật kế bên: cạnh tấm (10), đầu bản gusset crossbeam (30, hai phía), mặt thanh đứng (35).
- 2 dim DIM15 xoay 90° chia đoạn thanh đứng 9 mm không có gusset (x = 3289.8): đỉnh cạnh trên → điểm hàng h-stiff → đỉnh cạnh dưới.
- 4 dim DIM15 ở 4 góc: từ góc tấm đến đầu vạch MRK1 dài 100 (100 mm theo cạnh). Hai góc trên dùng aligned; hai góc dưới dùng dim xoay theo góc của đoạn cạnh dưới (1.3708° bên trái, 181.4524° bên phải).

**check crossbeam (9 dim DIM15)** — trên thanh có gusset ngang (x = 1332.4):
- 5 dim xoay 90° thành chuỗi dọc: đỉnh thanh → mặt trên gusset trên → mặt dưới gusset trên (10) → mặt trên gusset dưới → mặt dưới gusset dưới (10) → đáy thanh; đường dim lệch phải 80.1 so với đầu phải gusset.
- 2 dim xoay 0° bề rộng gusset (100) ở **mặt trong** (mặt dưới gusset trên, mặt trên gusset dưới), đặt vào khoảng giữa hai gusset.
- 2 dim xoay 0° bề dày thanh đứng 9 mm ở đáy (x = 1332.4 và x = 3289.8).

## Cách đặt
Layer: `check out`, `check v-stiff`, `check h-stiff`, `check kote`, `check crossbeam` (thiếu thì tạo). Dimstyle: `DIM50` cho chuỗi dài, `DIM15` cho chi tiết (thiếu thì dùng style hiện hành). Text để trống (AutoCAD tự đo). Khoảng cách đặt đường dim là hằng số trong `Dim01Config` (lấy theo sample).

## Biến thể
Chưa có (chỉ một sample).

## Điều kiện/giả định
WCS, Model space, mm. Tấm gần nằm ngang, cạnh dưới/trên là polyline gãy khúc đơn điệu theo x. Mỗi cột bu lông có đúng 2 bu lông. Có ít nhất một tấm h-stiff (MRK1) và một hàng CHECK ngang đi cùng nó. Thiếu dữ liệu: lệnh báo lỗi bằng thông điệp, không tạo dim nào.

## Chưa làm / bỏ qua
- Hàng h-stiff dưới (line CHECK ở y ≈ 1609–1723) không đo: sample không có dim nào ở hàng này.
- Không đo khe hở giữa đầu phải tấm h-stiff cuối và cạnh phải (sample không có; bật bằng `GapAtRightEdge`).
- Layer `OUT`, `MARK`, `BUZAI-INFO`: không dùng.
- Chưa có `DIMCFG` (đổi hằng số hiện phải sửa `Dim01Config`).
- Chưa hỗ trợ chọn nhiều tấm một lần.

## Điểm đang đoán
1. **Phân loại thanh đứng theo bề rộng**: 9 mm → layer `check crossbeam` (x = 1332.4 và 3289.8), 10 mm → `check v-stiff` (x = 4522.3). Chưa rõ quy tắc thật là bề rộng hay vai trò của thanh.
2. Chuỗi dim v-stiff bỏ cạnh phải (5404.6) nhưng lấy line ở 5754.8: quy tắc "bỏ cạnh phải, giữ line ngoài tấm" là suy từ một sample.
3. Chỉ hàng h-stiff **có tấm MRK1** được đo; hàng dưới bỏ qua vì sample không có.
4. **Đầu tấm đo bề dày**: ưu tiên đầu giáp thanh đứng không gusset (nếu không có thì đầu trái). Sample: tấm 1 đầu trái, tấm 2 đầu phải, tấm 3 và 4 đầu trái.
5. Hướng đặt dim bề dày tấm: tấm 1 ra ngoài (về phía cạnh), các tấm khác vào trong tấm.
6. Dim chia đoạn thanh đứng tại hàng h-stiff chỉ có ở x = 3289.8 (thanh 9 mm không gusset); thanh 10 mm ở 4522.3 không có. Suy ra từ bề rộng.
7. Dim xoay 0°/90°: chỉ tọa độ theo hướng đo được kiểm chứng (≤ 1 mm). Tọa độ còn lại của hai điểm gốc (ví dụ y của dim xoay 0°) trong sample lệch vài mm so với hình học (đặt tay bằng snap khác nhau); lệnh dùng trung điểm hoặc điểm nội suy.
8. Thứ tự P1/P2 trong sample không thống nhất (có dim ngược chiều); lệnh luôn đi từ trái sang phải / từ trên xuống dưới (chuỗi bu lông, v-stiff, h-stiff) và so khớp không xét thứ tự.
9. **15 dim đặt tay có khoảng cách đường dim không đều** nên lệnh dùng hằng số trung bình (xem bảng ở phần Kiểm chứng).

## Kiểm chứng
Sample: `origin.dxf` (gốc) và `dim.dxf` (đã đo), tấm G1-B7WR, 62 dim mới.

- **Khớp 62/62**: cùng layer, cùng kiểu (xoay/aligned), cùng dimstyle, cùng góc quay (≤ 0.1°), hai điểm gốc sai số ≤ 1 mm theo hướng đo.
- **Vị trí đường dim ≤ 1 mm: 47/62.** 15 dim còn lại do người dùng kéo tay nên không có quy tắc chung; độ lệch của hằng số mặc định (mm): `1D0` 3.0, `1DD` 1.4, `1EA` 1.1, `1F9` 3.8, `217` 22.6, `225` 2.8, `234` 15.5, `243` 21.0, `26C` 16.1, `279` 15.4, `2A0` 1.3, `2C9` 53.5, `2D6` 130.6, `2E3` 6.9, `2F0` 12.8.
- Chạy: nguyên mẫu Python (`prototypes/`) và bản C# của Core (biên dịch thử bằng Mono `mcs`, xem `tools/ConsoleCheck.cs`) cho cùng kết quả. Test xUnit (`tests/`) chưa được chạy trong môi trường của Claude.

## Lịch sử thay đổi
- 18/11/2026: tạo lệnh từ sample G1-B7WR (62 dim).
