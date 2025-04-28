using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClothingStore.Admin;
using ClothingStore.Class;
using MySql.Data.MySqlClient;

namespace ClothingStore.KhachHang
{
    public partial class KhachHang : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        private MySqlConnection conn;

        public KhachHang()
        {
            InitializeComponent();
            conn = new MySqlConnection(connectionString);
            LoadComboBoxes();
            LoadDataGridView();
        }

        private void LoadComboBoxes()
        {
            try
            {
                conn.Open();
                // Load Loai ComboBox
                var cmdLoai = new MySqlCommand("SELECT TenLoai FROM TheLoai", conn);
                var readerLoai = cmdLoai.ExecuteReader();
                while (readerLoai.Read())
                {
                    cboLoai.Items.Add(readerLoai.GetString("TenLoai"));
                }
                readerLoai.Close();

                // Load Co ComboBox
                var cmdCo = new MySqlCommand("SELECT TenCo FROM Co", conn);
                var readerCo = cmdCo.ExecuteReader();
                while (readerCo.Read())
                {
                    cboCo.Items.Add(readerCo.GetString("TenCo"));
                }
                readerCo.Close();

                // Load Mau ComboBox
                var cmdMau = new MySqlCommand("SELECT TenMau FROM Mau", conn);
                var readerMau = cmdMau.ExecuteReader();
                while (readerMau.Read())
                {
                    cboMau.Items.Add(readerMau.GetString("TenMau"));
                }
                readerMau.Close();

                // Load Mua ComboBox
                var cmdMua = new MySqlCommand("SELECT TenMua FROM Mua", conn);
                var readerMua = cmdMua.ExecuteReader();
                while (readerMua.Read())
                {
                    cboMua.Items.Add(readerMua.GetString("TenMua"));
                }
                readerMua.Close();

                // Load DoiTuong ComboBox
                var cmdDoiTuong = new MySqlCommand("SELECT TenDoiTuong FROM DoiTuong", conn);
                var readerDoiTuong = cmdDoiTuong.ExecuteReader();
                while (readerDoiTuong.Read())
                {
                    cboDoiTuong.Items.Add(readerDoiTuong.GetString("TenDoiTuong"));
                }
                readerDoiTuong.Close();

                // Load ChatLieu ComboBox
                var cmdChatLieu = new MySqlCommand("SELECT TenChatLieu FROM ChatLieu", conn);
                var readerChatLieu = cmdChatLieu.ExecuteReader();
                while (readerChatLieu.Read())
                {
                    cboChatLieu.Items.Add(readerChatLieu.GetString("TenChatLieu"));
                }
                readerChatLieu.Close();

                // Load NoiSanXuat ComboBox
                var cmdNoiSanXuat = new MySqlCommand("SELECT TenNSX FROM NoiSanXuat", conn);
                var readerNoiSanXuat = cmdNoiSanXuat.ExecuteReader();
                while (readerNoiSanXuat.Read())
                {
                    cboNoiSanXuat.Items.Add(readerNoiSanXuat.GetString("TenNSX"));
                }
                readerNoiSanXuat.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading ComboBoxes: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadDataGridView()
        {
            try
            {
                conn.Open();
                var query = "SELECT s.MaQuanAo, s.TenQuanAo, t.TenLoai, c.TenCo, m.TenMau, mu.TenMua, dt.TenDoiTuong, cl.TenChatLieu, ns.TenNSX, s.DonGiaBan " +
                            "FROM SanPham s " +
                            "JOIN TheLoai t ON s.MaLoai = t.MaLoai " +
                            "JOIN Co c ON s.MaCo = c.MaCo " +
                            "JOIN Mau m ON s.MaMau = m.MaMau " +
                            "JOIN Mua mu ON s.MaMua = mu.MaMua " +
                            "JOIN DoiTuong dt ON s.MaDoiTuong = dt.MaDoiTuong " +
                            "JOIN ChatLieu cl ON s.MaChatLieu = cl.MaChatLieu " +
                            "JOIN NoiSanXuat ns ON s.MaNSX = ns.MaNSX";
                var cmd = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading DataGridView: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private string LayAnhTuDatabase(int maQuanAo)
        {
            string anh = "";
            string query = "SELECT Anh FROM SanPham WHERE MaQuanAo = @MaQuanAo";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            anh = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi truy vấn ảnh: " + ex.Message);
                }
            }
            return anh;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];

                // Hiển thị dữ liệu lên các TextBox và ComboBox
                txtMaQuanAo.Text = row.Cells["MaQuanAo"].Value.ToString();
                txtTenQuanAo.Text = row.Cells["TenQuanAo"].Value.ToString();
                txtDonGiaBan.Text = row.Cells["DonGiaBan"].Value.ToString();
                try
                {
                    // Kiểm tra nếu cột 'MaQuanAo' tồn tại
                    if (dataGridView1.Columns.Contains("MaQuanAo") &&
                        dataGridView1.Rows[e.RowIndex].Cells["MaQuanAo"].Value != DBNull.Value)
                    {
                        int maQuanAo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["MaQuanAo"].Value);

                        // Lấy ảnh từ cơ sở dữ liệu dựa trên MaQuanAo
                        string anh = LayAnhTuDatabase(maQuanAo);

                        if (!string.IsNullOrEmpty(anh))
                        {
                            pictureBox1.ImageLocation = anh;
                        }
                        else
                        {
                            pictureBox1.Image = null; // Nếu không có ảnh, xóa ảnh hiện tại
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy xuất ảnh: " + ex.Message);
                }
                try
                {
                    // Kiểm tra và gán giá trị cho ComboBox Loai
                    if (dataGridView1.Columns.Contains("TenLoai") &&
                        dataGridView1.Rows[e.RowIndex].Cells["TenLoai"].Value != DBNull.Value)
                    {
                        cboLoai.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["TenLoai"].Value.ToString();
                    }
                    else
                    {
                        cboLoai.SelectedItem = null; // Đặt giá trị mặc định hoặc null nếu không có giá trị
                    }

                    // Kiểm tra và gán giá trị cho ComboBox Cỡ
                    if (dataGridView1.Columns.Contains("TenCo") &&
                        dataGridView1.Rows[e.RowIndex].Cells["TenCo"].Value != DBNull.Value)
                    {
                        cboCo.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["TenCo"].Value.ToString();
                    }
                    else
                    {
                        cboCo.SelectedItem = null;
                    }

                    // Kiểm tra và gán giá trị cho ComboBox Màu
                    if (dataGridView1.Columns.Contains("TenMau") &&
                        dataGridView1.Rows[e.RowIndex].Cells["TenMau"].Value != DBNull.Value)
                    {
                        cboMau.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["TenMau"].Value.ToString();
                    }
                    else
                    {
                        cboMau.SelectedItem = null;
                    }

                    // Kiểm tra và gán giá trị cho ComboBox Mùa
                    if (dataGridView1.Columns.Contains("TenMua") &&
                        dataGridView1.Rows[e.RowIndex].Cells["TenMua"].Value != DBNull.Value)
                    {
                        cboMua.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["TenMua"].Value.ToString();
                    }
                    else
                    {
                        cboMua.SelectedItem = null;
                    }

                    // Kiểm tra và gán giá trị cho ComboBox Đối tượng
                    if (dataGridView1.Columns.Contains("TenDoiTuong") &&
                        dataGridView1.Rows[e.RowIndex].Cells["TenDoiTuong"].Value != DBNull.Value)
                    {
                        cboDoiTuong.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["TenDoiTuong"].Value.ToString();
                    }
                    else
                    {
                        cboDoiTuong.SelectedItem = null;
                    }

                    // Kiểm tra và gán giá trị cho ComboBox Chất liệu
                    if (dataGridView1.Columns.Contains("TenChatLieu") &&
                        dataGridView1.Rows[e.RowIndex].Cells["TenChatLieu"].Value != DBNull.Value)
                    {
                        cboChatLieu.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["TenChatLieu"].Value.ToString();
                    }
                    else
                    {
                        cboChatLieu.SelectedItem = null;
                    }

                    // Kiểm tra và gán giá trị cho ComboBox Nơi sản xuất
                    if (dataGridView1.Columns.Contains("TenNSX") &&
                        dataGridView1.Rows[e.RowIndex].Cells["TenNSX"].Value != DBNull.Value)
                    {
                        cboNoiSanXuat.SelectedItem = dataGridView1.Rows[e.RowIndex].Cells["TenNSX"].Value.ToString();
                    }
                    else
                    {
                        cboNoiSanXuat.SelectedItem = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy cập cột trong DataGridView: " + ex.Message);
                }
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

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE taikhoan SET DangNhap = 0 WHERE MaTaiKhoan = @MaTaiKhoan";
                MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                cmd.ExecuteNonQuery();
            }

            // Sau khi update thành công, clear session
            SessionManager.ClearSession();

            DoiMatKhau doimatkhauForm = new DoiMatKhau();
            doimatkhauForm.Show();
            this.Hide();
        }
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            mainForm.Show();
            this.Close();
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "SELECT s.MaQuanAo, s.TenQuanAo, t.TenLoai, c.TenCo, m.TenMau, mu.TenMua, dt.TenDoiTuong, cl.TenChatLieu, ns.TenNSX, s.Anh, s.DonGiaBan " +
                               "FROM SanPham s " +
                               "JOIN TheLoai t ON s.MaLoai = t.MaLoai " +
                               "JOIN Co c ON s.MaCo = c.MaCo " +
                               "JOIN Mau m ON s.MaMau = m.MaMau " +
                               "JOIN Mua mu ON s.MaMua = mu.MaMua " +
                               "JOIN DoiTuong dt ON s.MaDoiTuong = dt.MaDoiTuong " +
                               "JOIN ChatLieu cl ON s.MaChatLieu = cl.MaChatLieu " +
                               "JOIN NoiSanXuat ns ON s.MaNSX = ns.MaNSX WHERE 1=1 ";

                if (!string.IsNullOrEmpty(txtMaQuanAo.Text))
                    query += " AND s.MaQuanAo = @MaQuanAo";

                if (cboLoai.SelectedItem != null)
                    query += " AND t.TenLoai = @TenLoai";

                if (cboCo.SelectedItem != null)
                    query += " AND c.TenCo = @TenCo";

                if (cboMau.SelectedItem != null)
                    query += " AND m.TenMau = @TenMau";

                if (cboMua.SelectedItem != null)
                    query += " AND mu.TenMua = @TenMua";

                if (cboDoiTuong.SelectedItem != null)
                    query += " AND dt.TenDoiTuong = @TenDoiTuong";

                if (cboChatLieu.SelectedItem != null)
                    query += " AND cl.TenChatLieu = @TenChatLieu";

                if (cboNoiSanXuat.SelectedItem != null)
                    query += " AND ns.TenNSX = @TenNSX";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (!string.IsNullOrEmpty(txtMaQuanAo.Text))
                    cmd.Parameters.AddWithValue("@MaQuanAo", txtMaQuanAo.Text);

                if (cboLoai.SelectedItem != null)
                    cmd.Parameters.AddWithValue("@TenLoai", cboLoai.SelectedItem.ToString());

                if (cboCo.SelectedItem != null)
                    cmd.Parameters.AddWithValue("@TenCo", cboCo.SelectedItem.ToString());

                if (cboMau.SelectedItem != null)
                    cmd.Parameters.AddWithValue("@TenMau", cboMau.SelectedItem.ToString());

                if (cboMua.SelectedItem != null)
                    cmd.Parameters.AddWithValue("@TenMua", cboMua.SelectedItem.ToString());

                if (cboDoiTuong.SelectedItem != null)
                    cmd.Parameters.AddWithValue("@TenDoiTuong", cboDoiTuong.SelectedItem.ToString());

                if (cboChatLieu.SelectedItem != null)
                    cmd.Parameters.AddWithValue("@TenChatLieu", cboChatLieu.SelectedItem.ToString());

                if (cboNoiSanXuat.SelectedItem != null)
                    cmd.Parameters.AddWithValue("@TenNSX", cboNoiSanXuat.SelectedItem.ToString());

                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SessionManager.MaTaiKhoanDangNhap))
            {
                MessageBox.Show("Bạn chưa đăng nhập!");
                return;
            }

            if (string.IsNullOrEmpty(txtMaQuanAo.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!");
                return;
            }

            int soLuong = Convert.ToInt32(textBox1.Text.Trim());
            if (soLuong <= 0)
            {
                MessageBox.Show("Số lượng đặt phải lớn hơn 0!");
                return;
            }
            if(label4.Text == "Giỏ hàng của bạn")
            {
                MessageBox.Show("Không thể thực hiện !!!!!");
                return;
            }

            try
            {
                conn.Open();

                // 🛠️ Kiểm tra khách hàng có tồn tại không (tìm theo MaTaiKhoan)
                string checkKhachHang = "SELECT COUNT(*) FROM KhachHang WHERE MaTaiKhoan = @MaTaiKhoan";
                MySqlCommand cmdCheckKH = new MySqlCommand(checkKhachHang, conn);
                cmdCheckKH.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                int khachHangCount = Convert.ToInt32(cmdCheckKH.ExecuteScalar());

                if (khachHangCount == 0)
                {
                    MessageBox.Show("Khách hàng không tồn tại!");
                    return;
                }

                // 🔄 Lấy MaKhachHang từ MaTaiKhoan
                string getMaKhachHang = "SELECT MaKhachHang FROM KhachHang WHERE MaTaiKhoan = @MaTaiKhoan";
                MySqlCommand cmdGetMaKH = new MySqlCommand(getMaKhachHang, conn);
                cmdGetMaKH.Parameters.AddWithValue("@MaTaiKhoan", SessionManager.MaTaiKhoanDangNhap);
                string maKhachHang = cmdGetMaKH.ExecuteScalar()?.ToString();

                if (string.IsNullOrEmpty(maKhachHang))
                {
                    MessageBox.Show("Không tìm thấy mã khách hàng!");
                    return;
                }

                // 🛠️ Kiểm tra sản phẩm có tồn tại không
                string checkSanPham = "SELECT COUNT(*) FROM SanPham WHERE MaQuanAo = @MaQuanAo";
                MySqlCommand cmdCheckSP = new MySqlCommand(checkSanPham, conn);
                cmdCheckSP.Parameters.AddWithValue("@MaQuanAo", txtMaQuanAo.Text);
                int sanPhamCount = Convert.ToInt32(cmdCheckSP.ExecuteScalar());

                if (sanPhamCount == 0)
                {
                    MessageBox.Show("Sản phẩm không tồn tại!");
                    return;
                }

                // Thêm vào giỏ hàng
                string query = @"INSERT INTO GioHang (MaKhachHang, MaQuanAo, DonGiaBan, SoLuongDat) 
                     VALUES (@MaKhachHang, @MaQuanAo, @DonGiaBan, @SoLuong)
                     ON DUPLICATE KEY UPDATE SoLuongDat = SoLuongDat + @SoLuong";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKhachHang", maKhachHang);  // Sử dụng MaKhachHang thay vì MaTaiKhoan
                cmd.Parameters.AddWithValue("@MaQuanAo", txtMaQuanAo.Text);
                cmd.Parameters.AddWithValue("@DonGiaBan", decimal.Parse(txtDonGiaBan.Text));
                cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm vào giỏ hàng thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm vào giỏ hàng: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void btnXemGioHang_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = "SELECT g.MaQuanAo, s.TenQuanAo, g.DonGiaBan, g.SoLuongDat, g.TongTien FROM GioHang g " +
                               "JOIN SanPham s ON g.MaQuanAo = s.MaQuanAo " +
                               "WHERE g.MaKhachHang = @MaKhachHang";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKhachHang", SessionManager.MaTaiKhoanDangNhap);
                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                label4.Text = "Giỏ hàng của bạn";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem giỏ hàng: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(label4.Text == "Giỏ hàng của bạn")
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    // Lấy mã sản phẩm từ ô được chọn
                    string maQuanAo = dataGridView1.SelectedCells[0].OwningRow.Cells["MaQuanAo"].Value.ToString();

                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM GioHang WHERE MaKhachHang = @MaKhachHang AND MaQuanAo = @MaQuanAo";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaKhachHang", SessionManager.MaTaiKhoanDangNhap);
                        cmd.Parameters.AddWithValue("@MaQuanAo", maQuanAo);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa sản phẩm khỏi giỏ hàng thành công!");
                            conn.Close();
                            btnXemGioHang.PerformClick(); // Reload giỏ hàng
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sản phẩm trong giỏ hàng để xóa.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa sản phẩm: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm để xóa.");
                }
            }
            else
            {
                MessageBox.Show("Không thể thực hiện!!!");
            }
        }
        private void BtnDatHang_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đặt hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Bạn đã đặt hàng thành công!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataGridView();
            label4.Text = "Danh sách sản phẩm";
            cboChatLieu.Text = null;
            cboCo.Text = null;
            cboDoiTuong.Text = null;
            cboLoai.Text = null;
            cboMau.Text = null;
            cboMua.Text = null;
            cboNoiSanXuat.Text = null;
            txtMaQuanAo.Clear();
            txtTenQuanAo.Clear();
            txtDonGiaBan.Clear();
        }
    }
}
