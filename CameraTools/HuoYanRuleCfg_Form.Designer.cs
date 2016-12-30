namespace CameraTools
{
    partial class HuoYanRuleCfg_Form
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
            this.pb_img = new System.Windows.Forms.PictureBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ud_Time = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_Dir = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_img)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Time)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_img
            // 
            this.pb_img.Dock = System.Windows.Forms.DockStyle.Left;
            this.pb_img.Location = new System.Drawing.Point(0, 0);
            this.pb_img.Name = "pb_img";
            this.pb_img.Size = new System.Drawing.Size(800, 456);
            this.pb_img.TabIndex = 0;
            this.pb_img.TabStop = false;
            this.pb_img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_img_MouseDown);
            this.pb_img.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_img_MouseMove);
            this.pb_img.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_img_MouseUp);
            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_save.Location = new System.Drawing.Point(850, 290);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(87, 33);
            this.btn_save.TabIndex = 4;
            this.btn_save.Text = "保 存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(816, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "相同车牌间隔时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(816, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "时间间隔";
            // 
            // ud_Time
            // 
            this.ud_Time.Location = new System.Drawing.Point(878, 158);
            this.ud_Time.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.ud_Time.Name = "ud_Time";
            this.ud_Time.Size = new System.Drawing.Size(69, 21);
            this.ud_Time.TabIndex = 7;
            this.ud_Time.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(953, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "秒";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(816, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "车辆通过方向";
            // 
            // cb_Dir
            // 
            this.cb_Dir.FormattingEnabled = true;
            this.cb_Dir.Items.AddRange(new object[] {
            "双向",
            "由上至下",
            "由下至上"});
            this.cb_Dir.Location = new System.Drawing.Point(818, 232);
            this.cb_Dir.Name = "cb_Dir";
            this.cb_Dir.Size = new System.Drawing.Size(152, 20);
            this.cb_Dir.TabIndex = 10;
            // 
            // RuleCfg_Form
            // 
            this.AcceptButton = this.btn_save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 456);
            this.Controls.Add(this.cb_Dir);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ud_Time);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.pb_img);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RuleCfg_Form";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "线圈设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RuleCfg_FormClosing);
            this.Load += new System.EventHandler(this.RuleCfg_Load);
             ((System.ComponentModel.ISupportInitialize)(this.pb_img)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ud_Time)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_img;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ud_Time;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_Dir;
    }
}