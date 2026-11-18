# AutoDimPanel – cách build và dùng

1. Copy `Directory.Build.props` thành `Directory.Build.local.props`, sửa `AcadDir` trỏ tới thư mục cài AutoCAD (chứa `accoremgd.dll`).
2. Mở `AutoDimPanel.sln` bằng Visual Studio, build `AutoDimPanel.Plugin` (x64). Ra `AutoDimPanel.dll` và `AutoDimPanel.Core.dll` cùng thư mục.
3. Trong AutoCAD: `NETLOAD` chọn `AutoDimPanel.dll`.
4. `DIMPAL` mở palette, `DIMLIST` liệt kê lệnh, `DIM01` đo tấm panel (chọn polyline BORDER).
5. Test: `dotnet test tests/AutoDimPanel.Tests` (hoặc Test Explorer).
6. Sinh dữ liệu mẫu mới: `python tools/gen_sample.py origin.dxf dim.dxf Sample02 > tests/AutoDimPanel.Tests/Sample02.cs` (cần `pip install ezdxf`).
7. Gom code gửi kèm chat mới: `python tools/make_bundle.py > bundle.md`.
