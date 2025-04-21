using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClothingStore.Admin
{
    public partial class QuenMatKhau : Form
    {
        public QuenMatKhau()
        {
            InitializeComponent();
        }
        private void btnLayLaiMatKhau_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                return;
            }

            string connectionString = "server=localhost;database=ClothingStore;user=root;password=binh11a10;";
            string email = "";
            string otp = GenerateOTP(); // Tạo mã OTP ngẫu nhiên

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Truy vấn MaTaiKhoan
                    string query = "SELECT MaTaiKhoan FROM taikhoan WHERE TenDangNhap = @username";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    object maTaiKhoanObj = cmd.ExecuteScalar();
                    if (maTaiKhoanObj == null)
                    {
                        MessageBox.Show("Không tìm thấy tài khoản!");
                        return;
                    }

                    string maTaiKhoan = maTaiKhoanObj.ToString();

                    // Tìm email trong bảng nhân viên
                    query = "SELECT Email FROM nhanvien WHERE MaTaiKhoan = @maTK " +
                            "UNION " +
                            "SELECT Email FROM khachhang WHERE MaTaiKhoan = @maTK";

                    cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maTK", maTaiKhoan);

                    object emailObj = cmd.ExecuteScalar();
                    if (emailObj == null)
                    {
                        MessageBox.Show("Không tìm thấy email để gửi OTP.");
                        return;
                    }

                    email = emailObj.ToString();

                    // Gửi OTP (bạn cần cài thư viện System.Net.Mail)
                    SendOtpToEmail(email, otp);
                    MessageBox.Show("Mã xác thực OTP đã gửi về Email của tài khoản này");

                    // Chuyển sang form OTP
                    OTP1 otpForm = new OTP1(otp, username);
                    this.Hide();
                    otpForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private string GenerateOTP()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString(); // 6 chữ số
        }
        private void SendOtpToEmail(string toEmail, string otpCode)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("binha10k56@gmail.com"); // Email bạn gửi đi
                mail.To.Add(toEmail);
                mail.Subject = "Mã OTP đổi mật khẩu";
                mail.Body = $"Mã OTP của bạn là: {otpCode}";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("binha10k56@gmail.com", "Liên hệ Bình để lấy (cấm push mã này lên github)");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể gửi Email: " + ex.Message);
            }
        }
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            // Quay lại form Main.cs
            this.Hide();
            Main mainForm = new Main();
            mainForm.Show();
        }

    }
}
