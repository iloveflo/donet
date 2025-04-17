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
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using ClothingStore.Class;

namespace ClothingStore.ThongKe
{
    public partial class ThongKe : Form
    {
        private string loaitaikhoan = SessionManager.LoaiTaiKhoanDangNhap;
        private string connectionString = "server=localhost;database=ClothingStore;user=root;password=binh11a10;";

        public ThongKe()
        {
            InitializeComponent();
            LoadData();
        }
        // Load dữ liệu từ bảng SanPham vào DataGridView
        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaQuanAo, TenQuanAo, SoLuong, Anh FROM SanPham WHERE SoLuong <= 30";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvDanhSachMatHangSapHet.DataSource = dt;
                    dgvDanhSachMatHangSapHet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvDanhSachMatHangSapHet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        // Khi người dùng chọn một dòng trong DataGridView
        private void dgvDanhSachMatHangSapHet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSachMatHangSapHet.Rows[e.RowIndex];
                txtMaQuanAo.Text = row.Cells["MaQuanAo"].Value.ToString();
                txtTenQuanAo.Text = row.Cells["TenQuanAo"].Value.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();

                string imagePath = row.Cells["Anh"].Value?.ToString();
                txtAnh.Text = imagePath; // Hiển thị đường dẫn trong TextBox
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Tạo đường dẫn đầy đủ (nếu cần)
                    string fullPath = Path.Combine(Application.StartupPath, imagePath.Replace("/", "\\"));

                    // Kiểm tra file có tồn tại không
                    if (File.Exists(fullPath))
                    {
                        // Hiển thị ảnh trong PictureBox
                        pictureBoxAnh.Image = Image.FromFile(fullPath);
                        pictureBoxAnh.SizeMode = PictureBoxSizeMode.StretchImage; // Chỉnh kích thước ảnh cho phù hợp
                    }
                    else
                    {
                        MessageBox.Show("Ảnh không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        pictureBoxAnh.Image = null; // Xóa ảnh nếu không tìm thấy
                    }
                }
                else
                {
                    pictureBoxAnh.Image = null; // Xóa ảnh nếu không có đường dẫn
                }
            }
        }

        // Sự kiện click nút "Quay lại"
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            if (loaitaikhoan == "Admin")
            {
                this.Hide();
                Admin.Adimin adminForm = new Admin.Adimin();
                adminForm.Show();
            }
            else
            {
                this.Hide();
                NhanVien.NhanVien nhanVien = new NhanVien.NhanVien();
                nhanVien.Show();
            }
        }
        // Sự kiện click nút "Hóa đơn nhập"
        private void btnHoaDonNhap_Click(object sender, EventArgs e)
        {
            HoaDonNhap.HoaDonNhap hoaDonNhapForm = new HoaDonNhap.HoaDonNhap();
            hoaDonNhapForm.Show();
            this.Hide(); // Ẩn form hiện tại (ThongKe)
        }
    }
}
