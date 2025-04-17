-- MySQL dump 10.13  Distrib 9.2.0, for Win64 (x86_64)
--
-- Host: localhost    Database: ClothingStore
-- ------------------------------------------------------
-- Server version	9.2.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `capcha`
--

DROP TABLE IF EXISTS `capcha`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `capcha` (
  `MaAnh` int NOT NULL AUTO_INCREMENT,
  `LinkAnh` varchar(255) NOT NULL,
  `KetQua` varchar(10) NOT NULL,
  PRIMARY KEY (`MaAnh`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `capcha`
--

LOCK TABLES `capcha` WRITE;
/*!40000 ALTER TABLE `capcha` DISABLE KEYS */;
INSERT INTO `capcha` VALUES (1,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/2b827.png','2b827'),(2,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/4c8n8.png','4c8n8'),(3,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/6pfy4.png','6pfy4'),(4,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/22d5n.png','22d5n'),(5,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/8n4n8.png','8n4n8'),(6,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/3pe4g.png\"','3pe4g'),(7,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/46mbm.png','46mbm'),(8,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/56ncx.png','56ncx'),(9,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/5dxnm.png','5dxnm'),(10,'D:/ProjectC#/ClothingStore/ClothingStore/images/samples/8bbw8.png\"','8bbw8');
/*!40000 ALTER TABLE `capcha` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `chatlieu`
--

DROP TABLE IF EXISTS `chatlieu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chatlieu` (
  `MaChatLieu` int NOT NULL AUTO_INCREMENT,
  `TenChatLieu` varchar(50) NOT NULL,
  PRIMARY KEY (`MaChatLieu`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chatlieu`
--

LOCK TABLES `chatlieu` WRITE;
/*!40000 ALTER TABLE `chatlieu` DISABLE KEYS */;
INSERT INTO `chatlieu` VALUES (1,'Cotton'),(2,'Jeans'),(3,'Polyester'),(4,'Lụa'),(5,'Len');
/*!40000 ALTER TABLE `chatlieu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `chitiethoadonban`
--

DROP TABLE IF EXISTS `chitiethoadonban`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chitiethoadonban` (
  `SoHoaDonBan` int NOT NULL,
  `MaQuanAo` int NOT NULL,
  `SoLuong` int NOT NULL,
  `GiamGia` decimal(5,2) DEFAULT '0.00',
  `ThanhTien` decimal(12,2) NOT NULL,
  PRIMARY KEY (`SoHoaDonBan`,`MaQuanAo`),
  KEY `MaQuanAo` (`MaQuanAo`),
  CONSTRAINT `chitiethoadonban_ibfk_1` FOREIGN KEY (`SoHoaDonBan`) REFERENCES `hoadonban` (`SoHoaDonBan`),
  CONSTRAINT `chitiethoadonban_ibfk_2` FOREIGN KEY (`MaQuanAo`) REFERENCES `sanpham` (`MaQuanAo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chitiethoadonban`
--

LOCK TABLES `chitiethoadonban` WRITE;
/*!40000 ALTER TABLE `chitiethoadonban` DISABLE KEYS */;
INSERT INTO `chitiethoadonban` VALUES (1,3,6,49.00,1224000.00);
/*!40000 ALTER TABLE `chitiethoadonban` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `chitiethoadonnhap`
--

DROP TABLE IF EXISTS `chitiethoadonnhap`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chitiethoadonnhap` (
  `SoHoaDonNhap` int NOT NULL,
  `MaQuanAo` int NOT NULL,
  `SoLuong` int NOT NULL,
  `DonGia` decimal(10,2) NOT NULL,
  `GiamGia` decimal(5,2) DEFAULT '0.00',
  `ThanhTien` decimal(12,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`SoHoaDonNhap`,`MaQuanAo`),
  KEY `MaQuanAo` (`MaQuanAo`),
  CONSTRAINT `chitiethoadonnhap_ibfk_1` FOREIGN KEY (`SoHoaDonNhap`) REFERENCES `hoadonnhap` (`SoHoaDonNhap`),
  CONSTRAINT `chitiethoadonnhap_ibfk_2` FOREIGN KEY (`MaQuanAo`) REFERENCES `sanpham` (`MaQuanAo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chitiethoadonnhap`
--

LOCK TABLES `chitiethoadonnhap` WRITE;
/*!40000 ALTER TABLE `chitiethoadonnhap` DISABLE KEYS */;
INSERT INTO `chitiethoadonnhap` VALUES (5,5,20,50000.00,15.00,0.00),(6,7,20,5000.00,15.00,0.00),(7,3,10,40000.00,15.00,0.00),(8,3,12,45000.00,5.00,0.00),(9,5,12,55000.00,15.00,0.00),(10,5,10,45000.00,15.00,382500.00);
/*!40000 ALTER TABLE `chitiethoadonnhap` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `co`
--

DROP TABLE IF EXISTS `co`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `co` (
  `MaCo` int NOT NULL AUTO_INCREMENT,
  `TenCo` varchar(10) NOT NULL,
  PRIMARY KEY (`MaCo`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `co`
--

LOCK TABLES `co` WRITE;
/*!40000 ALTER TABLE `co` DISABLE KEYS */;
INSERT INTO `co` VALUES (1,'S'),(2,'M'),(3,'L'),(4,'XL'),(5,'XXL');
/*!40000 ALTER TABLE `co` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `congviec`
--

DROP TABLE IF EXISTS `congviec`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `congviec` (
  `MaCongViec` int NOT NULL AUTO_INCREMENT,
  `TenCongViec` varchar(100) NOT NULL,
  PRIMARY KEY (`MaCongViec`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `congviec`
--

LOCK TABLES `congviec` WRITE;
/*!40000 ALTER TABLE `congviec` DISABLE KEYS */;
INSERT INTO `congviec` VALUES (1,'Nhân viên bán hàng'),(2,'Nhân viên thu ngân');
/*!40000 ALTER TABLE `congviec` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doituong`
--

DROP TABLE IF EXISTS `doituong`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doituong` (
  `MaDoiTuong` int NOT NULL AUTO_INCREMENT,
  `TenDoiTuong` varchar(50) NOT NULL,
  PRIMARY KEY (`MaDoiTuong`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doituong`
--

LOCK TABLES `doituong` WRITE;
/*!40000 ALTER TABLE `doituong` DISABLE KEYS */;
INSERT INTO `doituong` VALUES (1,'Nam'),(2,'Nữ'),(3,'Trẻ em'),(4,'Unisex'),(5,'Người già');
/*!40000 ALTER TABLE `doituong` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `giohang`
--

DROP TABLE IF EXISTS `giohang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `giohang` (
  `MaKhachHang` varchar(20) NOT NULL,
  `MaQuanAo` int NOT NULL,
  `DonGiaBan` decimal(10,2) NOT NULL,
  `SoLuongDat` int NOT NULL,
  `TongTien` decimal(10,2) GENERATED ALWAYS AS ((`DonGiaBan` * `SoLuongDat`)) STORED,
  PRIMARY KEY (`MaKhachHang`,`MaQuanAo`),
  KEY `MaQuanAo` (`MaQuanAo`),
  CONSTRAINT `giohang_ibfk_1` FOREIGN KEY (`MaKhachHang`) REFERENCES `khachhang` (`MaKhachHang`),
  CONSTRAINT `giohang_ibfk_2` FOREIGN KEY (`MaQuanAo`) REFERENCES `sanpham` (`MaQuanAo`) ON DELETE CASCADE,
  CONSTRAINT `giohang_chk_1` CHECK ((`SoLuongDat` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `giohang`
--

LOCK TABLES `giohang` WRITE;
/*!40000 ALTER TABLE `giohang` DISABLE KEYS */;
/*!40000 ALTER TABLE `giohang` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hoadonban`
--

DROP TABLE IF EXISTS `hoadonban`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hoadonban` (
  `SoHoaDonBan` int NOT NULL AUTO_INCREMENT,
  `MaNhanVien` int DEFAULT NULL,
  `NgayBan` date NOT NULL,
  `MaKhachHang` varchar(20) DEFAULT NULL,
  `TongTien` decimal(12,2) NOT NULL,
  PRIMARY KEY (`SoHoaDonBan`),
  KEY `MaNhanVien` (`MaNhanVien`),
  KEY `hoadonban_ibfk_2` (`MaKhachHang`),
  CONSTRAINT `hoadonban_ibfk_1` FOREIGN KEY (`MaNhanVien`) REFERENCES `nhanvien` (`MaNhanVien`),
  CONSTRAINT `hoadonban_ibfk_2` FOREIGN KEY (`MaKhachHang`) REFERENCES `khachhang` (`MaKhachHang`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hoadonban`
--

LOCK TABLES `hoadonban` WRITE;
/*!40000 ALTER TABLE `hoadonban` DISABLE KEYS */;
INSERT INTO `hoadonban` VALUES (1,NULL,'2025-03-18',NULL,2400000.00);
/*!40000 ALTER TABLE `hoadonban` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hoadonnhap`
--

DROP TABLE IF EXISTS `hoadonnhap`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hoadonnhap` (
  `SoHoaDonNhap` int NOT NULL AUTO_INCREMENT,
  `MaNhanVien` int DEFAULT NULL,
  `NgayNhap` date NOT NULL,
  `MaNCC` int DEFAULT NULL,
  `TongTien` decimal(12,2) NOT NULL,
  PRIMARY KEY (`SoHoaDonNhap`),
  KEY `MaNhanVien` (`MaNhanVien`),
  KEY `MaNCC` (`MaNCC`),
  CONSTRAINT `hoadonnhap_ibfk_1` FOREIGN KEY (`MaNhanVien`) REFERENCES `nhanvien` (`MaNhanVien`),
  CONSTRAINT `hoadonnhap_ibfk_2` FOREIGN KEY (`MaNCC`) REFERENCES `nhacungcap` (`MaNCC`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hoadonnhap`
--

LOCK TABLES `hoadonnhap` WRITE;
/*!40000 ALTER TABLE `hoadonnhap` DISABLE KEYS */;
INSERT INTO `hoadonnhap` VALUES (2,NULL,'2025-03-16',1,255000.00),(3,NULL,'2025-03-16',1,170000.00),(4,NULL,'2025-03-16',2,25000.00),(5,NULL,'2025-03-16',1,1000000.00),(6,NULL,'2025-03-16',3,100000.00),(7,NULL,'2025-03-16',3,400000.00),(8,NULL,'2025-03-16',4,540000.00),(9,NULL,'2025-03-16',5,660000.00),(10,NULL,'2025-03-16',5,450000.00);
/*!40000 ALTER TABLE `hoadonnhap` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `khachhang`
--

DROP TABLE IF EXISTS `khachhang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `khachhang` (
  `MaKhachHang` varchar(20) NOT NULL,
  `TenKhach` varchar(100) NOT NULL,
  `DiaChi` varchar(255) NOT NULL,
  `SoDienThoai` varchar(15) NOT NULL,
  `MaTaiKhoan` varchar(20) NOT NULL,
  PRIMARY KEY (`MaKhachHang`),
  KEY `FK_KhachHang_TaiKhoan` (`MaTaiKhoan`),
  CONSTRAINT `FK_KhachHang_TaiKhoan` FOREIGN KEY (`MaTaiKhoan`) REFERENCES `taikhoan` (`MaTaiKhoan`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `khachhang`
--

LOCK TABLES `khachhang` WRITE;
/*!40000 ALTER TABLE `khachhang` DISABLE KEYS */;
INSERT INTO `khachhang` VALUES ('2','sfjbheskf','tb','0000009','2'),('5','jshfgksdsd','dgjhdjk','9847239434','5');
/*!40000 ALTER TABLE `khachhang` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mau`
--

DROP TABLE IF EXISTS `mau`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mau` (
  `MaMau` int NOT NULL AUTO_INCREMENT,
  `TenMau` varchar(30) NOT NULL,
  PRIMARY KEY (`MaMau`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mau`
--

LOCK TABLES `mau` WRITE;
/*!40000 ALTER TABLE `mau` DISABLE KEYS */;
INSERT INTO `mau` VALUES (1,'Đỏ'),(2,'Xanh'),(3,'Vàng'),(4,'Trắng'),(5,'Đen');
/*!40000 ALTER TABLE `mau` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mua`
--

DROP TABLE IF EXISTS `mua`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mua` (
  `MaMua` int NOT NULL AUTO_INCREMENT,
  `TenMua` varchar(50) NOT NULL,
  PRIMARY KEY (`MaMua`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mua`
--

LOCK TABLES `mua` WRITE;
/*!40000 ALTER TABLE `mua` DISABLE KEYS */;
INSERT INTO `mua` VALUES (1,'Xuân'),(2,'Hạ'),(3,'Thu'),(4,'Đông'),(5,'Quanh năm');
/*!40000 ALTER TABLE `mua` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nhacungcap`
--

DROP TABLE IF EXISTS `nhacungcap`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nhacungcap` (
  `MaNCC` int NOT NULL AUTO_INCREMENT,
  `TenNCC` varchar(100) NOT NULL,
  `DiaChi` varchar(255) NOT NULL,
  `SoDienThoai` varchar(15) NOT NULL,
  PRIMARY KEY (`MaNCC`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nhacungcap`
--

LOCK TABLES `nhacungcap` WRITE;
/*!40000 ALTER TABLE `nhacungcap` DISABLE KEYS */;
INSERT INTO `nhacungcap` VALUES (1,'Công ty A','Hà Nội','0123456789'),(2,'Công ty B','TP.HCM','0987654321'),(3,'Công ty C','Đà Nẵng','0234567890'),(4,'Công ty D','Hải Phòng','0345678901'),(5,'Công ty E','Cần Thơ','0456789012');
/*!40000 ALTER TABLE `nhacungcap` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nhanvien`
--

DROP TABLE IF EXISTS `nhanvien`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nhanvien` (
  `MaNhanVien` int NOT NULL AUTO_INCREMENT,
  `TenNhanVien` varchar(100) NOT NULL,
  `GioiTinh` enum('Nam','Nữ','Khác') NOT NULL,
  `NgaySinh` date NOT NULL,
  `SoDienThoai` varchar(15) NOT NULL,
  `DiaChi` varchar(255) NOT NULL,
  `MaCongViec` int DEFAULT NULL,
  PRIMARY KEY (`MaNhanVien`),
  KEY `MaCongViec` (`MaCongViec`),
  CONSTRAINT `nhanvien_ibfk_1` FOREIGN KEY (`MaCongViec`) REFERENCES `congviec` (`MaCongViec`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nhanvien`
--

LOCK TABLES `nhanvien` WRITE;
/*!40000 ALTER TABLE `nhanvien` DISABLE KEYS */;
/*!40000 ALTER TABLE `nhanvien` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `noisanxuat`
--

DROP TABLE IF EXISTS `noisanxuat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `noisanxuat` (
  `MaNSX` int NOT NULL AUTO_INCREMENT,
  `TenNSX` varchar(100) NOT NULL,
  PRIMARY KEY (`MaNSX`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `noisanxuat`
--

LOCK TABLES `noisanxuat` WRITE;
/*!40000 ALTER TABLE `noisanxuat` DISABLE KEYS */;
INSERT INTO `noisanxuat` VALUES (1,'Việt Nam'),(2,'Trung Quốc'),(3,'Hàn Quốc'),(4,'Nhật Bản'),(5,'Mỹ');
/*!40000 ALTER TABLE `noisanxuat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sanpham`
--

DROP TABLE IF EXISTS `sanpham`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sanpham` (
  `MaQuanAo` int NOT NULL AUTO_INCREMENT,
  `TenQuanAo` varchar(100) NOT NULL,
  `MaLoai` int DEFAULT NULL,
  `MaCo` int DEFAULT NULL,
  `MaChatLieu` int DEFAULT NULL,
  `MaMau` int DEFAULT NULL,
  `MaDoiTuong` int DEFAULT NULL,
  `MaMua` int DEFAULT NULL,
  `MaNSX` int DEFAULT NULL,
  `SoLuong` int NOT NULL,
  `Anh` varchar(255) DEFAULT NULL,
  `DonGiaNhap` decimal(10,2) NOT NULL,
  `DonGiaBan` decimal(10,2) NOT NULL,
  PRIMARY KEY (`MaQuanAo`),
  KEY `MaLoai` (`MaLoai`),
  KEY `MaCo` (`MaCo`),
  KEY `MaChatLieu` (`MaChatLieu`),
  KEY `MaMau` (`MaMau`),
  KEY `MaDoiTuong` (`MaDoiTuong`),
  KEY `MaMua` (`MaMua`),
  KEY `MaNSX` (`MaNSX`),
  CONSTRAINT `sanpham_ibfk_1` FOREIGN KEY (`MaLoai`) REFERENCES `theloai` (`MaLoai`),
  CONSTRAINT `sanpham_ibfk_2` FOREIGN KEY (`MaCo`) REFERENCES `co` (`MaCo`),
  CONSTRAINT `sanpham_ibfk_3` FOREIGN KEY (`MaChatLieu`) REFERENCES `chatlieu` (`MaChatLieu`),
  CONSTRAINT `sanpham_ibfk_4` FOREIGN KEY (`MaMau`) REFERENCES `mau` (`MaMau`),
  CONSTRAINT `sanpham_ibfk_5` FOREIGN KEY (`MaDoiTuong`) REFERENCES `doituong` (`MaDoiTuong`),
  CONSTRAINT `sanpham_ibfk_6` FOREIGN KEY (`MaMua`) REFERENCES `mua` (`MaMua`),
  CONSTRAINT `sanpham_ibfk_7` FOREIGN KEY (`MaNSX`) REFERENCES `noisanxuat` (`MaNSX`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sanpham`
--

LOCK TABLES `sanpham` WRITE;
/*!40000 ALTER TABLE `sanpham` DISABLE KEYS */;
INSERT INTO `sanpham` VALUES (3,'Váy mùa hè',3,3,3,2,2,2,3,106,'D:/ProjectC#/ClothingStore/ClothingStore/images/images (1).jpg',45000.00,400000.00),(5,'Khăn ấm',5,5,5,4,4,4,5,104,'D:/ProjectC#/ClothingStore/ClothingStore/images/tải xuống (2).jpg',45000.00,200000.00),(6,'quần đẹp',NULL,NULL,NULL,NULL,NULL,NULL,NULL,70,NULL,400000.00,900000.00),(7,'quần xấu',1,1,1,1,1,1,1,120,'D:/ProjectC#/ClothingStore/ClothingStore/images/tải xuống (4).jpg',5000.00,900000.00);
/*!40000 ALTER TABLE `sanpham` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taikhoan`
--

DROP TABLE IF EXISTS `taikhoan`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `taikhoan` (
  `MaTaiKhoan` varchar(50) NOT NULL,
  `TenDangNhap` varchar(50) NOT NULL,
  `MatKhau` varchar(255) NOT NULL,
  `LoaiTaiKhoan` enum('Admin','NhanVien','KhachHang') NOT NULL,
  PRIMARY KEY (`MaTaiKhoan`),
  UNIQUE KEY `TenDangNhap` (`TenDangNhap`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taikhoan`
--

LOCK TABLES `taikhoan` WRITE;
/*!40000 ALTER TABLE `taikhoan` DISABLE KEYS */;
INSERT INTO `taikhoan` VALUES ('2','biwhd','123','KhachHang'),('5','binh001','12345678','KhachHang'),('admin','admin','456','Admin');
/*!40000 ALTER TABLE `taikhoan` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `theloai`
--

DROP TABLE IF EXISTS `theloai`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `theloai` (
  `MaLoai` int NOT NULL AUTO_INCREMENT,
  `TenLoai` varchar(50) NOT NULL,
  PRIMARY KEY (`MaLoai`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `theloai`
--

LOCK TABLES `theloai` WRITE;
/*!40000 ALTER TABLE `theloai` DISABLE KEYS */;
INSERT INTO `theloai` VALUES (1,'Áo'),(2,'Quần'),(3,'Váy'),(4,'Giày'),(5,'Phụ kiện');
/*!40000 ALTER TABLE `theloai` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-17 21:57:01
