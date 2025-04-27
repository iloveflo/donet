using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ClothingStore.Class;

namespace ClothingStore.Admin
{
    public partial class SanPham_Admin : Form
    {
        public SanPham_Admin()
        {
            InitializeComponent();
            LoadDataGridView();
        }
        private void SanPham_Admin_Load(object sender, EventArgs e)
        {
            // Thêm giá trị vào ComboBox Loại
            comboBoxLoai.Items.Add("Áo");
            comboBoxLoai.Items.Add("Quần");
            comboBoxLoai.Items.Add("Váy");
            comboBoxLoai.Items.Add("Giày");
            comboBoxLoai.Items.Add("Phụ kiện");

            // Thêm giá trị vào ComboBox Cỡ
            comboBoxCo.Items.Add("S");
            comboBoxCo.Items.Add("M");
            comboBoxCo.Items.Add("L");
            comboBoxCo.Items.Add("XL");
            comboBoxCo.Items.Add("XXL");

            // Thêm giá trị vào ComboBox Chất liệu
            comboBoxChatLieu.Items.Add("Cotton");
            comboBoxChatLieu.Items.Add("Jeans");
            comboBoxChatLieu.Items.Add("Polyester");
            comboBoxChatLieu.Items.Add("Lụa");
            comboBoxChatLieu.Items.Add("Len");

            // Thêm giá trị vào ComboBox Màu
            comboBoxMau.Items.Add("Đỏ");
            comboBoxMau.Items.Add("Xanh");
            comboBoxMau.Items.Add("Vàng");
            comboBoxMau.Items.Add("Trắng");
            comboBoxMau.Items.Add("Đen");

            // Thêm giá trị vào ComboBox Đối tượng
            comboBoxDoiTuong.Items.Add("Nam");
            comboBoxDoiTuong.Items.Add("Nữ");
            comboBoxDoiTuong.Items.Add("Trẻ em");
            comboBoxDoiTuong.Items.Add("Unisex");
            comboBoxDoiTuong.Items.Add("Người già");

            // Thêm giá trị vào ComboBox Mùa
            comboBoxMua.Items.Add("Xuân");
            comboBoxMua.Items.Add("Hạ");
            comboBoxMua.Items.Add("Thu");
            comboBoxMua.Items.Add("Đông");
            comboBoxMua.Items.Add("Quanh năm");

            // Thêm giá trị vào ComboBox Nơi sản xuất
            comboBoxNoiSanXuat.Items.Add("Việt Nam");
            comboBoxNoiSanXuat.Items.Add("Trung Quốc");
            comboBoxNoiSanXuat.Items.Add("Hàn Quốc");
            comboBoxNoiSanXuat.Items.Add("Nhật Bản");
            comboBoxNoiSanXuat.Items.Add("Mỹ");

            // Đặt giá trị mặc định cho các ComboBox (tuỳ chọn)
            comboBoxLoai.SelectedIndex = 0;
            comboBoxCo.SelectedIndex = 0;
            comboBoxChatLieu.SelectedIndex = 0;
            comboBoxMau.SelectedIndex = 0;
            comboBoxDoiTuong.SelectedIndex = 0;
            comboBoxMua.SelectedIndex = 0;
            comboBoxNoiSanXuat.SelectedIndex = 0;
        }
        // Chuỗi kết nối cho MySQL
        private string connectionString = "server=192.168.0.101;database=ClothingStore;uid=root;pwd=binh11a10;";

        // Load dữ liệu vào DataGridView
        private void LoadDataGridView()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT sp.MaQuanAo, sp.TenQuanAo, tl.TenLoai, c.TenCo, cl.TenChatLieu, m.TenMau, dt.TenDoiTuong, mu.TenMua, nsx.TenNSX, sp.SoLuong, sp.Anh, sp.DonGiaNhap, sp.DonGiaBan " +
                               "FROM SanPham sp " +
                               "INNER JOIN TheLoai tl ON sp.MaLoai = tl.MaLoai " +
                               "INNER JOIN Co c ON sp.MaCo = c.MaCo " +
                               "INNER JOIN ChatLieu cl ON sp.MaChatLieu = cl.MaChatLieu " +
                               "INNER JOIN Mau m ON sp.MaMau = m.MaMau " +
                               "INNER JOIN DoiTuong dt ON sp.MaDoiTuong = dt.MaDoiTuong " +
                               "INNER JOIN Mua mu ON sp.MaMua = mu.MaMua " +
                               "INNER JOIN NoiSanXuat nsx ON sp.MaNSX = nsx.MaNSX";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridViewSanPham.DataSource = dt;
                dataGridViewSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewSanPham.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                // Gán hình ảnh cho từng dòng trong DataGridView
                foreach (DataGridViewRow row in dataGridViewSanPham.Rows)
                {
                    string imagePath = row.Cells["Anh"].Value?.ToString();
                }
            }
        }
        // Nút thêm
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTenQuanAo.Text == "" || 
                txtDonGiaNhap.Text == "" || 
                txtDonGiaBan.Text == "" || 
                txtSoLuong.Text == ""||
                comboBoxChatLieu.Text==null||
                comboBoxCo.Text==null||
                comboBoxDoiTuong.Text==null||
                comboBoxLoai.Text==null||
                comboBoxMau.Text==null||
                comboBoxMua.Text==null||
                comboBoxNoiSanXuat.Text==null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Lấy mã từ tên trong ComboBox
                    int maLoai = GetIdFromName("TheLoai", "MaLoai", "TenLoai", comboBoxLoai.SelectedItem.ToString(), conn);
                    int maCo = GetIdFromName("Co", "MaCo", "TenCo", comboBoxCo.SelectedItem.ToString(), conn);
                    int maChatLieu = GetIdFromName("ChatLieu", "MaChatLieu", "TenChatLieu", comboBoxChatLieu.SelectedItem.ToString(), conn);
                    int maMau = GetIdFromName("Mau", "MaMau", "TenMau", comboBoxMau.SelectedItem.ToString(), conn);
                    int maDoiTuong = GetIdFromName("DoiTuong", "MaDoiTuong", "TenDoiTuong", comboBoxDoiTuong.SelectedItem.ToString(), conn);
                    int maMua = GetIdFromName("Mua", "MaMua", "TenMua", comboBoxMua.SelectedItem.ToString(), conn);
                    int maNSX = GetIdFromName("NoiSanXuat", "MaNSX", "TenNSX", comboBoxNoiSanXuat.SelectedItem.ToString(), conn);

                    // Thêm sản phẩm vào bảng SanPham
                    string insertQuery = @"INSERT INTO SanPham (MaQuanAo,TenQuanAo, MaLoai, MaCo, MaChatLieu, MaMau, MaDoiTuong, MaMua, MaNSX, SoLuong, Anh, DonGiaNhap, DonGiaBan) 
                                   VALUES (@MaQuanAo,@TenQuanAo, @MaLoai, @MaCo, @MaChatLieu, @MaMau, @MaDoiTuong, @MaMua, @MaNSX, @SoLuong, @Anh, @DonGiaNhap, @DonGiaBan)";

                    MySqlCommand cmdInsert = new MySqlCommand(insertQuery, conn);
                    cmdInsert.Parameters.AddWithValue("@MaQuanAo", txtMaQuanAo.Text);
                    cmdInsert.Parameters.AddWithValue("@TenQuanAo", txtTenQuanAo.Text);
                    cmdInsert.Parameters.AddWithValue("@MaLoai", maLoai);
                    cmdInsert.Parameters.AddWithValue("@MaCo", maCo);
                    cmdInsert.Parameters.AddWithValue("@MaChatLieu", maChatLieu);
                    cmdInsert.Parameters.AddWithValue("@MaMau", maMau);
                    cmdInsert.Parameters.AddWithValue("@MaDoiTuong", maDoiTuong);
                    cmdInsert.Parameters.AddWithValue("@MaMua", maMua);
                    cmdInsert.Parameters.AddWithValue("@MaNSX", maNSX);
                    cmdInsert.Parameters.AddWithValue("@SoLuong", int.Parse(txtSoLuong.Text));
                    cmdInsert.Parameters.AddWithValue("@Anh", picAnh.ImageLocation ?? (object)DBNull.Value);
                    cmdInsert.Parameters.AddWithValue("@DonGiaNhap", decimal.Parse(txtDonGiaNhap.Text));
                    cmdInsert.Parameters.AddWithValue("@DonGiaBan", decimal.Parse(txtDonGiaBan.Text));

                    int rowsAffected = cmdInsert.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đã thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataGridView(); // Cập nhật lại danh sách sản phẩm
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi thêm sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private int GetIdFromName(string tableName, string idColumn, string nameColumn, string name, MySqlConnection conn)
        {
            string query = $"SELECT {idColumn} FROM {tableName} WHERE {nameColumn} = @name";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0; // Trả về 0 nếu không tìm thấy
            }
        }

        // Nút xóa
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewSanPham.SelectedRows.Count == 0)
            {
                MessageBox.Show("Hãy chọn 1 dòng để thao tác!");
                return;
            }

            int maQuanAo = Convert.ToInt32(dataGridViewSanPham.SelectedRows[0].Cells[0].Value);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string deleteQuery = "DELETE FROM SanPham WHERE MaQuanAo = @MaQuanAo";
                MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, conn);
                cmdDelete.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                conn.Open();

                int rowsAffected = cmdDelete.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Đã xóa sản phẩm thành công!");
                    LoadDataGridView();
                }
                else
                {
                    MessageBox.Show("Lỗi khi xóa sản phẩm!");
                }
            }
        }

        // Nút cập nhật
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaQuanAo.Text))
            {
                MessageBox.Show("Hãy chọn một dòng để thao tác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cập nhật giá trị ảnh mới từ textbox
            if (!string.IsNullOrEmpty(txtAnh.Text))
            {
                picAnh.ImageLocation = txtAnh.Text.Trim();
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string updateQuery = @"
            UPDATE SanPham 
            SET TenQuanAo = @TenQuanAo, 
                MaLoai = (SELECT MaLoai FROM TheLoai WHERE TenLoai = @TenLoai LIMIT 1), 
                MaCo = (SELECT MaCo FROM Co WHERE TenCo = @TenCo LIMIT 1), 
                MaChatLieu = (SELECT MaChatLieu FROM ChatLieu WHERE TenChatLieu = @TenChatLieu LIMIT 1), 
                MaMau = (SELECT MaMau FROM Mau WHERE TenMau = @TenMau LIMIT 1), 
                MaDoiTuong = (SELECT MaDoiTuong FROM DoiTuong WHERE TenDoiTuong = @TenDoiTuong LIMIT 1), 
                MaMua = (SELECT MaMua FROM Mua WHERE TenMua = @TenMua LIMIT 1), 
                MaNSX = (SELECT MaNSX FROM NoiSanXuat WHERE TenNSX = @TenNSX LIMIT 1), 
                SoLuong = @SoLuong, 
                DonGiaNhap = @DonGiaNhap, 
                DonGiaBan = @DonGiaBan, 
                Anh = @Anh
            WHERE MaQuanAo = @MaQuanAo";

                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);

                    // Kiểm tra null cho combobox
                    cmd.Parameters.AddWithValue("@TenQuanAo", txtTenQuanAo.Text);
                    cmd.Parameters.AddWithValue("@TenLoai", comboBoxLoai.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@TenCo", comboBoxCo.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@TenChatLieu", comboBoxChatLieu.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@TenMau", comboBoxMau.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@TenDoiTuong", comboBoxDoiTuong.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@TenMua", comboBoxMua.SelectedItem?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@TenNSX", comboBoxNoiSanXuat.SelectedItem?.ToString() ?? "");

                    // Kiểm tra giá trị số
                    if (!int.TryParse(txtSoLuong.Text, out int soLuong))
                    {
                        MessageBox.Show("Số lượng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);

                    if (!decimal.TryParse(txtDonGiaNhap.Text, out decimal donGiaNhap) || !decimal.TryParse(txtDonGiaBan.Text, out decimal donGiaBan))
                    {
                        MessageBox.Show("Đơn giá nhập/bán không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                    cmd.Parameters.AddWithValue("@DonGiaBan", donGiaBan);

                    // Kiểm tra đường dẫn ảnh trước khi cập nhật
                    string imagePath = string.IsNullOrWhiteSpace(txtAnh.Text) ? null : txtAnh.Text.Trim();
                    cmd.Parameters.AddWithValue("@Anh", imagePath ?? (object)DBNull.Value);

                    // Kiểm tra mã sản phẩm
                    if (!int.TryParse(txtMaQuanAo.Text, out int maQuanAo))
                    {
                        MessageBox.Show("Mã sản phẩm không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cmd.Parameters.AddWithValue("@MaQuanAo", maQuanAo);

                    // Thực thi lệnh SQL
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Đã cập nhật sản phẩm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGridView();  // Cập nhật lại bảng dữ liệu
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Nút quay lại
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide(); // Đóng form hiện tại
            Adimin formAdmin = new Adimin(); // Tạo mới form Admin
            formAdmin.Show(); // Hiển thị lại form Admin
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


        // Hiển thị dữ liệu lên textbox, combobox khi chọn dòng trong DataGridView
        private void dataGridViewSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)  // Kiểm tra có phải là một dòng hợp lệ
            {
                // Lấy thông tin từ dòng đã click
                DataGridViewRow row = dataGridViewSanPham.Rows[e.RowIndex];

                // Hiển thị giá trị từ DataGridView vào các TextBox
                txtMaQuanAo.Text = row.Cells["MaQuanAo"].Value.ToString();
                txtTenQuanAo.Text = row.Cells["TenQuanAo"].Value.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                txtDonGiaNhap.Text = row.Cells["DonGiaNhap"].Value.ToString();
                txtDonGiaBan.Text = row.Cells["DonGiaBan"].Value.ToString();
                // Gán giá trị cho ComboBox dựa trên dữ liệu từ DataGridView
                comboBoxLoai.SelectedItem = row.Cells["TenLoai"].Value.ToString();
                comboBoxCo.SelectedItem = row.Cells["TenCo"].Value.ToString();
                comboBoxChatLieu.SelectedItem = row.Cells["TenChatLieu"].Value.ToString();
                comboBoxMau.SelectedItem = row.Cells["TenMau"].Value.ToString();
                comboBoxDoiTuong.SelectedItem = row.Cells["TenDoiTuong"].Value.ToString();
                comboBoxMua.SelectedItem = row.Cells["TenMua"].Value.ToString();
                comboBoxNoiSanXuat.SelectedItem = row.Cells["TenNSX"].Value.ToString();
                string imagePath = row.Cells["Anh"].Value?.ToString();
                txtAnh.Text = imagePath; // Hiển thị đường dẫn trong TextBox
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Tạo đường dẫn đầy đủ (nếu cần)
                    string fullPath = Path.Combine(Application.StartupPath, imagePath.Replace("/","\\"));

                    // Kiểm tra file có tồn tại không
                    if (File.Exists(fullPath))
                    {
                        // Hiển thị ảnh trong PictureBox
                        picAnh.Image = Image.FromFile(fullPath);
                        picAnh.SizeMode = PictureBoxSizeMode.StretchImage; // Chỉnh kích thước ảnh cho phù hợp
                    }
                    else
                    {
                        MessageBox.Show("Ảnh không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        picAnh.Image = null; // Xóa ảnh nếu không tìm thấy
                    }
                }
                else
                {
                    picAnh.Image = null; // Xóa ảnh nếu không có đường dẫn
                }
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                txtAnh.Text = open.FileName;
                picAnh.ImageLocation = open.FileName;
                picAnh.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataGridView();
            comboBoxLoai.Text = null;
            comboBoxCo.Text = null;
            comboBoxChatLieu.Text = null;
            comboBoxMua.Text = null;
            comboBoxMau.Text = null;
            comboBoxDoiTuong.Text = null;
            comboBoxNoiSanXuat.Text = null;
            txtMaQuanAo.Clear();
            txtTenQuanAo.Clear ();
            txtSoLuong.Clear();
            txtDonGiaNhap.Clear();
            txtAnh.Clear();
            txtDonGiaBan.Clear();
            picAnh.Image=null;
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT s.MaQuanAo, s.TenQuanAo, t.TenLoai, c.TenCo, m.TenMau, mu.TenMua, dt.TenDoiTuong, cl.TenChatLieu, ns.TenNSX, s.Anh, s.DonGiaBan, s.SoLuong, s.DonGiaNhap, s.DonGiaBan " +
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

                    if (comboBoxLoai.SelectedItem != null)
                        query += " AND t.TenLoai = @TenLoai";

                    if (comboBoxCo.SelectedItem != null)
                        query += " AND c.TenCo = @TenCo";

                    if (comboBoxMau.SelectedItem != null)
                        query += " AND m.TenMau = @TenMau";

                    if (comboBoxMua.SelectedItem != null)
                        query += " AND mu.TenMua = @TenMua";

                    if (comboBoxDoiTuong.SelectedItem != null)
                        query += " AND dt.TenDoiTuong = @TenDoiTuong";

                    if (comboBoxChatLieu.SelectedItem != null)
                        query += " AND cl.TenChatLieu = @TenChatLieu";

                    if (comboBoxNoiSanXuat.SelectedItem != null)
                        query += " AND ns.TenNSX = @TenNSX";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(txtMaQuanAo.Text))
                        cmd.Parameters.AddWithValue("@MaQuanAo", txtMaQuanAo.Text);

                    if (comboBoxLoai.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@TenLoai", comboBoxLoai.SelectedItem.ToString());

                    if (comboBoxCo.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@TenCo", comboBoxCo.SelectedItem.ToString());

                    if (comboBoxMau.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@TenMau", comboBoxMau.SelectedItem.ToString());

                    if (comboBoxMua.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@TenMua", comboBoxMua.SelectedItem.ToString());

                    if (comboBoxDoiTuong.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@TenDoiTuong", comboBoxDoiTuong.SelectedItem.ToString());

                    if (comboBoxChatLieu.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@TenChatLieu", comboBoxChatLieu.SelectedItem.ToString());

                    if (comboBoxNoiSanXuat.SelectedItem != null)
                        cmd.Parameters.AddWithValue("@TenNSX", comboBoxNoiSanXuat.SelectedItem.ToString());

                    var adapter = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewSanPham.DataSource = dt;
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
        }
    }
}
