using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClothingStore
{
    public partial class Register : Form
    {
        private string connectionString = "server=192.168.0.101;database=ClothingStore;user=root;password=binh11a10;";
        private string currentCaptchaCode = "";
        private int currentCaptchaId = -1;
        private Random random = new Random();

        public Register()
        {
            InitializeComponent();
            LoadRandomCaptcha();
            txtPassword.PasswordChar = '*';
            txtRePassword.PasswordChar = '*';
        }
        private void LoadRandomCaptcha()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Đếm tổng số CAPTCHA có trong bảng
                string countQuery = "SELECT COUNT(*) FROM CapCha";
                int count = 0;
                using (MySqlCommand cmdCount = new MySqlCommand(countQuery, conn))
                {
                    count = Convert.ToInt32(cmdCount.ExecuteScalar());
                }
                if (count == 0)
                {
                    MessageBox.Show("Không có CAPTCHA nào trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Chọn ID ngẫu nhiên
                int randomIndex = random.Next(0, count);

                // Lấy CAPTCHA theo chỉ mục ngẫu nhiên
                string query = $"SELECT MaAnh, LinkAnh, KetQua FROM CapCha LIMIT {randomIndex}, 1";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentCaptchaId = reader.GetInt32("MaAnh");
                        currentCaptchaCode = reader.GetString("KetQua");
                        pictureBoxCaptcha.ImageLocation = reader.GetString("LinkAnh");
                    }
                }
            }
        }
        private void btnChangeCaptcha_Click(object sender, EventArgs e)
        {
            int previousId = currentCaptchaId;
            do
            {
                LoadRandomCaptcha();
            } while (currentCaptchaId == previousId);
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string rePassword = txtRePassword.Text.Trim();
            string maTaiKhoan = txtMaTaiKhoan.Text.Trim();
            string email = txtEmail.Text.Trim();

            // 🔹 Kiểm tra rỗng
            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(soDienThoai) || string.IsNullOrEmpty(diaChi) ||
                string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(rePassword) ||
                string.IsNullOrEmpty(maTaiKhoan)|| string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Hãy nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Kiểm tra Password và Re-enter Password
            if (password != rePassword)
            {
                MessageBox.Show("Password và Re-enter Password chưa giống nhau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra độ dài mật khẩu
            if (password.Length > 16||password.Length<8)
            {
                MessageBox.Show("Mật khẩu không được ít hơn 8 hoặc vượt quá 16 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtCaptcha.Text.Trim() != currentCaptchaCode)
            {
                MessageBox.Show("CAPTCHA không đúng! Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //kiểm tra xem mã tài khoản, sdt, email có tồn tại hay chưa
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string queryCheck = @"
                   
                    SELECT COUNT(*) 
                    FROM TaiKhoan 
                    WHERE TenDangNhap = @userName OR MaTaiKhoan = @maTaiKhoan;

                    
                    SELECT COUNT(*) 
                    FROM (
                        SELECT SoDienThoai FROM KhachHang WHERE SoDienThoai = @soDienThoai
                        UNION ALL
                        SELECT SoDienThoai FROM NhanVien WHERE SoDienThoai = @soDienThoai
                    ) AS TempSDT;

                    
                    SELECT COUNT(*) 
                    FROM (
                        SELECT Email FROM KhachHang WHERE Email = @email
                        UNION ALL
                        SELECT Email FROM NhanVien WHERE Email = @email
                    ) AS TempEmail;";

                    using (MySqlCommand cmd = new MySqlCommand(queryCheck, conn))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@maTaiKhoan", maTaiKhoan);
                        cmd.Parameters.AddWithValue("@soDienThoai", soDienThoai);
                        cmd.Parameters.AddWithValue("@email", email);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            int countTaiKhoan = Convert.ToInt32(reader[0]);

                            reader.NextResult();
                            reader.Read();
                            int countSDT = Convert.ToInt32(reader[0]);

                            reader.NextResult();
                            reader.Read();
                            int countEmail = Convert.ToInt32(reader[0]);

                            if (countTaiKhoan > 0)
                            {
                                MessageBox.Show("Tên tài khoản hoặc mã tài khoản đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (countSDT > 0)
                            {
                                MessageBox.Show("Số điện thoại đã tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (countEmail > 0)
                            {
                                MessageBox.Show("Email đã tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        // 🔹 Nếu tất cả đều hợp lệ, tiến hành thêm dữ liệu vào CSDL
                        string queryInsertTaiKhoan = "INSERT INTO TaiKhoan (MaTaiKhoan, TenDangNhap, MatKhau, LoaiTaiKhoan) VALUES (@maTaiKhoan, @userName, @password, 'KhachHang')";
                        string queryInsertKhachHang = "INSERT INTO KhachHang (MaKhachHang, TenKhach, SoDienThoai, DiaChi,MaTaiKhoan,Email) VALUES (@maKhachHang, @hoTen, @soDienThoai, @diaChi,@maTaiKhoan,@email)";

                        using (MySqlCommand cmdTaiKhoan = new MySqlCommand(queryInsertTaiKhoan, conn))
                        {
                            cmdTaiKhoan.Parameters.AddWithValue("@maTaiKhoan", maTaiKhoan);
                            cmdTaiKhoan.Parameters.AddWithValue("@userName", userName);
                            cmdTaiKhoan.Parameters.AddWithValue("@password", password);
                            cmdTaiKhoan.ExecuteNonQuery();
                        }

                        using (MySqlCommand cmdKhachHang = new MySqlCommand(queryInsertKhachHang, conn))
                        {
                            cmdKhachHang.Parameters.AddWithValue("@maKhachHang", maTaiKhoan);
                            cmdKhachHang.Parameters.AddWithValue("@maTaiKhoan", maTaiKhoan);
                            cmdKhachHang.Parameters.AddWithValue("@hoTen", hoTen);
                            cmdKhachHang.Parameters.AddWithValue("@soDienThoai", soDienThoai);
                            cmdKhachHang.Parameters.AddWithValue("@diaChi", diaChi);
                            cmdKhachHang.Parameters.AddWithValue("@email", email);
                            cmdKhachHang.ExecuteNonQuery();
                        }

                        MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 🔹 Quay về màn hình đăng nhập
                        Main mainForm = new Main();
                        mainForm.Show();
                        this.Close();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
