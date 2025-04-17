using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClothingStore.Admin
{
    public partial class DoiMatKhau : Form
    {
        // Chuỗi kết nối tới MySQL
        private string connectionString = "server=localhost;database=ClothingStore;user=root;password=binh11a10;";

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

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @UserName AND MatKhau = @Password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count == 0)
                    {
                        MessageBox.Show("Tên đăng nhập và mật khẩu cũ không trùng khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Xác nhận đổi mật khẩu
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đổi mật khẩu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Cập nhật mật khẩu mới
                        string updateQuery = "UPDATE TaiKhoan SET MatKhau = @NewPassword WHERE TenDangNhap = @UserName";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@NewPassword", txtNewPassword.Text);
                        updateCmd.Parameters.AddWithValue("@UserName", txtUserName.Text);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("Đã đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
