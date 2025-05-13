using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using ClothingStore.Class;
using MySql.Data.MySqlClient;

namespace ClothingStore.HoaDonNhap
{
    public partial class HoaDonNhap_UserControl : UserControl
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        private MySqlConnection connection;
        public HoaDonNhap_UserControl()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
            LoadNhaCungCap();
            LoadHoaDonNhapData();
        }
        private void LoadNhaCungCap()
        {
            try
            {
                connection.Open();
                string query = "SELECT MaNCC, TenNCC FROM NhaCungCap";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxNhaCungCap.Items.Add(reader["TenNCC"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void LoadHoaDonNhapData()
        {
            try
            {
                connection.Open();
                string query = "SELECT hd.SoHoaDonNhap, hd.MaNhanVien, ncc.TenNCC, ct.MaQuanAo, ct.SoLuong, ct.DonGia, ct.GiamGia, ct.SoLuong * ct.DonGia * (1 - ct.GiamGia / 100) AS ThanhTien " +
                               "FROM HoaDonNhap hd " +
                               "JOIN NhaCungCap ncc ON hd.MaNCC = ncc.MaNCC " +
                               "JOIN ChiTietHoaDonNhap ct ON hd.SoHoaDonNhap = ct.SoHoaDonNhap";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewHoaDonNhap.DataSource = dt;
                dataGridViewHoaDonNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewHoaDonNhap.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void dataGridViewHoaDonNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewHoaDonNhap.Rows[e.RowIndex];

                // Hiển thị giá trị từ các cột trong DataGridView vào các TextBox tương ứng
                textBoxSoHoaDonNhap.Text = row.Cells["SoHoaDonNhap"].Value?.ToString();
                textBoxMaQuanAo.Text = row.Cells["MaQuanAo"].Value?.ToString();
                textBoxSoLuongNhap.Text = row.Cells["SoLuong"].Value?.ToString();
                textBoxDonGiaNhap.Text = row.Cells["DonGia"].Value?.ToString();
                textBoxGiamGia.Text = row.Cells["GiamGia"].Value?.ToString();

                // Nếu bạn đã JOIN được mã nhân viên từ truy vấn, ví dụ là MaNhanVien
                textBoxMaNhanVien.Text = row.Cells["MaNhanVien"].Value?.ToString();

                // Lấy tên nhà cung cấp từ cột TenNCC và chọn trong ComboBox
                string tenNCC = row.Cells["TenNCC"].Value?.ToString();
                if (!string.IsNullOrEmpty(tenNCC) && comboBoxNhaCungCap.Items.Contains(tenNCC))
                {
                    comboBoxNhaCungCap.SelectedItem = tenNCC;
                }
                else
                {
                    comboBoxNhaCungCap.SelectedIndex = -1; // Clear selection nếu không khớp
                }
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "SELECT hd.SoHoaDonNhap, ncc.TenNCC, ct.MaQuanAo, ct.SoLuong, ct.DonGia, ct.GiamGia, ct.SoLuong * ct.DonGia * (1 - ct.GiamGia / 100) AS ThanhTien " +
                               "FROM HoaDonNhap hd " +
                               "JOIN NhaCungCap ncc ON hd.MaNCC = ncc.MaNCC " +
                               "JOIN ChiTietHoaDonNhap ct ON hd.SoHoaDonNhap = ct.SoHoaDonNhap " +
                               "WHERE hd.SoHoaDonNhap LIKE @SoHoaDonNhap OR ncc.TenNCC LIKE @TenNCC";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@SoHoaDonNhap", "%" + textBoxSoHoaDonNhap.Text + "%");
                cmd.Parameters.AddWithValue("@TenNCC", "%" + comboBoxNhaCungCap.SelectedItem.ToString() + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewHoaDonNhap.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        private void btnXuatHoaDonNhap_Click(object sender, EventArgs e)
        {
            if (dataGridViewHoaDonNhap.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soHoaDonNhap = Convert.ToInt32(dataGridViewHoaDonNhap.CurrentRow.Cells["SoHoaDonNhap"].Value);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Truy vấn thông tin chung của hóa đơn
                    string queryHeader = @"
                SELECT hd.SoHoaDonNhap, hd.NgayNhap, hd.TongTien, nv.TenNhanVien
                FROM hoadonnhap hd
                JOIN nhanvien nv ON hd.MaNhanVien = nv.MaNhanVien
                WHERE hd.SoHoaDonNhap = @SoHoaDonNhap";

                    MySqlCommand cmdHeader = new MySqlCommand(queryHeader, conn);
                    cmdHeader.Parameters.AddWithValue("@SoHoaDonNhap", soHoaDonNhap);
                    MySqlDataReader reader = cmdHeader.ExecuteReader();

                    if (!reader.Read())
                    {
                        MessageBox.Show("Không tìm thấy thông tin hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string tenNhanVien = reader.GetString("TenNhanVien");
                    DateTime ngayNhap = reader.GetDateTime("NgayNhap");
                    decimal tongTien = reader.GetDecimal("TongTien");

                    reader.Close();

                    // 2. Truy vấn chi tiết sản phẩm
                    string queryDetails = @"
                SELECT 
                    sp.TenQuanAo AS 'Tên quần áo',
                    cthdn.SoLuong AS 'Số lượng',
                    cthdn.DonGia AS 'Đơn giá',
                    cthdn.GiamGia AS 'Giảm giá (%)',
                    cthdn.ThanhTien AS 'Thành tiền'
                FROM chitiethoadonnhap cthdn
                JOIN sanpham sp ON cthdn.MaQuanAo = sp.MaQuanAo
                WHERE cthdn.SoHoaDonNhap = @SoHoaDonNhap";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(queryDetails, conn);
                    DataTable dtDetails = new DataTable();
                    adapter.SelectCommand.Parameters.AddWithValue("@SoHoaDonNhap", soHoaDonNhap);
                    adapter.Fill(dtDetails);

                    // 3. Ghi ra file Excel theo chiều dọc
                    string folderPath = @"D:\ProjectC#";
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                    string filePath = Path.Combine(folderPath, $"HoaDonNhap_{soHoaDonNhap}.xlsx");

                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("HoaDonNhap");

                        // Tiêu đề hóa đơn
                        ws.Cell("A1").Value = "HÓA ĐƠN NHẬP";
                        ws.Cell("A1").Style.Font.Bold = true;
                        ws.Cell("A1").Style.Font.FontSize = 16;
                        ws.Range("A1:E1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        // Thông tin chung
                        ws.Cell("A3").Value = "Số HĐ:";
                        ws.Cell("B3").Value = soHoaDonNhap;

                        ws.Cell("A4").Value = "Ngày nhập:";
                        ws.Cell("B4").Value = ngayNhap.ToString("dd/MM/yyyy");

                        ws.Cell("A5").Value = "Nhân viên:";
                        ws.Cell("B5").Value = tenNhanVien;

                        ws.Cell("A6").Value = "Tổng tiền:";
                        ws.Cell("B6").Value = tongTien.ToString("N0") + " đ";

                        // Danh sách sản phẩm
                        ws.Cell("A8").Value = "Sản Phẩm:";
                        ws.Cell("A8").Style.Font.Bold = true;

                        ws.Cell("A10").InsertTable(dtDetails);
                        ws.Columns().AdjustToContents();

                        workbook.SaveAs(filePath);
                    }

                    // Mở file
                    System.Diagnostics.Process.Start("explorer.exe", filePath);
                    MessageBox.Show("Xuất hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
