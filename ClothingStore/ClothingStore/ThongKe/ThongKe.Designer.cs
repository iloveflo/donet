namespace ClothingStore.ThongKe
{
    partial class ThongKe
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
            this.tabPage_mathangsaphet = new System.Windows.Forms.TabPage();
            this.dgvDanhSachMatHangSapHet = new System.Windows.Forms.DataGridView();
            this.panel9 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.txtAnh = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.pictureBoxAnh = new System.Windows.Forms.PictureBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.txtTenQuanAo = new System.Windows.Forms.TextBox();
            this.txtMaQuanAo = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tabControl_sub_thongke = new System.Windows.Forms.TabControl();
            this.tabPage_mathangsaphet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachMatHangSapHet)).BeginInit();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnh)).BeginInit();
            this.tabControl_sub_thongke.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage_mathangsaphet
            // 
            this.tabPage_mathangsaphet.Controls.Add(this.dgvDanhSachMatHangSapHet);
            this.tabPage_mathangsaphet.Controls.Add(this.panel9);
            this.tabPage_mathangsaphet.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPage_mathangsaphet.Location = new System.Drawing.Point(4, 31);
            this.tabPage_mathangsaphet.Name = "tabPage_mathangsaphet";
            this.tabPage_mathangsaphet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_mathangsaphet.Size = new System.Drawing.Size(1139, 800);
            this.tabPage_mathangsaphet.TabIndex = 0;
            this.tabPage_mathangsaphet.Text = "  Mặt Hàng Sắp Hết  ";
            this.tabPage_mathangsaphet.UseVisualStyleBackColor = true;
            // 
            // dgvDanhSachMatHangSapHet
            // 
            this.dgvDanhSachMatHangSapHet.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvDanhSachMatHangSapHet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSachMatHangSapHet.Location = new System.Drawing.Point(3, 386);
            this.dgvDanhSachMatHangSapHet.Name = "dgvDanhSachMatHangSapHet";
            this.dgvDanhSachMatHangSapHet.RowHeadersWidth = 51;
            this.dgvDanhSachMatHangSapHet.RowTemplate.Height = 24;
            this.dgvDanhSachMatHangSapHet.Size = new System.Drawing.Size(1130, 411);
            this.dgvDanhSachMatHangSapHet.TabIndex = 1;
            this.dgvDanhSachMatHangSapHet.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDanhSachMatHangSapHet_CellClick);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.MistyRose;
            this.panel9.Controls.Add(this.button3);
            this.panel9.Controls.Add(this.txtAnh);
            this.panel9.Controls.Add(this.button6);
            this.panel9.Controls.Add(this.pictureBoxAnh);
            this.panel9.Controls.Add(this.txtSoLuong);
            this.panel9.Controls.Add(this.txtTenQuanAo);
            this.panel9.Controls.Add(this.txtMaQuanAo);
            this.panel9.Controls.Add(this.label25);
            this.panel9.Controls.Add(this.label26);
            this.panel9.Controls.Add(this.label27);
            this.panel9.Controls.Add(this.label28);
            this.panel9.Controls.Add(this.label24);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1133, 377);
            this.panel9.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button3.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(5, 56);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 50);
            this.button3.TabIndex = 34;
            this.button3.Text = "Hóa đơn nhập";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.btnHoaDonNhap_Click);
            // 
            // txtAnh
            // 
            this.txtAnh.Location = new System.Drawing.Point(606, 330);
            this.txtAnh.Name = "txtAnh";
            this.txtAnh.Size = new System.Drawing.Size(100, 39);
            this.txtAnh.TabIndex = 33;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button6.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.button6.ForeColor = System.Drawing.Color.Red;
            this.button6.Location = new System.Drawing.Point(3, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(150, 50);
            this.button6.TabIndex = 32;
            this.button6.Text = "Quay lại";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.btnQuayLai_Click);
            // 
            // pictureBoxAnh
            // 
            this.pictureBoxAnh.BackColor = System.Drawing.Color.White;
            this.pictureBoxAnh.Location = new System.Drawing.Point(735, 73);
            this.pictureBoxAnh.Name = "pictureBoxAnh";
            this.pictureBoxAnh.Size = new System.Drawing.Size(237, 296);
            this.pictureBoxAnh.TabIndex = 31;
            this.pictureBoxAnh.TabStop = false;
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(359, 269);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(181, 39);
            this.txtSoLuong.TabIndex = 28;
            // 
            // txtTenQuanAo
            // 
            this.txtTenQuanAo.Location = new System.Drawing.Point(359, 164);
            this.txtTenQuanAo.Name = "txtTenQuanAo";
            this.txtTenQuanAo.Size = new System.Drawing.Size(181, 39);
            this.txtTenQuanAo.TabIndex = 27;
            // 
            // txtMaQuanAo
            // 
            this.txtMaQuanAo.Location = new System.Drawing.Point(359, 80);
            this.txtMaQuanAo.Name = "txtMaQuanAo";
            this.txtMaQuanAo.Size = new System.Drawing.Size(181, 39);
            this.txtMaQuanAo.TabIndex = 26;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label25.ForeColor = System.Drawing.Color.Blue;
            this.label25.Location = new System.Drawing.Point(547, 347);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(42, 22);
            this.label25.TabIndex = 25;
            this.label25.Text = "Ảnh";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label26.ForeColor = System.Drawing.Color.Blue;
            this.label26.Location = new System.Drawing.Point(156, 279);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(155, 22);
            this.label26.TabIndex = 24;
            this.label26.Text = "Số Lượng Còn Lại";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label27.ForeColor = System.Drawing.Color.Blue;
            this.label27.Location = new System.Drawing.Point(196, 181);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(115, 22);
            this.label27.TabIndex = 23;
            this.label27.Text = "Tên Quần Áo";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label28.ForeColor = System.Drawing.Color.Blue;
            this.label28.Location = new System.Drawing.Point(205, 90);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(106, 22);
            this.label28.TabIndex = 22;
            this.label28.Text = "Mã Quần áo";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.White;
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label24.Location = new System.Drawing.Point(397, 13);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(441, 33);
            this.label24.TabIndex = 0;
            this.label24.Text = "DANH SÁCH MẶT HÀNG SẮP HẾT";
            // 
            // tabControl_sub_thongke
            // 
            this.tabControl_sub_thongke.Controls.Add(this.tabPage_mathangsaphet);
            this.tabControl_sub_thongke.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_sub_thongke.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabControl_sub_thongke.Location = new System.Drawing.Point(0, 0);
            this.tabControl_sub_thongke.Name = "tabControl_sub_thongke";
            this.tabControl_sub_thongke.SelectedIndex = 0;
            this.tabControl_sub_thongke.Size = new System.Drawing.Size(1147, 835);
            this.tabControl_sub_thongke.TabIndex = 2;
            // 
            // ThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 835);
            this.Controls.Add(this.tabControl_sub_thongke);
            this.Name = "ThongKe";
            this.Text = "ThongKe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.tabPage_mathangsaphet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachMatHangSapHet)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnh)).EndInit();
            this.tabControl_sub_thongke.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage_mathangsaphet;
        private System.Windows.Forms.DataGridView dgvDanhSachMatHangSapHet;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtAnh;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.PictureBox pictureBoxAnh;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.TextBox txtTenQuanAo;
        private System.Windows.Forms.TextBox txtMaQuanAo;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TabControl tabControl_sub_thongke;
    }
}