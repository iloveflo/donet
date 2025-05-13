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
    public partial class TaiKhoan : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        public TaiKhoan()
        {
            InitializeComponent();
        }
        private void ThongTinTaiKhoanForm_Load(object sender, EventArgs e)
        {
            string maTK = SessionManager.MaTaiKhoanDangNhap;
            if (!string.IsNullOrEmpty(maTK))
            {
                LoadThongTinTaiKhoan(maTK);
                txtHoTen.Enabled = false;
                txtDiaChi.Enabled = false;
                txtEmail.Enabled = false;
                txtMaTaiKhoan.Enabled = false;
                txtSoDienThoai.Enabled = false;
                txtUserName.Enabled = false;
                btnHuy.Enabled = false;
                btnLuu.Enabled = false;
            }
            else
            {
                MessageBox.Show("Không tìm thấy mã tài khoản đăng nhập.", "Lỗi");
            }
        }
        private void LoadThongTinTaiKhoan(string maTaiKhoan)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Lấy tên đăng nhập từ bảng taikhoan
                string queryTaiKhoan = "SELECT TenDangNhap, LoaiTaiKhoan FROM taikhoan WHERE MaTaiKhoan = @MaTaiKhoan";
                string tenDangNhap = "";
                string loaiTaiKhoan = "";

                using (MySqlCommand cmd = new MySqlCommand(queryTaiKhoan, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tenDangNhap = reader["TenDangNhap"].ToString();
                            loaiTaiKhoan = reader["LoaiTaiKhoan"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Tài khoản không tồn tại trong hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                // Xử lý theo loại tài khoản
                string queryThongTin = "";

                if (loaiTaiKhoan == "NhanVien")
                {
                    queryThongTin = @"SELECT TenNhanVien AS Ten, SoDienThoai, Email, DiaChi, MaTaiKhoan 
                              FROM nhanvien WHERE MaTaiKhoan = @MaTaiKhoan LIMIT 1";
                }
                else if (loaiTaiKhoan == "KhachHang")
                {
                    queryThongTin = @"SELECT TenKhach AS Ten, SoDienThoai, Email, DiaChi, MaTaiKhoan 
                              FROM khachhang WHERE MaTaiKhoan = @MaTaiKhoan LIMIT 1";
                }
                else
                {
                    MessageBox.Show("Loại tài khoản không hợp lệ hoặc chưa hỗ trợ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (MySqlCommand cmd = new MySqlCommand(queryThongTin, conn))
                {
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtHoTen.Text = reader["Ten"].ToString();
                            txtSoDienThoai.Text = reader["SoDienThoai"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtDiaChi.Text = reader["DiaChi"].ToString();
                            txtUserName.Text = tenDangNhap;
                            txtMaTaiKhoan.Text = reader["MaTaiKhoan"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin người dùng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (SessionManager.LoaiTaiKhoanDangNhap == "KhachHang")
            {
                KhachHang.KhachHang khachHang= new KhachHang.KhachHang();
                khachHang.Show();
                this.Hide();
            }
            if (SessionManager.LoaiTaiKhoanDangNhap == "NhanVien")
            {
                NhanVien.Nhan_Vien nhan_Vien=new NhanVien.Nhan_Vien();
                nhan_Vien.Show();
                this.Hide();
            }
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Cho phép chỉnh sửa các thông tin (trừ tài khoản)
            txtHoTen.Enabled = true;
            txtSoDienThoai.Enabled = true;
            txtEmail.Enabled = true;
            txtDiaChi.Enabled = true;

            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            btnSua.Enabled = false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Load lại dữ liệu gốc
            LoadThongTinTaiKhoan(SessionManager.MaTaiKhoanDangNhap);

            // Vô hiệu hóa các ô nhập
            txtHoTen.Enabled = false;
            txtSoDienThoai.Enabled = false;
            txtEmail.Enabled = false;
            txtDiaChi.Enabled = false;

            btnLuu.Enabled = false;
            btnHuy.Enabled = false;
            btnSua.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maTK = txtMaTaiKhoan.Text;
            string email = txtEmail.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            string hoten = txtHoTen.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra trùng email hoặc SDT trong nhân viên hoặc khách hàng (trừ chính mình)
                string queryCheck = @"
            SELECT COUNT(*) FROM (
                SELECT Email FROM nhanvien WHERE Email = @Email AND MaTaiKhoan != @MaTK
                UNION ALL
                SELECT Email FROM khachhang WHERE Email = @Email AND MaTaiKhoan != @MaTK
                UNION ALL
                SELECT SoDienThoai FROM nhanvien WHERE SoDienThoai = @SDT AND MaTaiKhoan != @MaTK
                UNION ALL
                SELECT SoDienThoai FROM khachhang WHERE SoDienThoai = @SDT AND MaTaiKhoan != @MaTK
            ) AS Duplicates;";

                MySqlCommand cmdCheck = new MySqlCommand(queryCheck, conn);
                cmdCheck.Parameters.AddWithValue("@Email", email);
                cmdCheck.Parameters.AddWithValue("@SDT", sdt);
                cmdCheck.Parameters.AddWithValue("@MaTK", maTK);

                int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Email hoặc số điện thoại đã tồn tại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy loại tài khoản
                string loai = "";
                MySqlCommand cmdLoai = new MySqlCommand("SELECT LoaiTaiKhoan FROM taikhoan WHERE MaTaiKhoan = @MaTK", conn);
                cmdLoai.Parameters.AddWithValue("@MaTK", maTK);
                using (MySqlDataReader reader = cmdLoai.ExecuteReader())
                {
                    if (reader.Read()) loai = reader["LoaiTaiKhoan"].ToString();
                }

                // Cập nhật dữ liệu
                string queryUpdate = "";
                if (loai == "NhanVien")
                {
                    queryUpdate = @"UPDATE nhanvien 
                            SET TenNhanVien = @Hoten, SoDienThoai = @SDT, Email = @Email, DiaChi = @DiaChi 
                            WHERE MaTaiKhoan = @MaTK";
                }
                else if (loai == "KhachHang")
                {
                    queryUpdate = @"UPDATE khachhang 
                            SET TenKhach = @Hoten, SoDienThoai = @SDT, Email = @Email, DiaChi = @DiaChi 
                            WHERE MaTaiKhoan = @MaTK";
                }

                if (!string.IsNullOrEmpty(queryUpdate))
                {
                    MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, conn);
                    cmdUpdate.Parameters.AddWithValue("@Hoten", hoten);
                    cmdUpdate.Parameters.AddWithValue("@SDT", sdt);
                    cmdUpdate.Parameters.AddWithValue("@Email", email);
                    cmdUpdate.Parameters.AddWithValue("@DiaChi", diachi);
                    cmdUpdate.Parameters.AddWithValue("@MaTK", maTK);

                    int result = cmdUpdate.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnHuy.PerformClick(); // Reload lại
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu nào được cập nhật.", "Thông báo");
                    }
                }
            }
        }
    }
}
