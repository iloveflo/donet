using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.Class
{
    public static class SessionManager
    {
        public static string MaTaiKhoanDangNhap { get; private set; } = "";
        public static string LoaiTaiKhoanDangNhap { get; private set; } = "";

        public static void SetSession(string maTaiKhoan, string loaiTaiKhoan)
        {
            MaTaiKhoanDangNhap = maTaiKhoan;
            LoaiTaiKhoanDangNhap = loaiTaiKhoan;
        }

        public static void ClearSession()
        {
            MaTaiKhoanDangNhap = "";
            LoaiTaiKhoanDangNhap = "";
        }
    }
}
