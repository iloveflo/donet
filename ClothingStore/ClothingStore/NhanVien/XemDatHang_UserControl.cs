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
    public partial class XemDatHang_UserControl : UserControl
    {
        private string connectionString = DatabaseHelper.ConnectionString;
        private MySqlConnection conn;
        private int maNhanVien = Convert.ToInt32(SessionManager.MaTaiKhoanDangNhap);
        public XemDatHang_UserControl()
        {
            InitializeComponent();
            conn = new MySqlConnection(connectionString);
            LoadDonDatHangVaoDataGridView();
        }
        private void LoadDonDatHangVaoDataGridView()
        {
            try
            {
                conn.Open();
                string query = "SELECT g.MaKhachHang, g.MaQuanAo, s.TenQuanAo, g.DonGiaBan, g.SoLuongDat, g.TongTien " +
                               "FROM GioHang g " +
                               "JOIN SanPham s ON g.MaQuanAo = s.MaQuanAo";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem Đơn Đặt hàng: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void btnLapHoaDonBan_Click(object sender, EventArgs e)
        {

            // Kiểm tra xem DataGridView có đang hiển thị bảng GioHang không
            if (dataGridView1.DataSource == null || dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Không có sản phẩm trong giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem có dòng nào được chọn không
            int selectedRowIndex = dataGridView1.CurrentCell?.RowIndex ?? -1;
            if (selectedRowIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để lập hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Lấy dữ liệu từ dòng đang được chọn
            DataGridViewRow row = dataGridView1.Rows[selectedRowIndex];
            int maQuanAo = Convert.ToInt32(row.Cells["MaQuanAo"].Value);
            int soLuongDat = Convert.ToInt32(row.Cells["SoLuongDat"].Value);
            decimal donGia = Convert.ToDecimal(row.Cells["DonGiaBan"].Value);
            string maKhachHang = Convert.ToString(row.Cells["MaKhachHang"].Value);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction(); // Bắt đầu transaction để tránh lỗi dữ liệu

                try
                {
                    // Lấy số lượng hiện tại của sản phẩm
                    string queryGetSoLuong = "SELECT SoLuong FROM SanPham WHERE MaQuanAo = @MaQuanAo";
                    MySqlCommand cmdGetSoLuong = new MySqlCommand(queryGetSoLuong, conn);
                    cmdGetSoLuong.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                    int soLuongHienTai = Convert.ToInt32(cmdGetSoLuong.ExecuteScalar());

                    // Kiểm tra nếu số lượng trong kho đủ để bán
                    if (soLuongHienTai < soLuongDat)
                    {
                        MessageBox.Show("Không đủ số lượng sản phẩm trong kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        transaction.Rollback();
                        return;
                    }

                    // Trừ số lượng sản phẩm trong bảng SanPham
                    string queryUpdateSanPham = "UPDATE SanPham SET SoLuong = SoLuong - @SoLuongDat WHERE MaQuanAo = @MaQuanAo";
                    MySqlCommand cmdUpdateSanPham = new MySqlCommand(queryUpdateSanPham, conn);
                    cmdUpdateSanPham.Parameters.AddWithValue("@SoLuongDat", soLuongDat);
                    cmdUpdateSanPham.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                    cmdUpdateSanPham.ExecuteNonQuery();

                    // Thêm hóa đơn bán mới vào bảng HoaDonBan
                    string queryInsertHoaDon = "INSERT INTO HoaDonBan (MaKhachHang, NgayBan, MaNhanVien, TongTien) " +
                           "VALUES (@MaKhachHang, NOW(), @MaNhanVien, @TongTien); " +
                           "SELECT LAST_INSERT_ID();";

                    decimal tongTien = soLuongDat * donGia;
                    MySqlCommand cmdInsertHoaDon = new MySqlCommand(queryInsertHoaDon, conn);

                    // Thêm tham số vào câu lệnh
                    cmdInsertHoaDon.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    cmdInsertHoaDon.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                    cmdInsertHoaDon.Parameters.AddWithValue("@TongTien", tongTien);

                    // Thực thi câu lệnh và lấy ID của hóa đơn mới
                    int maHoaDon = Convert.ToInt32(cmdInsertHoaDon.ExecuteScalar());

                    // Bạn có thể sử dụng maHoaDon để lưu các chi tiết hóa đơn sau này vào bảng ChiTietHoaDonBan

                    // Chọn giảm giá ngẫu nhiên từ 1-50
                    Random rand = new Random();
                    int giamGia = rand.Next(1, 51); // Giá trị từ 1 đến 50%

                    // Tính tổng tiền và thành tiền
                    decimal thanhTien = tongTien * (1 - giamGia / 100.0m);

                    // Thêm chi tiết hóa đơn vào bảng ChiTietHoaDonBan
                    string queryInsertChiTiet = "INSERT INTO ChiTietHoaDonBan (SoHoaDonBan, MaQuanAo, SoLuong, GiamGia, ThanhTien) " +
                            "VALUES (@MaHoaDon, @MaQuanAo, @SoLuong, @GiamGia, @ThanhTien)";

                    using (MySqlCommand cmdInsertChiTiet = new MySqlCommand(queryInsertChiTiet, conn))
                    {
                        // Thêm tham số vào câu lệnh
                        cmdInsertChiTiet.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                        cmdInsertChiTiet.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                        cmdInsertChiTiet.Parameters.AddWithValue("@SoLuong", soLuongDat);
                        cmdInsertChiTiet.Parameters.AddWithValue("@GiamGia", giamGia); // Giảm giá đã tính ngẫu nhiên (1-50)
                        cmdInsertChiTiet.Parameters.AddWithValue("@ThanhTien", thanhTien); // ThanhTien đã tính

                        // Thực thi câu lệnh insert chi tiết hóa đơn vào bảng ChiTietHoaDonBan
                        cmdInsertChiTiet.ExecuteNonQuery();
                    }

                    // Xóa dòng vừa chọn khỏi bảng GioHang
                    string queryDeleteGioHang = "DELETE FROM GioHang WHERE MaQuanAo = @MaQuanAo AND MaKhachHang = @MaKhachHang";
                    MySqlCommand cmdDeleteGioHang = new MySqlCommand(queryDeleteGioHang, conn);
                    cmdDeleteGioHang.Parameters.AddWithValue("@MaQuanAo", maQuanAo);
                    cmdDeleteGioHang.Parameters.AddWithValue("@MaKhachHang", maKhachHang);
                    cmdDeleteGioHang.ExecuteNonQuery();

                    // Commit transaction
                    transaction.Commit();

                    MessageBox.Show("Hóa đơn bán đã được lập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Cập nhật lại DataGridView GioHang
                    LoadDonDatHangVaoDataGridView();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
