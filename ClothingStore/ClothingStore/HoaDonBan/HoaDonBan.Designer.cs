using System;

namespace ClothingStore.HoaDonBan
{
    partial class HoaDonBan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label37 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label38 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSoHoaDonBan = new System.Windows.Forms.TextBox();
            this.txtMaKhachHang = new System.Windows.Forms.TextBox();
            this.txtMaNhanVien = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.txtMaQuanAo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTenQuanAo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSoLuongBan = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.White;
            this.label37.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label37.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label37.Location = new System.Drawing.Point(341, 17);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(370, 33);
            this.label37.TabIndex = 2;
            this.label37.Text = "DANH SÁCH HÓA ĐƠN BÁN";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(1, 417);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1064, 237);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label38.ForeColor = System.Drawing.Color.Blue;
            this.label38.Location = new System.Drawing.Point(150, 197);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(144, 22);
            this.label38.TabIndex = 33;
            this.label38.Text = "Số Hóa Đơn Bán";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(150, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 22);
            this.label1.TabIndex = 34;
            this.label1.Text = "Mã Khách Hàng";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(166, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 22);
            this.label2.TabIndex = 35;
            this.label2.Text = "Mã Nhân viên";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(664, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 22);
            this.label3.TabIndex = 36;
            this.label3.Text = "Tên Quần Áo";
            // 
            // txtSoHoaDonBan
            // 
            this.txtSoHoaDonBan.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtSoHoaDonBan.Location = new System.Drawing.Point(337, 185);
            this.txtSoHoaDonBan.Name = "txtSoHoaDonBan";
            this.txtSoHoaDonBan.Size = new System.Drawing.Size(232, 34);
            this.txtSoHoaDonBan.TabIndex = 37;
            // 
            // txtMaKhachHang
            // 
            this.txtMaKhachHang.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtMaKhachHang.Location = new System.Drawing.Point(337, 77);
            this.txtMaKhachHang.Name = "txtMaKhachHang";
            this.txtMaKhachHang.Size = new System.Drawing.Size(232, 34);
            this.txtMaKhachHang.TabIndex = 38;
            // 
            // txtMaNhanVien
            // 
            this.txtMaNhanVien.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtMaNhanVien.Location = new System.Drawing.Point(337, 286);
            this.txtMaNhanVien.Name = "txtMaNhanVien";
            this.txtMaNhanVien.Size = new System.Drawing.Size(232, 34);
            this.txtMaNhanVien.TabIndex = 39;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button6.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.button6.ForeColor = System.Drawing.Color.Red;
            this.button6.Location = new System.Drawing.Point(1, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(150, 50);
            this.button6.TabIndex = 43;
            this.button6.Text = "Quay lại";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.btnQuaylai_click);
            // 
            // txtMaQuanAo
            // 
            this.txtMaQuanAo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtMaQuanAo.Location = new System.Drawing.Point(802, 77);
            this.txtMaQuanAo.Name = "txtMaQuanAo";
            this.txtMaQuanAo.Size = new System.Drawing.Size(232, 34);
            this.txtMaQuanAo.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(664, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 22);
            this.label4.TabIndex = 45;
            this.label4.Text = "Số lượng bán";
            // 
            // txtTenQuanAo
            // 
            this.txtTenQuanAo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtTenQuanAo.Location = new System.Drawing.Point(802, 185);
            this.txtTenQuanAo.Name = "txtTenQuanAo";
            this.txtTenQuanAo.Size = new System.Drawing.Size(232, 34);
            this.txtTenQuanAo.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(664, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 22);
            this.label5.TabIndex = 47;
            this.label5.Text = "Mã Quần Áo";
            // 
            // txtSoLuongBan
            // 
            this.txtSoLuongBan.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtSoLuongBan.Location = new System.Drawing.Point(802, 286);
            this.txtSoLuongBan.Name = "txtSoLuongBan";
            this.txtSoLuongBan.Size = new System.Drawing.Size(232, 34);
            this.txtSoLuongBan.TabIndex = 48;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(522, 691);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 40);
            this.button1.TabIndex = 49;
            this.button1.Text = "Xuất hóa đơn";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnInHoaDon_Click);
            // 
            // HoaDonBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(1064, 759);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSoLuongBan);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTenQuanAo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMaQuanAo);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.txtMaNhanVien);
            this.Controls.Add(this.txtMaKhachHang);
            this.Controls.Add(this.txtSoHoaDonBan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label37);
            this.Name = "HoaDonBan";
            this.Text = "Hóa Đơn Bán";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSoHoaDonBan;
        private System.Windows.Forms.TextBox txtMaKhachHang;
        private System.Windows.Forms.TextBox txtMaNhanVien;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtMaQuanAo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTenQuanAo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSoLuongBan;
        private System.Windows.Forms.Button button1;
    }
}