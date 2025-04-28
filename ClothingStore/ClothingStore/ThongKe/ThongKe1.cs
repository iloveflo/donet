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
using ClothingStore.Class;
using MySql.Data.MySqlClient;

namespace ClothingStore.ThongKe
{
    public partial class ThongKe1 : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        public ThongKe1()
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

        // Khi click nút "Hóa đơn bán", mở form HoaDonBan.cs
        private void btnHoaDonBan_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDonBan.HoaDonBan form = new HoaDonBan.HoaDonBan();
            form.Show();
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
                    LIMIT 3;";

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

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE taikhoan SET DangNhap = 0 WHERE MaTaiKhoan = @MaTaiKhoan";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng xuất: " + ex.Message);
            }
            finally
            {
                SessionManager.ClearSession();
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin.Adimin adminForm = new Admin.Adimin();
            adminForm.Show();
        }
    }
}
