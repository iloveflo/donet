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
    public partial class HoaDonNhapNV_UserControl : UserControl
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        private MySqlConnection connection;
        public HoaDonNhapNV_UserControl()
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
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                int soHoaDonNhap = Convert.ToInt32(textBoxSoHoaDonNhap.Text);
                int maQuanAo = Convert.ToInt32(textBoxMaQuanAo.Text);
                int maNhanVien = Convert.ToInt32(textBoxMaNhanVien.Text);
                int soLuongNhap = Convert.ToInt32(textBoxSoLuongNhap.Text);
                decimal donGiaNhap = Convert.ToDecimal(textBoxDonGiaNhap.Text);
                decimal giamGia = Convert.ToDecimal(textBoxGiamGia.Text);
                string nhaCungCap = comboBoxNhaCungCap.SelectedItem.ToString();

                // Kiểm tra nếu sản phẩm đã có trong bảng SanPham
                connection.Open();
                string checkSanPhamQuery = "SELECT COUNT(*) FROM SanPham WHERE MaQuanAo = @MaQuanAo";
                MySqlCommand cmdCheckSanPham = new MySqlCommand(checkSanPhamQuery, connection);
                cmdCheckSanPham.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                int productCount = Convert.ToInt32(cmdCheckSanPham.ExecuteScalar());

                if (productCount == 0)
                {
                    MessageBox.Show("Sản phẩm không tồn tại trong bảng SanPham!");
                    return;
                }

                // Lấy mã NCC từ tên NCC
                string getMaNCCQuery = "SELECT MaNCC FROM NhaCungCap WHERE TenNCC = @TenNCC";
                MySqlCommand cmdGetMaNCC = new MySqlCommand(getMaNCCQuery, connection);
                cmdGetMaNCC.Parameters.AddWithValue("@TenNCC", nhaCungCap);
                int maNCC = Convert.ToInt32(cmdGetMaNCC.ExecuteScalar());

                // Cập nhật số lượng và đơn giá nhập trong bảng SanPham
                string updateSanPhamQuery = "UPDATE SanPham SET SoLuong = SoLuong + @SoLuongNhap, DonGiaNhap = @DonGiaNhap WHERE MaQuanAo = @MaQuanAo";
                MySqlCommand cmdUpdateSanPham = new MySqlCommand(updateSanPhamQuery, connection);
                cmdUpdateSanPham.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);
                cmdUpdateSanPham.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                cmdUpdateSanPham.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                cmdUpdateSanPham.ExecuteNonQuery();

                // Cập nhật bảng HoaDonNhap
                string insertHoaDonNhapQuery = "INSERT INTO HoaDonNhap (MaNhanVien, NgayNhap, MaNCC, TongTien) " +
                                               "VALUES (@MaNhanVien, @NgayNhap, @MaNCC, @TongTien)";
                MySqlCommand cmdInsertHoaDonNhap = new MySqlCommand(insertHoaDonNhapQuery, connection);
                cmdInsertHoaDonNhap.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                cmdInsertHoaDonNhap.Parameters.AddWithValue("@NgayNhap", DateTime.Now);
                cmdInsertHoaDonNhap.Parameters.AddWithValue("@MaNCC", maNCC);
                cmdInsertHoaDonNhap.Parameters.AddWithValue("@TongTien", soLuongNhap * donGiaNhap);
                cmdInsertHoaDonNhap.ExecuteNonQuery();

                // Lấy số hóa đơn nhập vừa tạo
                int soHoaDonNhapMoi = (int)cmdInsertHoaDonNhap.LastInsertedId;

                // Cập nhật bảng ChiTietHoaDonNhap
                string insertChiTietHoaDonNhapQuery = "INSERT INTO ChiTietHoaDonNhap (SoHoaDonNhap, MaQuanAo, SoLuong, DonGia, GiamGia,ThanhTien) " +
                                                      "VALUES (@SoHoaDonNhap, @MaQuanAo, @SoLuong, @DonGia, @GiamGia,@ThanhTien)";
                MySqlCommand cmdInsertChiTietHoaDonNhap = new MySqlCommand(insertChiTietHoaDonNhapQuery, connection);
                cmdInsertChiTietHoaDonNhap.Parameters.AddWithValue("@SoHoaDonNhap", soHoaDonNhapMoi);
                cmdInsertChiTietHoaDonNhap.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                cmdInsertChiTietHoaDonNhap.Parameters.AddWithValue("@SoLuong", soLuongNhap);
                cmdInsertChiTietHoaDonNhap.Parameters.AddWithValue("@DonGia", donGiaNhap);
                cmdInsertChiTietHoaDonNhap.Parameters.AddWithValue("@GiamGia", giamGia);
                cmdInsertChiTietHoaDonNhap.Parameters.AddWithValue("@ThanhTien", soLuongNhap * donGiaNhap * (1 - giamGia / 100));

                cmdInsertChiTietHoaDonNhap.ExecuteNonQuery();

                MessageBox.Show("Thêm hóa đơn nhập thành công!");
                connection.Close();
                LoadHoaDonNhapData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        // Hàm làm sạch các textbox và combobox
        private void ClearInputFields()
        {
            textBoxSoHoaDonNhap.Clear();
            textBoxMaQuanAo.Clear();
            textBoxMaNhanVien.Clear();
            textBoxSoLuongNhap.Clear();
            textBoxDonGiaNhap.Clear();
            textBoxGiamGia.Clear();
            comboBoxNhaCungCap.SelectedIndex = -1;
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
