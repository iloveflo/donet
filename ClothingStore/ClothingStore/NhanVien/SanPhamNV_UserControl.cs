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

namespace ClothingStore.NhanVien
{
    public partial class SanPhamNV_UserControl : UserControl
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        private MySqlConnection conn;
        private int maNhanVien = Convert.ToInt32(SessionManager.MaTaiKhoanDangNhap);
        public SanPhamNV_UserControl()
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
                var query = "SELECT s.MaQuanAo, s.TenQuanAo, t.TenLoai, c.TenCo, m.TenMau, mu.TenMua, dt.TenDoiTuong, cl.TenChatLieu, ns.TenNSX, s.Anh, s.DonGiaBan " +
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
                    // Check if the column 'Anh' exists and handle null values
                    if (dataGridView1.Columns.Contains("Anh") &&
                        dataGridView1.Rows[e.RowIndex].Cells["Anh"].Value != DBNull.Value)
                    {
                        string anh = dataGridView1.Rows[e.RowIndex].Cells["Anh"].Value.ToString();
                        pictureBox1.ImageLocation = anh;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy cập cột 'Anh': " + ex.Message);
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
        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadDataGridView();
            txtMaQuanAo.Clear();
            txtTenQuanAo.Clear();
            txtDonGiaBan.Clear();
            cboChatLieu.Text = null;
            cboCo.Text = null;
            cboDoiTuong.Text = null;
            cboLoai.Text = null;
            cboMau.Text = null;
            cboMua.Text = null;
            cboNoiSanXuat.Text = null;
        }
    }
}
