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
using System.IO;
using MySql.Data.MySqlClient;

namespace ClothingStore.Admin
{
    public partial class ThongKe1_UserControl : UserControl
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        public ThongKe1_UserControl()
        {
            InitializeComponent();
            LoadTopSellingProducts();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textboxMaQuanAo.Text = row.Cells["MaQuanAo"].Value.ToString();
                textboxTenQuanAo.Text = row.Cells["TenQuanAo"].Value.ToString();
                textboxSLDaBan.Text = row.Cells["TongBan"].Value.ToString();
                textboxAnh.Text = row.Cells["Anh"].Value.ToString();

                string imagePath = row.Cells["Anh"].Value?.ToString();
                textboxAnh.Text = imagePath; // Hiển thị đường dẫn trong TextBox
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Tạo đường dẫn đầy đủ (nếu cần)
                    string fullPath = Path.Combine(Application.StartupPath, imagePath.Replace("/", "\\"));

                    // Kiểm tra file có tồn tại không
                    if (File.Exists(fullPath))
                    {
                        // Hiển thị ảnh trong PictureBox
                        picAnh.Image = Image.FromFile(fullPath);
                        picAnh.SizeMode = PictureBoxSizeMode.StretchImage; // Chỉnh kích thước ảnh cho phù hợp
                    }
                    else
                    {
                        MessageBox.Show("Ảnh không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        picAnh.Image = null; // Xóa ảnh nếu không tìm thấy
                    }
                }
                else
                {
                    picAnh.Image = null; // Xóa ảnh nếu không có đường dẫn
                }
            }
        }
        private void LoadTopSellingProducts()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    SELECT sp.MaQuanAo, sp.TenQuanAo, SUM(ct.SoLuong) AS TongBan, sp.Anh 
                    FROM SanPham sp
                    JOIN ChiTietHoaDonBan ct ON sp.MaQuanAo = ct.MaQuanAo
                    JOIN HoaDonBan hd ON ct.SoHoaDonBan = hd.SoHoaDonBan
                    GROUP BY sp.MaQuanAo, sp.TenQuanAo, sp.Anh
                    ORDER BY TongBan DESC
                    LIMIT 10;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                }
            }
        }
    }
}
