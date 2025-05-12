namespace ClothingStore.NhanVien
{
    partial class XemDatHang_UserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnXemGioHang = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(493, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 25);
            this.label4.TabIndex = 88;
            this.label4.Text = "Đơn Đặt Hàng";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(92, 75);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(993, 305);
            this.dataGridView1.TabIndex = 89;
            // 
            // btnXemGioHang
            // 
            this.btnXemGioHang.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnXemGioHang.Location = new System.Drawing.Point(498, 477);
            this.btnXemGioHang.Name = "btnXemGioHang";
            this.btnXemGioHang.Size = new System.Drawing.Size(151, 38);
            this.btnXemGioHang.TabIndex = 90;
            this.btnXemGioHang.Text = "Lập Hóa Đơn";
            this.btnXemGioHang.UseVisualStyleBackColor = true;
            this.btnXemGioHang.Click += new System.EventHandler(this.btnLapHoaDonBan_Click);
            // 
            // XemDatHang_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnXemGioHang);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label4);
            this.Name = "XemDatHang_UserControl";
            this.Size = new System.Drawing.Size(1204, 595);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnXemGioHang;
    }
}
