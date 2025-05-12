using System;
using System.Windows.Forms;
using ClothingStore.Class;
using ClothingStore.KhachHang;
using MySql.Data.MySqlClient;

namespace ClothingStore.Admin
{
    public partial class Adimin : Form
    {
        private string connectionString= DatabaseHelper.ConnectionString;
        public Adimin()
        {
            InitializeComponent();
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            LoadControl(new Khachhang_UserControl());
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            LoadControl(new NhanVien_UserControl());
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            LoadControl(new SanPham_UserControl());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadControl(new ThongKe_UserControl());
        }
        private void btnThongKe1_Click(object sender, EventArgs e)
        {
            LoadControl(new ThongKe1_UserControl());
        }
        private void btnDoanhThu_Click(object sender, EventArgs e)
        {
            LoadControl(new DoanhThu_UserControl());
        }
        private void btnHoaDonBan_Click(object sender, EventArgs e)
        {
            LoadControl(new HoaDonBan.HoaDonBan_UserControl());
        }

        private void btnHoaDonNhap_Click(object sender, EventArgs e)
        {
            LoadControl(new HoaDonNhap.HoaDonNhap_UserControl());
        }
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE taikhoan SET DangNhap = 0 WHERE MaTaiKhoan = @MaTaiKhoan";
                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                cmd.ExecuteNonQuery();
            }

            // Sau khi update thành công, clear session
            SessionManager.ClearSession();

            DoiMatKhau doimatkhauForm = new DoiMatKhau();
            doimatkhauForm.Show();
            this.Hide();
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
        private void LoadControl(UserControl uc)
        {
            panelMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Add(uc);
        }
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            mainForm.Show();
            this.Close();
        }
    }
}
