using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClothingStore.Class;
using ClosedXML.Excel;
using System.IO;

namespace ClothingStore.HoaDonBan
{
    public partial class HoaDonBan : Form
    {
        private string connectionString = "server=192.168.0.101;database=ClothingStore;user=root;password=binh11a10;";
        public HoaDonBan()
        {
            InitializeComponent();
            LoadHoaDonBan();
        }
        private void LoadHoaDonBan()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        hdb.SoHoaDonBan, 
                        sp.MaQuanAo, 
                        sp.TenQuanAo, 
                        hdb.MaNhanVien, 
                        hdb.MaKhachHang, 
                        cthdb.SoLuong 
                    FROM HoaDonBan hdb
                    JOIN ChiTietHoaDonBan cthdb ON hdb.SoHoaDonBan = cthdb.SoHoaDonBan
                    JOIN SanPham sp ON cthdb.MaQuanAo = sp.MaQuanAo;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        // Khi click vào một dòng trong DataGridView, hiển thị dữ liệu lên TextBox
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtSoHoaDonBan.Text = row.Cells["SoHoaDonBan"].Value.ToString();
                txtMaQuanAo.Text = row.Cells["MaQuanAo"].Value.ToString();
                txtTenQuanAo.Text = row.Cells["TenQuanAo"].Value.ToString();
                txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value.ToString();
                txtMaKhachHang.Text = row.Cells["MaKhachHang"].Value.ToString();
                txtSoLuongBan.Text = row.Cells["SoLuong"].Value.ToString();
            }
        }
        private void btnQuaylai_click(object sender, EventArgs e)
        {
            if (SessionManager.LoaiTaiKhoanDangNhap == "Admin")
            {
                this.Hide();
                ThongKe.ThongKe1 thongKeForm1 = new ThongKe.ThongKe1();
                thongKeForm1.Show();
                this.Hide();
            }
            if (SessionManager.LoaiTaiKhoanDangNhap == "NhanVien")
            {
                this.Hide();
                NhanVien.NhanVien nhanVien=new NhanVien.NhanVien();
                nhanVien.Show();
            }
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            string soHoaDon = txtSoHoaDonBan.Text;

            if (string.IsNullOrEmpty(soHoaDon))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in.");
                return;
            }

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        hdb.SoHoaDonBan, hdb.NgayBan, hdb.TongTien,
                        kh.TenKhach, kh.DiaChi AS DiaChiKhach, kh.SoDienThoai AS SDTKhach, kh.Email AS EmailKhach,
                        nv.TenNhanVien, nv.SoDienThoai AS SDTNhanVien,
                        sp.TenQuanAo, cthd.SoLuong, cthd.GiamGia, cthd.ThanhTien
                    FROM hoadonban hdb
                    JOIN khachhang kh ON hdb.MaKhachHang = kh.MaKhachHang
                    JOIN nhanvien nv ON hdb.MaNhanVien = nv.MaNhanVien
                    JOIN chitiethoadonban cthd ON hdb.SoHoaDonBan = cthd.SoHoaDonBan
                    JOIN sanpham sp ON cthd.MaQuanAo = sp.MaQuanAo
                    WHERE hdb.SoHoaDonBan = @soHoaDon";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@soHoaDon", soHoaDon);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add("HoaDon");

                        int row = 1;

                        // Tiêu đề hóa đơn
                        ws.Cell(row, 1).Value = "HÓA ĐƠN BÁN HÀNG - CLOTHINGSTORE";

                        // Merge các ô trong tiêu đề
                        ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetBold().Font.SetFontSize(18)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                            .Fill.SetBackgroundColor(XLColor.LightBlue);

                        // Tự động điều chỉnh độ rộng các cột
                        ws.Columns().AdjustToContents();

                        // Điều chỉnh chiều cao hàng để phù hợp với tiêu đề dài
                        ws.Row(row).Height = 30;  // Điều chỉnh chiều cao hàng, bạn có thể thử giá trị khác nếu cần
                        row++;

                        // Cập nhật chiều cao dòng cho tiêu đề
                        ws.Row(row).Height = 30; // Điều chỉnh chiều cao cho dòng này
                        ws.Cell(row, 1).Value = $"Số hóa đơn: {soHoaDon}";
                        ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16)
                            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        row++;

                        while (reader.Read())
                        {
                            ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16);
                            ws.Cell(row++, 1).Value = $"Ngày bán: {Convert.ToDateTime(reader["NgayBan"]).ToString("dd/MM/yyyy")}";
                            ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16);
                            ws.Cell(row++, 1).Value = $"Khách hàng: {reader["TenKhach"]}";
                            ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16);
                            ws.Cell(row++, 1).Value = $"Số Điện Thoại: {reader["SDTKhach"]}";
                            ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16);
                            ws.Cell(row++, 1).Value= $"Email: {reader["EmailKhach"]}";
                            ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16);
                            ws.Cell(row++, 1).Value = $"Nhân viên bán: {reader["TenNhanVien"]}";
                            ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16);
                            ws.Cell(row++, 1).Value= $"Liên hệ: {reader["SDTNhanVien"]}";
                            ws.Range(row, 1, row, 4).Merge().Style
                            .Font.SetFontSize(16);
                            row++;

                            // Tiêu đề bảng chi tiết sản phẩm
                            ws.Cell(row, 1).Value = "Tên sản phẩm";
                            ws.Cell(row, 2).Value = "Số lượng";
                            ws.Cell(row, 3).Value = "Giảm giá (%)";
                            ws.Cell(row, 4).Value = "Thành tiền";

                            ws.Range(row, 1, row, 4).Style
                                .Font.SetBold()
                                .Fill.SetBackgroundColor(XLColor.LightGray)
                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                .Font.SetFontSize(15);
                            row++;

                            // Cập nhật chiều cao dòng cho tiêu đề bảng
                            ws.Row(row).Height = 25; // Điều chỉnh chiều cao cho dòng này
                            break; // để không lặp lại
                        }

                        reader.Close();

                        // Ghi chi tiết sản phẩm
                        using (var cmd2 = new MySqlCommand(query, conn))
                        {
                            cmd2.Parameters.AddWithValue("@soHoaDon", soHoaDon);
                            using (var reader2 = cmd2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    ws.Cell(row, 1).Value = reader2["TenQuanAo"];
                                    ws.Cell(row, 2).Value = reader2["SoLuong"];
                                    ws.Cell(row, 3).Value = reader2["GiamGia"];
                                    ws.Cell(row, 4).Value = reader2["ThanhTien"];

                                    ws.Range(row, 1, row, 4).Style
                                        .Fill.SetBackgroundColor(XLColor.WhiteSmoke)
                                        .Font.SetFontSize(12)
                                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                    row++;

                                    // Cập nhật chiều cao dòng cho các dòng chi tiết sản phẩm
                                    ws.Row(row).Height = 25; // Điều chỉnh chiều cao cho dòng này
                                }
                            }
                        }

                        // Tổng tiền
                        ws.Cell(row, 3).Value = "Tổng tiền:";
                        ws.Cell(row, 4).FormulaA1 = $"=SUM(D6:D{row - 1})";
                        ws.Cell(row, 4).Style.NumberFormat.Format = "#,##0.00";
                        ws.Range(row, 3, row, 4).Style
                            .Font.SetBold()
                            .Fill.SetBackgroundColor(XLColor.LightGreen);
                        row++;

                        // Cập nhật chiều cao dòng cho tổng tiền
                        ws.Row(row).Height = 30; // Điều chỉnh chiều cao cho dòng này

                        // Căn giữa các cột phù hợp
                        ws.Range(6, 2, row - 1, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        ws.Range(6, 4, row - 1, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        ws.Range(6, 4, row, 4).Style.NumberFormat.Format = "#,##0.00";

                        // Tự động căn độ rộng
                        ws.Columns().AdjustToContents();

                        // Ghi file
                        string safeSoHoaDon = soHoaDon.Replace("/", "_").Replace("\\", "_").Replace(":", "_");
                        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"HoaDon_{safeSoHoaDon}.xlsx");

                        wb.SaveAs(filePath);
                        MessageBox.Show($"✅ Đã xuất hóa đơn ra Excel thành công!\nFile: {filePath}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start("explorer.exe", filePath);
                    }
                }
            }
        }
    }
}


