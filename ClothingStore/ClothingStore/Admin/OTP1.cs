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

namespace ClothingStore.Admin
{
    public partial class OTP1 : Form
    {
        public OTP1()
        {
            InitializeComponent();
        }
        string otpHeThong, tenDangNhap;

        public OTP1(string otp, string username)
        {
            InitializeComponent();
            otpHeThong = otp;
            tenDangNhap = username;
        }
        private void btnXacThuc_Click(object sender, EventArgs e)
        {
            string otpNhap = txtOTP.Text.Trim();
            if (otpNhap != otpHeThong)
            {
                MessageBox.Show("Mã OTP không đúng. Vui lòng kiểm tra lại.");
                return;
            }

            // Nếu đúng OTP → đặt lại mật khẩu
            string connectionString = "server=192.168.0.101;database=ClothingStore;user=root;password=binh11a10;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Đặt mật khẩu mới là "admin123"
                    string matKhauMoi = "admin123"; // có thể băm nếu muốn
                    string query = "UPDATE TaiKhoan SET MatKhau = @matkhau WHERE TenDangNhap = @username";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@matkhau", matKhauMoi);
                    cmd.Parameters.AddWithValue("@username", tenDangNhap);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xác thực thành công! Mật khẩu mới là: admin123");
                        this.Hide(); // hoặc quay về form đăng nhập
                        Main main = new Main();
                        main.Show();

                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để cập nhật.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

    }
}
