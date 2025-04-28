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

namespace ClothingStore.Admin
{
    public partial class OTP : Form
    {
        private string generatedOtp;
        private string userName;
        private string newPassword;
        private string connectionString = DatabaseHelper.ConnectionString;

        public OTP(string otp, string user, string newPass)
        {
            InitializeComponent();
            generatedOtp = otp;
            userName = user;
            newPassword = newPass;
        }

        private void btnXacThuc_Click(object sender, EventArgs e)
        {
            if (txtOTP.Text.Trim() == generatedOtp)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE TaiKhoan SET MatKhau = @NewPassword WHERE TenDangNhap = @UserName";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@UserName", userName);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Xác thực thành công. Mật khẩu đã được đổi!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Main main = new Main();
                    main.Show();
                }
            }
            else
            {
                MessageBox.Show("Sai mã OTP. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
