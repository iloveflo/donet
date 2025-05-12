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

namespace ClothingStore.NhanVien
{
    public partial class Nhan_Vien : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        public Nhan_Vien()
        {
            InitializeComponent();
        }
        private void btnSanPham_Click(object sender, EventArgs e)
        {
            LoadControl(new SanPhamNV_UserControl());
        }
        private void LoadControl(UserControl uc)
        {
            panelMain.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelMain.Controls.Add(uc);
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
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            Main mainForm = new Main();
            mainForm.Show();
            this.Close();
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
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadControl(new ThongKe_UserControl());
        }
        private void btnHoaDonNhap_Click(object sender, EventArgs e)
        {
            LoadControl(new HoaDonNhap.HoaDonNhapNV_UserControl());
        }
        private void btnHoaDonBan_Click(object sender, EventArgs e)
        {
            LoadControl(new HoaDonBan.HoaDonBan_UserControl());
        }
        private void btnXemDonDatHang_Click(object sender, EventArgs e)
        {
            LoadControl(new XemDatHang_UserControl());
        }
    }
}
