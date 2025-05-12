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
using System.IO;

namespace ClothingStore.Admin
{
    public partial class ThongKe_UserControl : UserControl
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        public ThongKe_UserControl()
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
                    string query = "SELECT MaQuanAo, TenQuanAo, SoLuong, Anh FROM SanPham WHERE SoLuong <= 100";
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
    }
}
