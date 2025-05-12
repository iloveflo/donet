using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ClothingStore.Class;
using MySql.Data.MySqlClient;

namespace ClothingStore
{
    public partial class Main : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;

        public Main()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
        }
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Kiểm tra nếu TextBox rỗng
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra tài khoản trong MySQL
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaTaiKhoan, LoaiTaiKhoan, DangNhap FROM TaiKhoan WHERE TenDangNhap = @username AND MatKhau = @password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string maTaiKhoan = reader["MaTaiKhoan"].ToString();  // ✅ Lấy đúng MaTaiKhoan
                                string loaiTaiKhoan = reader["LoaiTaiKhoan"].ToString();
                                bool dangNhap = Convert.ToBoolean(reader["DangNhap"]);

                                if (dangNhap)
                                {
                                    // Tài khoản đang đăng nhập ở nơi khác
                                    MessageBox.Show("Tài khoản đang đăng nhập ở thiết bị khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                // Lưu vào SessionManager
                                SessionManager.SetSession(maTaiKhoan, loaiTaiKhoan);

                                // Kiểm tra giá trị trước khi mở form
                                MessageBox.Show("MaTaiKhoan: " + maTaiKhoan + "\nLoaiTaiKhoan: " + loaiTaiKhoan);

                                // Mở form tương ứng với loại tài khoản
                                switch (loaiTaiKhoan)
                                {
                                    case "Admin":
                                        reader.Close();
                                        string updateQuery = "UPDATE TaiKhoan SET DangNhap = 1 WHERE MaTaiKhoan = @MaTaiKhoan";
                                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                                        {
                                            updateCmd.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                        Admin.Adimin adminForm = new Admin.Adimin();
                                        adminForm.Show();
                                        this.Hide();
                                        break;

                                    case "KhachHang":
                                        reader.Close();
                                        string updateQuery1 = "UPDATE TaiKhoan SET DangNhap = 1 WHERE MaTaiKhoan = @MaTaiKhoan";
                                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery1, conn))
                                        {
                                            updateCmd.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                        KhachHang.KhachHang khachForm = new KhachHang.KhachHang();
                                        khachForm.Show();
                                        this.Hide();
                                        break;
                                    case "NhanVien":
                                        reader.Close();
                                        string updateQuery2 = "UPDATE TaiKhoan SET DangNhap = 1 WHERE MaTaiKhoan = @MaTaiKhoan";
                                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery2, conn))
                                        {
                                            updateCmd.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                        NhanVien.Nhan_Vien nhanVien= new NhanVien.Nhan_Vien();
                                        nhanVien.Show();
                                        this.Hide();
                                        break;

                                    default:
                                        MessageBox.Show("Loại tài khoản không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Thông tin tài khoản hoặc mật khẩu không chính xác", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.Show();
            this.Hide();
        }
        private void btnQuenMatKhau_Click(object sender, EventArgs e)
        {
            Admin.QuenMatKhau quenMatKhau= new Admin.QuenMatKhau();
            quenMatKhau.Show();
            this.Hide();
        }
    }
}
