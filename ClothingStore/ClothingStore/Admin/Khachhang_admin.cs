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
using ClothingStore.NhanVien;
using MySql.Data.MySqlClient;

namespace ClothingStore.Admin
{
    public partial class Khachhang_admin : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;

        public Khachhang_admin()
        {
            InitializeComponent();
            LoadKhachHang(); // Hiển thị dữ liệu khi mở form
        }

        // 🟢 HIỂN THỊ DANH SÁCH KHÁCH HÀNG
        private void LoadKhachHang()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM KhachHang";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    ClearTextBoxes(); // Xóa dữ liệu trên textbox
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 🟢 KHI CLICK VÀO DÒNG TRONG DGV -> HIỂN THỊ DỮ LIỆU LÊN TEXTBOX
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo chọn dòng hợp lệ
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtMaKhachHang.Text = row.Cells["MaKhachHang"].Value.ToString();
                txtTenKhach.Text = row.Cells["TenKhach"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
            }
        }

        // 🟢 XÓA KHÁCH HÀNG & TÀI KHOẢN LIÊN QUAN
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text))
            {
                MessageBox.Show("Hãy chọn một dòng để thao tác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string maKhachHang =Convert.ToString(txtMaKhachHang.Text);

                        string updateHoaDonBanQuery = "UPDATE HoaDonBan SET MaKhachHang = NULL WHERE MaKhachHang = @MaKhachHang";
                        MySqlCommand cmd0 = new MySqlCommand(updateHoaDonBanQuery, conn);
                        cmd0.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                        cmd0.ExecuteNonQuery();

                        // Xóa tài khoản nếu có
                        string deleteTaiKhoanQuery = "DELETE FROM TaiKhoan WHERE MaTaiKhoan = @MaKhachHang";
                        MySqlCommand cmd1 = new MySqlCommand(deleteTaiKhoanQuery, conn);
                        cmd1.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                        cmd1.ExecuteNonQuery();

                        // Xóa khách hàng
                        string deleteKhachHangQuery = "DELETE FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
                        MySqlCommand cmd2 = new MySqlCommand(deleteKhachHangQuery, conn);
                        cmd2.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                        cmd2.ExecuteNonQuery();

                        MessageBox.Show("Đã xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKhachHang(); // Cập nhật lại DataGridView
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 🟢 CẬP NHẬT THÔNG TIN KHÁCH HÀNG (KHÔNG ĐƯỢC CHỈNH SỬA MÃ KHÁCH HÀNG)
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text))
            {
                MessageBox.Show("Hãy chọn một dòng để thao tác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string updateQuery = "UPDATE KhachHang SET TenKhach = @TenKhach, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, Email=@Email WHERE MaKhachHang = @MaKhachHang";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@TenKhach", txtTenKhach.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text);
                    cmd.Parameters.AddWithValue("@MaKhachHang", txtMaKhachHang.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Đã cập nhật thông tin khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhachHang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 🟢 TÌM KIẾM KHÁCH HÀNG THEO MÃ KHÁCH HÀNG
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKhachHang.Text))
            {
                MessageBox.Show("Hãy nhập mã khách hàng để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM KhachHang WHERE MaKhachHang = @MaKhachHang";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaKhachHang", txtMaKhachHang.Text);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 🟢 HIỂN THỊ DANH SÁCH KHÁCH HÀNG & LÀM SẠCH TEXTBOX
        private void btnHienThi_Click(object sender, EventArgs e)
        {
            LoadKhachHang();
        }

        // 🟢 QUAY LẠI FORM ADMIN
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Hide();
            Adimin adminForm = new Adimin();
            adminForm.Show();
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

        // 🟢 HÀM XÓA TEXTBOX
        private void ClearTextBoxes()
        {
            txtMaKhachHang.Clear();
            txtTenKhach.Clear();
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
        }
    }
}
