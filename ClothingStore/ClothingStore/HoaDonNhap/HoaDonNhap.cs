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

namespace ClothingStore.HoaDonNhap
{
    public partial class HoaDonNhap : Form
    {
        private string connectionString = "server=192.168.0.101;database=ClothingStore;user=root;password=binh11a10;";
        private MySqlConnection connection;

        public HoaDonNhap()
        {
            InitializeComponent();
            connection = new MySqlConnection(connectionString);
            LoadNhaCungCap();
            LoadHoaDonNhapData();
        }

        // Hàm load danh sách Nhà Cung Cấp vào ComboBox
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

        // Hàm thêm hóa đơn nhập và cập nhật dữ liệu vào các bảng
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

        // Hàm tìm kiếm hóa đơn nhập
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

        // Hàm quay lại giao diện ThongKe
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongKe.ThongKe thongKeForm = new ThongKe.ThongKe();
            thongKeForm.Show();
        }
        private void btnHienThi_Click(object sender, EventArgs e)
        {
            LoadHoaDonNhapData();
            // Làm sạch các TextBox và ComboBox
            textBoxSoHoaDonNhap.Clear();
            textBoxMaNhanVien.Clear();
            textBoxSoLuongNhap.Clear();
            textBoxDonGiaNhap.Clear();
            textBoxGiamGia.Clear();
            comboBoxNhaCungCap.SelectedIndex = -1; // Chọn lại nhà cung cấp đầu tiên
        }
    }
}
