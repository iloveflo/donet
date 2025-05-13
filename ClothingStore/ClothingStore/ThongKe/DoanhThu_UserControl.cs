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
        private void LoadDoanhThuTheoThang(int thang, int nam)
        {
            try
            {
                string query = @"
                SELECT 
                    MONTH(hb.NgayBan) AS Thang, 
                    YEAR(hb.NgayBan) AS Nam,
                    COALESCE(SUM(ctb.SoLuong), 0) AS TongSoLuong, 
                    COALESCE(SUM(hb.TongTien), 0) AS TongDoanhThu,
                    COALESCE(SUM(hb.TongTien), 0) - 
                        COALESCE((
                            SELECT SUM(
                                ctb2.SoLuong * (
                                    SELECT AVG(COALESCE(ctn.DonGia * (1 - ctn.GiamGia/100), 0))
                                    FROM chitiethoadonnhap ctn
                                    WHERE ctn.MaQuanAo = ctb2.MaQuanAo
                                )
                            )
                            FROM chitiethoadonban ctb2
                            JOIN hoadonban hb2 ON ctb2.SoHoaDonBan = hb2.SoHoaDonBan
                            WHERE MONTH(hb2.NgayBan) = @Thang 
                            AND YEAR(hb2.NgayBan) = @Nam
                        ), 0) AS TongLoiNhuan
                FROM hoadonban hb
                LEFT JOIN chitiethoadonban ctb ON hb.SoHoaDonBan = ctb.SoHoaDonBan
                WHERE MONTH(hb.NgayBan) = @Thang AND YEAR(hb.NgayBan) = @Nam
                GROUP BY Thang, Nam;";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Thang", thang);
                        cmd.Parameters.AddWithValue("@Nam", nam);

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            // Định dạng hiển thị cho các cột số
                            if (dt.Rows.Count > 0)
                            {
                                // Thêm cột hiển thị tháng/năm
                                dt.Columns.Add("ThangNam", typeof(string));
                                foreach (DataRow row in dt.Rows)
                                {
                                    row["ThangNam"] = $"{row["Thang"]}/{row["Nam"]}";
                                }
                            }

                            dataGridViewDoanhThu.DataSource = dt;

                            // Ẩn cột Nam nếu không cần hiển thị
                            if (dataGridViewDoanhThu.Columns.Contains("Nam"))
                            {
                                dataGridViewDoanhThu.Columns["Nam"].Visible = false;
                            }

                            // Định dạng tiêu đề và hiển thị các cột
                            if (dataGridViewDoanhThu.Columns.Contains("Thang"))
                            {
                                dataGridViewDoanhThu.Columns["Thang"].HeaderText = "Tháng";
                                dataGridViewDoanhThu.Columns["Thang"].Visible = false; // Ẩn cột này và hiển thị ThangNam
                            }

                            if (dataGridViewDoanhThu.Columns.Contains("ThangNam"))
                            {
                                dataGridViewDoanhThu.Columns["ThangNam"].HeaderText = "Tháng/Năm";
                            }

                            if (dataGridViewDoanhThu.Columns.Contains("TongSoLuong"))
                            {
                                dataGridViewDoanhThu.Columns["TongSoLuong"].HeaderText = "Tổng số lượng";
                                dataGridViewDoanhThu.Columns["TongSoLuong"].DefaultCellStyle.Format = "N0";
                            }

                            if (dataGridViewDoanhThu.Columns.Contains("TongDoanhThu"))
                            {
                                dataGridViewDoanhThu.Columns["TongDoanhThu"].HeaderText = "Tổng doanh thu";
                                dataGridViewDoanhThu.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                            }

                            if (dataGridViewDoanhThu.Columns.Contains("TongLoiNhuan"))
                            {
                                dataGridViewDoanhThu.Columns["TongLoiNhuan"].HeaderText = "Tổng lợi nhuận";
                                dataGridViewDoanhThu.Columns["TongLoiNhuan"].DefaultCellStyle.Format = "N0";
                            }

                            // Tự động điều chỉnh kích thước cột
                            dataGridViewDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dataGridViewDoanhThu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu doanh thu: " + ex.Message,
                               "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBoxThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxThang.SelectedItem != null)
                {
                    int thang = Convert.ToInt32(comboBoxThang.SelectedItem);

                    // Lấy năm từ control lựa chọn năm
                    // Nếu bạn có control để chọn năm (như numericUpDownNam)
                    int nam = DateTime.Now.Year; // Mặc định là năm hiện tại


                    // Tải dữ liệu theo tháng và năm đã chọn
                    LoadDoanhThuTheoThang(thang, nam);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn tháng: " + ex.Message,
                               "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridViewDoanhThu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra nếu người dùng click vào header hoặc ô không hợp lệ
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow row = dataGridViewDoanhThu.Rows[e.RowIndex];

                // Kiểm tra xem row có dữ liệu không
                if (row.Cells["Thang"].Value == null)
                    return;

                // Lấy giá trị tháng và xử lý cho comboBox
                string thang = row.Cells["Thang"].Value.ToString();


                if (comboBoxThang.Items.Contains(thang))
                {
                    comboBoxThang.SelectedItem = thang;
                }

                // Lấy và định dạng giá trị số lượng
                if (row.Cells["TongSoLuong"].Value != null)
                {
                    int tongSoLuong = Convert.ToInt32(row.Cells["TongSoLuong"].Value);
                    textBoxTongSoLuong.Text = tongSoLuong.ToString("N0");
                }
                else
                {
                    textBoxTongSoLuong.Text = "0";
                }

                // Lấy và định dạng giá trị doanh thu
                if (row.Cells["TongDoanhThu"].Value != null)
                {
                    decimal tongDoanhThu = Convert.ToDecimal(row.Cells["TongDoanhThu"].Value);
                    textBoxTongDoanhThu.Text = tongDoanhThu.ToString("N0");
                }
                else
                {
                    textBoxTongDoanhThu.Text = "0";
                }

                // Lấy và định dạng giá trị lợi nhuận
                if (row.Cells["TongLoiNhuan"].Value != null)
                {
                    decimal tongLoiNhuan = Convert.ToDecimal(row.Cells["TongLoiNhuan"].Value);
                    textBoxTongLoiNhuan.Text = tongLoiNhuan.ToString("N0");
                }
                else
                {
                    textBoxTongLoiNhuan.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị dữ liệu chi tiết: " + ex.Message,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DoanhThu1_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 12; i++)
            {
                comboBoxThang.Items.Add(i);
                HienThiDoanhThuTheoThang();
            }
        }
        public void HienThiDoanhThuTheoThang()
        {
            try
            {
                // Tạo DataTable để lưu kết quả
                DataTable dtDoanhThu = new DataTable();
                dtDoanhThu.Columns.Add("Thang", typeof(string));
                dtDoanhThu.Columns.Add("TongSoLuong", typeof(int));
                dtDoanhThu.Columns.Add("TongDoanhThu", typeof(decimal));
                dtDoanhThu.Columns.Add("TongLoiNhuan", typeof(decimal));

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Lấy danh sách các tháng có hóa đơn bán
                    string queryGetMonths = @"
                SELECT DISTINCT YEAR(NgayBan) AS Nam, MONTH(NgayBan) AS Thang, 
                                CONCAT(YEAR(NgayBan), '-', LPAD(MONTH(NgayBan), 2, '0')) AS ThangNam
                FROM hoadonban
                ORDER BY Nam, Thang";

                    MySqlCommand cmdGetMonths = new MySqlCommand(queryGetMonths, connection);
                    DataTable dtMonths = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmdGetMonths))
                    {
                        adapter.Fill(dtMonths);
                    }

                    // Duyệt qua từng tháng để tính toán
                    foreach (DataRow monthRow in dtMonths.Rows)
                    {
                        string thangNam = monthRow["ThangNam"].ToString();
                        int nam = Convert.ToInt32(monthRow["Nam"]);
                        int thangSo = Convert.ToInt32(monthRow["Thang"]);

                        // 1. Tính tổng số lượng bán trong tháng
                        string querySoLuong = @"
                    SELECT SUM(ctb.SoLuong) AS TongSoLuong
                    FROM chitiethoadonban ctb
                    JOIN hoadonban hdb ON ctb.SoHoaDonBan = hdb.SoHoaDonBan
                    WHERE YEAR(hdb.NgayBan) = @Nam AND MONTH(hdb.NgayBan) = @Thang";

                        MySqlCommand cmdSoLuong = new MySqlCommand(querySoLuong, connection);
                        cmdSoLuong.Parameters.AddWithValue("@Nam", nam);
                        cmdSoLuong.Parameters.AddWithValue("@Thang", thangSo);

                        object soLuongResult = cmdSoLuong.ExecuteScalar();
                        int tongSoLuong = soLuongResult == DBNull.Value ? 0 : Convert.ToInt32(soLuongResult);

                        // 2. Tính tổng doanh thu trong tháng
                        string queryDoanhThu = @"
                    SELECT SUM(TongTien) AS TongDoanhThu
                    FROM hoadonban
                    WHERE YEAR(NgayBan) = @Nam AND MONTH(NgayBan) = @Thang";

                        MySqlCommand cmdDoanhThu = new MySqlCommand(queryDoanhThu, connection);
                        cmdDoanhThu.Parameters.AddWithValue("@Nam", nam);
                        cmdDoanhThu.Parameters.AddWithValue("@Thang", thangSo);

                        object doanhThuResult = cmdDoanhThu.ExecuteScalar();
                        decimal tongDoanhThu = doanhThuResult == DBNull.Value ? 0 : Convert.ToDecimal(doanhThuResult);

                        // 3. Tính chi phí nhập hàng cho sản phẩm bán trong tháng (tính lợi nhuận)
                        // Đây là truy vấn phức tạp hơn, cần tìm ra chi phí nhập của các sản phẩm đã bán trong tháng
                        string queryLoiNhuan = @"
                    SELECT 
                        (SELECT SUM(TongTien) FROM hoadonban 
                         WHERE YEAR(NgayBan) = @Nam AND MONTH(NgayBan) = @Thang) -
                        (SELECT SUM(
                            (SELECT AVG(ctn.DonGia * (1 - ctn.GiamGia / 100))
                             FROM chitiethoadonnhap ctn
                             WHERE ctn.MaQuanAo = ctb.MaQuanAo) * ctb.SoLuong
                        )
                        FROM chitiethoadonban ctb
                        JOIN hoadonban hdb ON ctb.SoHoaDonBan = hdb.SoHoaDonBan
                        WHERE YEAR(hdb.NgayBan) = @Nam AND MONTH(hdb.NgayBan) = @Thang) 
                    AS TongLoiNhuan";

                        MySqlCommand cmdLoiNhuan = new MySqlCommand(queryLoiNhuan, connection);
                        cmdLoiNhuan.Parameters.AddWithValue("@Nam", nam);
                        cmdLoiNhuan.Parameters.AddWithValue("@Thang", thangSo);

                        object loiNhuanResult = cmdLoiNhuan.ExecuteScalar();
                        decimal tongLoiNhuan = loiNhuanResult == DBNull.Value ? 0 : Convert.ToDecimal(loiNhuanResult);

                        // Thêm dữ liệu vào DataTable
                        DataRow newRow = dtDoanhThu.NewRow();
                        newRow["Thang"] = string.Format("{0}/{1}", thangSo, nam);
                        newRow["TongSoLuong"] = tongSoLuong;
                        newRow["TongDoanhThu"] = tongDoanhThu;
                        newRow["TongLoiNhuan"] = tongLoiNhuan;
                        dtDoanhThu.Rows.Add(newRow);
                    }
                }

                // Hiển thị dữ liệu lên DataGridView
                dataGridViewDoanhThu.DataSource = dtDoanhThu;

                // Định dạng các cột số tiền
                dataGridViewDoanhThu.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";
                dataGridViewDoanhThu.Columns["TongLoiNhuan"].DefaultCellStyle.Format = "N0";

                // Đặt tên hiển thị cho các cột
                dataGridViewDoanhThu.Columns["Thang"].HeaderText = "Tháng";
                dataGridViewDoanhThu.Columns["TongSoLuong"].HeaderText = "Tổng số lượng";
                dataGridViewDoanhThu.Columns["TongDoanhThu"].HeaderText = "Tổng doanh thu";
                dataGridViewDoanhThu.Columns["TongLoiNhuan"].HeaderText = "Tổng lợi nhuận";
                dataGridViewDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewDoanhThu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
