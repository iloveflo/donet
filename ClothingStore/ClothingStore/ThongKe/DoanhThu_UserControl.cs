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
    public partial class DoanhThu_UserControl : UserControl
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        public DoanhThu_UserControl()
        {
            InitializeComponent();
        }
        private void LoadDoanhThuTheoThang(int thang)
        {
            string query = @"
        SELECT 
            MONTH(hb.NgayBan) AS Thang, 
            COALESCE(SUM(ctb.SoLuong), 0) AS TongSoLuongBan, 
            COALESCE(SUM(ctb.ThanhTien), 0) AS TongDoanhThu,
            COALESCE(SUM(ctb.ThanhTien), 0) - COALESCE(
                (SELECT SUM(ctn.ThanhTien) 
                 FROM chitiethoadonnhap ctn 
                 JOIN hoadonnhap hn ON ctn.SoHoaDonNhap = hn.SoHoaDonNhap
                 WHERE MONTH(hn.NgayNhap) = @Thang), 0) 
            AS TongLoiNhuan
        FROM hoadonban hb
        LEFT JOIN chitiethoadonban ctb ON hb.SoHoaDonBan = ctb.SoHoaDonBan
        WHERE MONTH(hb.NgayBan) = @Thang
        GROUP BY Thang;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridViewDoanhThu.DataSource = dt;
                        dataGridViewDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridViewDoanhThu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    }
                }
            }
        }
        private void comboBoxThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxThang.SelectedItem != null)
            {
                int thang = Convert.ToInt32(comboBoxThang.SelectedItem);
                LoadDoanhThuTheoThang(thang);
            }
        }
        private void dataGridViewDoanhThu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewDoanhThu.Rows[e.RowIndex];
                comboBoxThang.SelectedItem = row.Cells["Thang"].Value.ToString();
                textBoxTongSoLuong.Text = row.Cells["TongSoLuongBan"].Value.ToString();
                textBoxTongDoanhThu.Text = row.Cells["TongDoanhThu"].Value.ToString();
                textBoxTongLoiNhuan.Text = row.Cells["TongLoiNhuan"].Value.ToString();
            }
        }
        private void DoanhThu1_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 12; i++)
            {
                comboBoxThang.Items.Add(i);
            }
            LoadDoanhThuTheoThang(DateTime.Now.Month); // Mặc định lấy tháng hiện tại
        }
    }
}
