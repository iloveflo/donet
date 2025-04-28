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
    public partial class NhanVien_Admin : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;

        public NhanVien_Admin()
        {
            InitializeComponent();
            LoadComboBoxData();
            LoadDataGridView();
        }

        private void LoadComboBoxData()
        {
            cmbGioiTinh.Items.AddRange(new string[] { "Nam", "Nữ", "Khác" });
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM CongViec";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbCongViec.Items.Add(reader["TenCongViec"].ToString());
                }
            }
        }

        private void LoadDataGridView()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT NhanVien.MaNhanVien, TenNhanVien, NgaySinh, DiaChi, SoDienThoai, GioiTinh, TenCongViec, Email " +
                               "FROM NhanVien JOIN CongViec ON NhanVien.MaCongViec = CongViec.MaCongViec";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvNhanVien.DataSource = dt;
                dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvNhanVien.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
        }
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo chọn dòng hợp lệ
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value?.ToString() ?? "";
                txtTenNhanVien.Text = row.Cells["TenNhanVien"].Value?.ToString() ?? "";
                

                // Xử lý NgaySinh nếu có giá trị
                if (row.Cells["NgaySinh"].Value != DBNull.Value && row.Cells["NgaySinh"].Value != null)
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                else
                    dtpNgaySinh.Value = DateTime.Now; // hoặc giá trị mặc định bạn muốn

                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString() ?? "";

                // Set giới tính nếu có
                string gioiTinh = row.Cells["GioiTinh"].Value?.ToString();
                if (!string.IsNullOrEmpty(gioiTinh))
                    cmbGioiTinh.SelectedItem = gioiTinh;

                // Set công việc nếu có
                string tenCongViec = row.Cells["TenCongViec"].Value?.ToString();
                if (!string.IsNullOrEmpty(tenCongViec))
                    cmbCongViec.SelectedItem = tenCongViec;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text) ||
                string.IsNullOrWhiteSpace(txtTenNhanVien.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text) ||
                string.IsNullOrWhiteSpace(txtSoDienThoai.Text) || string.IsNullOrWhiteSpace(txtEmail.Text)||
                cmbGioiTinh.SelectedItem == null || cmbCongViec.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra mã nhân viên đã tồn tại trong bảng NhanVien
                string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE MaNhanVien = @MaNV";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@MaNV", txtMaNhanVien.Text);
                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (exists > 0)
                {
                    MessageBox.Show("Mã nhân viên đã tồn tại!");
                    return;
                }

                // 2. Kiểm tra số điện thoại có bị trùng ở cả 2 bảng
                string checkSDTQuery = @"
        SELECT COUNT(*) FROM (
            SELECT SoDienThoai FROM NhanVien WHERE SoDienThoai = @SDT
            UNION ALL
            SELECT SoDienThoai FROM KhachHang WHERE SoDienThoai = @SDT
        ) AS Temp";
                MySqlCommand checkSDTCmd = new MySqlCommand(checkSDTQuery, conn);
                checkSDTCmd.Parameters.AddWithValue("@SDT", txtSoDienThoai.Text);
                int existsSDT = Convert.ToInt32(checkSDTCmd.ExecuteScalar());
                if (existsSDT > 0)
                {
                    MessageBox.Show("Số điện thoại đã tồn tại!");
                    return;
                }

                // 3. Kiểm tra email có bị trùng ở cả 2 bảng
                string checkEmailQuery = @"
        SELECT COUNT(*) FROM (
            SELECT Email FROM NhanVien WHERE Email = @Email
            UNION ALL
            SELECT Email FROM KhachHang WHERE Email = @Email
        ) AS Temp";
                MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, conn);
                checkEmailCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                int existsEmail = Convert.ToInt32(checkEmailCmd.ExecuteScalar());
                if (existsEmail > 0)
                {
                    MessageBox.Show("Email đã tồn tại!");
                    return;
                }

                // Thêm nhân viên vào bảng NhanVien
                string insertQuery = "INSERT INTO NhanVien (TenNhanVien, NgaySinh, DiaChi,Email, SoDienThoai, GioiTinh, MaCongViec) " +
                                     "VALUES (@TenNV, @NgaySinh, @DiaChi,@Email, @SDT, @GioiTinh, (SELECT MaCongViec FROM CongViec WHERE TenCongViec=@CongViec))";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@TenNV", txtTenNhanVien.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSoDienThoai.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", cmbGioiTinh.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@CongViec", cmbCongViec.SelectedItem.ToString());
                cmd.ExecuteNonQuery();

                // Lấy MaNhanVien mới tạo ra sau khi thêm vào bảng NhanVien
                string getMaNhanVienQuery = "SELECT LAST_INSERT_ID()";
                MySqlCommand getMaNhanVienCmd = new MySqlCommand(getMaNhanVienQuery, conn);
                int maNhanVien = Convert.ToInt32(getMaNhanVienCmd.ExecuteScalar());

                // Thêm tài khoản vào bảng TaiKhoan
                string queryTaiKhoan = "INSERT INTO TaiKhoan (MaTaiKhoan, TenDangNhap, MatKhau, LoaiTaiKhoan) " +
                                       "VALUES (@MaTaiKhoan, @TenDangNhap, @MatKhau, 'NhanVien')";
                MySqlCommand cmdTK = new MySqlCommand(queryTaiKhoan, conn);
                cmdTK.Parameters.AddWithValue("@MaTaiKhoan", maNhanVien); // Sử dụng MaNhanVien mới tạo
                cmdTK.Parameters.AddWithValue("@TenDangNhap", maNhanVien);
                cmdTK.Parameters.AddWithValue("@MatKhau", maNhanVien); // Mật khẩu mặc định
                cmdTK.ExecuteNonQuery();

                //thêm lại mã tài khoản vào bảng nhân viên;
                string updateNV = "UPDATE NhanVien SET MaTaiKhoan = @MaTaiKhoan WHERE MaNhanVien = @MaNhanVien";
                MySqlCommand cmdNV = new MySqlCommand(updateNV, conn);
                cmdNV.Parameters.AddWithValue("@MaTaiKhoan", maNhanVien);
                cmdNV.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                cmdNV.ExecuteNonQuery();


                MessageBox.Show("Đã thêm nhân viên thành công!");
                LoadDataGridView();
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNhanVien.Text))
            {
                MessageBox.Show("Hãy chọn một dòng để thao tác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này? Dữ liệu hóa đơn sẽ được giữ lại!",
                                                  "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string maNhanVien = txtMaNhanVien.Text;

                        // Cập nhật MaNhanVien trong HoaDonBan thành NULL
                        string updateHoaDonBanQuery = "UPDATE HoaDonBan SET MaNhanVien = NULL WHERE MaNhanVien = @MaNhanVien";
                        MySqlCommand cmd1 = new MySqlCommand(updateHoaDonBanQuery, conn);
                        cmd1.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd1.ExecuteNonQuery();

                        string updateHoaDonNhapQuery = "UPDATE HoaDonNhap SET MaNhanVien = NULL WHERE MaNhanVien = @MaNhanVien";
                        MySqlCommand cmd4 = new MySqlCommand(updateHoaDonNhapQuery, conn);
                        cmd4.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd4.ExecuteNonQuery();

                        // Xóa tài khoản nếu có
                        string deleteTaiKhoanQuery = "DELETE FROM TaiKhoan WHERE MaTaiKhoan = @MaNhanVien";
                        MySqlCommand cmd2 = new MySqlCommand(deleteTaiKhoanQuery, conn);
                        cmd2.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd2.ExecuteNonQuery();

                        // Xóa nhân viên
                        string deleteNhanVienQuery = "DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien";
                        MySqlCommand cmd3 = new MySqlCommand(deleteNhanVienQuery, conn);
                        cmd3.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                        cmd3.ExecuteNonQuery();

                        MessageBox.Show("Đã xóa nhân viên thành công! Dữ liệu hóa đơn vẫn được giữ lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataGridView(); // Cập nhật lại DataGridView
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtMaNhanVien.Enabled = false;
            button1.Enabled = false;
            btnXoa.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtMaNhanVien.Enabled = true;
            button1.Enabled = true;
            btnXoa.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNhanVien.Text))
            {
                MessageBox.Show("Hãy chọn một dòng để thao tác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string updateQuery = "UPDATE NhanVien SET TenNhanVien = @TenNhanVien, NgaySinh = @NgaySinh, DiaChi = @DiaChi,Email=@Email, SoDienThoai = @SoDienThoai, GioiTinh = @GioiTinh, MaCongViec = (SELECT MaCongViec FROM CongViec WHERE TenCongViec = @TenCongViec) WHERE MaNhanVien = @MaNhanVien";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@TenNhanVien", txtTenNhanVien.Text);
                    cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@SoDienThoai", txtSoDienThoai.Text);
                    cmd.Parameters.AddWithValue("@GioiTinh", cmbGioiTinh.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TenCongViec", cmbCongViec.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@MaNhanVien", txtMaNhanVien.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Đã cập nhật thông tin nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNhanVien.Text))
            {
                MessageBox.Show("Hãy nhập mã nhân viên để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM NhanVien WHERE MaNhanVien = @MaNhanVien";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaNhanVien", txtMaNhanVien.Text);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvNhanVien.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnHienThi_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Truy vấn dữ liệu từ bảng NhanVien kết hợp với bảng CongViec để lấy thông tin công việc
                string query = "SELECT N.MaNhanVien, N.TenNhanVien, N.NgaySinh, N.DiaChi,N.Email, N.SoDienThoai, N.GioiTinh, C.TenCongViec " +
                               "FROM NhanVien N " +
                               "JOIN CongViec C ON N.MaCongViec = C.MaCongViec";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Gán dữ liệu vào DataGridView
                dgvNhanVien.DataSource = dt;
            }

            // Làm sạch các textbox, combobox, datetimepicker
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            cmbGioiTinh.SelectedIndex = -1;
            cmbCongViec.SelectedIndex = -1;
            dtpNgaySinh.Value = DateTime.Now;
        }
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
    }
}
