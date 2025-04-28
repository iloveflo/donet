using System;
using System.Windows.Forms;
using ClothingStore.Class;
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
            Khachhang_admin khachhangForm = new Khachhang_admin();
            khachhangForm.Show();
            this.Hide();
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            NhanVien_Admin nhanvienForm = new NhanVien_Admin();
            nhanvienForm.Show();
            this.Hide();
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            SanPham_Admin sanphamForm = new SanPham_Admin();
            sanphamForm.Show();
            this.Hide();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            ThongKe.ThongKe thongkeForm = new ThongKe.ThongKe();
            thongkeForm.Show();
            this.Hide();
        }
        private void btnThongKe1_Click(object sender, EventArgs e)
        {
            ThongKe.ThongKe1 thongkeForm = new ThongKe.ThongKe1();
            thongkeForm.Show();
            this.Hide();
        }
        private void btnDoanhThu_Click(object sender, EventArgs e)
        {
            ThongKe.DoanhThu doanhThu= new ThongKe.DoanhThu();
            doanhThu.Show();
            this.Hide();
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


        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            mainForm.Show();
            this.Close();
        }
    }
}
