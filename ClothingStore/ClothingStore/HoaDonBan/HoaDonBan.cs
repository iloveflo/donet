using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClothingStore.HoaDonBan
{
    public partial class HoaDonBan : Form
    {
        private string connectionString = "server=localhost;database=ClothingStore;user=root;password=binh11a10;";
        public HoaDonBan()
        {
            InitializeComponent();
            LoadHoaDonBan();
        }
        private void LoadHoaDonBan()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        hdb.SoHoaDonBan, 
                        sp.MaQuanAo, 
                        sp.TenQuanAo, 
                        hdb.MaNhanVien, 
                        hdb.MaKhachHang, 
                        cthdb.SoLuong 
                    FROM HoaDonBan hdb
                    JOIN ChiTietHoaDonBan cthdb ON hdb.SoHoaDonBan = cthdb.SoHoaDonBan
                    JOIN SanPham sp ON cthdb.MaQuanAo = sp.MaQuanAo;";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
        }

        // Khi click vào một dòng trong DataGridView, hiển thị dữ liệu lên TextBox
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtSoHoaDonBan.Text = row.Cells["SoHoaDonBan"].Value.ToString();
                txtMaQuanAo.Text = row.Cells["MaQuanAo"].Value.ToString();
                txtTenQuanAo.Text = row.Cells["TenQuanAo"].Value.ToString();
                txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value.ToString();
                txtMaKhachHang.Text = row.Cells["MaKhachHang"].Value.ToString();
                txtSoLuongBan.Text = row.Cells["SoLuong"].Value.ToString();
            }
        }
        private void btnQuaylai_click(object sender, EventArgs e)
        {
            this.Hide();
            ThongKe.ThongKe1 thongKeForm1 = new ThongKe.ThongKe1();
            thongKeForm1.Show();
            this.Hide();
        }
    }
}

