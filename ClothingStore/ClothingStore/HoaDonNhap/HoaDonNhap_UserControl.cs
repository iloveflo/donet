using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
