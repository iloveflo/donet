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
        private string connectionString = "server=192.168.0.101;database=ClothingStore;user=root;password=binh11a10;";

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
                    string query = "SELECT MaTaiKhoan, LoaiTaiKhoan FROM TaiKhoan WHERE TenDangNhap = @username AND MatKhau = @password";
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

                                // Lưu vào SessionManager
                                SessionManager.SetSession(maTaiKhoan, loaiTaiKhoan);

                                // Kiểm tra giá trị trước khi mở form
                                MessageBox.Show("MaTaiKhoan: " + maTaiKhoan + "\nLoaiTaiKhoan: " + loaiTaiKhoan);

                                // Mở form tương ứng với loại tài khoản
                                switch (loaiTaiKhoan)
                                {
                                    case "Admin":
                                        Admin.Adimin adminForm = new Admin.Adimin();
                                        adminForm.Show();
                                        this.Hide();
                                        break;

                                    case "KhachHang":
                                        KhachHang.KhachHang khachForm = new KhachHang.KhachHang();
                                        khachForm.Show();
                                        this.Hide();
                                        break;
                                    case "NhanVien":
                                        NhanVien.NhanVien nhanVien= new NhanVien.NhanVien();
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
