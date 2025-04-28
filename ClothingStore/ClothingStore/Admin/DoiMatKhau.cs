using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using ClothingStore.Class;

namespace ClothingStore.Admin
{
    public partial class DoiMatKhau : Form
    {
        // Chuỗi kết nối tới MySQL
        private string connectionString = DatabaseHelper.ConnectionString;

        public DoiMatKhau()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            txtNewPassword.PasswordChar = '*';
            txtReEnterNewPassword.PasswordChar = '*';
        }
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
            txtNewPassword.PasswordChar= chkShowPassword.Checked ? '\0' : '*';
            txtReEnterNewPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu bất kỳ ô nào bị bỏ trống
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text) ||
                string.IsNullOrEmpty(txtNewPassword.Text) || string.IsNullOrEmpty(txtReEnterNewPassword.Text))
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mật khẩu mới có khớp không
            if (txtNewPassword.Text != txtReEnterNewPassword.Text)
            {
                MessageBox.Show("Vui lòng nhập đúng mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string emailQuery = @"
                    SELECT Email 
                    FROM (
                        SELECT Email FROM KhachHang WHERE MaTaiKhoan = (SELECT MaTaiKhoan FROM TaiKhoan WHERE TenDangNhap = @UserName)
                        UNION
                        SELECT Email FROM NhanVien WHERE MaTaiKhoan = (SELECT MaTaiKhoan FROM TaiKhoan WHERE TenDangNhap = @UserName)
                    ) AS CombinedEmails
                    LIMIT 1;";

                    MySqlCommand emailCmd = new MySqlCommand(emailQuery, conn);
                    emailCmd.Parameters.AddWithValue("@UserName", txtUserName.Text);

                    object result = emailCmd.ExecuteScalar();
                    string email = result != null ? result.ToString() : null;

                    if (string.IsNullOrEmpty(email))
                    {
                        MessageBox.Show("Không tìm thấy email của người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Tạo mã OTP
                    Random rnd = new Random();
                    string otpCode = rnd.Next(100000, 999999).ToString();

                    // Gửi OTP qua Email
                    SendOtpToEmail(email, otpCode);
                    MessageBox.Show($"Mã xác thực OTP đã gửi về Email: {email}");

                    // Mở form OTP và truyền dữ liệu
                    OTP otpForm = new OTP(otpCode, txtUserName.Text, txtNewPassword.Text);
                    otpForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                smtp.Credentials = new NetworkCredential("binha10k56@gmail.com", "");
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
