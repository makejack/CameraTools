namespace CameraTools
{
    partial class HuoYanParamSet_Form
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_ExposureTime = new System.Windows.Forms.ComboBox();
            this.cb_Provinces = new System.Windows.Forms.ComboBox();
            this.tb_Led = new System.Windows.Forms.TrackBar();
            this.rb_LedMode1 = new System.Windows.Forms.RadioButton();
            this.rb_LedMode2 = new System.Windows.Forms.RadioButton();
            this.rb_LedMode3 = new System.Windows.Forms.RadioButton();
            this.btn_SetTime = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Led)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "曝光时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "预设省份";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "LED亮度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "LED补光";
            // 
            // cb_ExposureTime
            // 
            this.cb_ExposureTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ExposureTime.FormattingEnabled = true;
            this.cb_ExposureTime.Items.AddRange(new object[] {
            "0 ~ 8ms 停车场推荐",
            "0 ~ 4ms",
            "0 ~ 2ms 卡口推荐"});
            this.cb_ExposureTime.Location = new System.Drawing.Point(93, 24);
            this.cb_ExposureTime.Name = "cb_ExposureTime";
            this.cb_ExposureTime.Size = new System.Drawing.Size(153, 20);
            this.cb_ExposureTime.TabIndex = 4;
            this.cb_ExposureTime.SelectedIndexChanged += new System.EventHandler(this.cb_ExposureTime_SelectedIndexChanged);
            // 
            // cb_Provinces
            // 
            this.cb_Provinces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Provinces.FormattingEnabled = true;
            this.cb_Provinces.Items.AddRange(new object[] {
            "无",
            "京",
            "津",
            "冀",
            "晋",
            "蒙",
            "辽",
            "吉",
            "黑",
            "沪",
            "苏",
            "浙",
            "皖",
            "闽",
            "赣",
            "鲁",
            "豫",
            "鄂",
            "湘",
            "粤",
            "桂",
            "琼",
            "渝",
            "川",
            "贵",
            "云",
            "藏",
            "陕",
            "甘",
            "青",
            "宁",
            "新"});
            this.cb_Provinces.Location = new System.Drawing.Point(93, 73);
            this.cb_Provinces.Name = "cb_Provinces";
            this.cb_Provinces.Size = new System.Drawing.Size(153, 20);
            this.cb_Provinces.TabIndex = 5;
            this.cb_Provinces.SelectedIndexChanged += new System.EventHandler(this.cb_Provinces_SelectedIndexChanged);
            // 
            // tb_Led
            // 
            this.tb_Led.AutoSize = false;
            this.tb_Led.LargeChange = 1;
            this.tb_Led.Location = new System.Drawing.Point(93, 122);
            this.tb_Led.Maximum = 6;
            this.tb_Led.Name = "tb_Led";
            this.tb_Led.Size = new System.Drawing.Size(153, 23);
            this.tb_Led.TabIndex = 6;
            this.tb_Led.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tb_Led.Value = 2;
            this.tb_Led.Scroll += new System.EventHandler(this.tb_Led_Scroll);
            // 
            // rb_LedMode1
            // 
            this.rb_LedMode1.AutoSize = true;
            this.rb_LedMode1.Location = new System.Drawing.Point(93, 174);
            this.rb_LedMode1.Name = "rb_LedMode1";
            this.rb_LedMode1.Size = new System.Drawing.Size(47, 16);
            this.rb_LedMode1.TabIndex = 7;
            this.rb_LedMode1.TabStop = true;
            this.rb_LedMode1.Text = "智能";
            this.rb_LedMode1.UseVisualStyleBackColor = true;
            this.rb_LedMode1.CheckedChanged += new System.EventHandler(this.rb_LedMode1_CheckedChanged);
            // 
            // rb_LedMode2
            // 
            this.rb_LedMode2.AutoSize = true;
            this.rb_LedMode2.Location = new System.Drawing.Point(146, 174);
            this.rb_LedMode2.Name = "rb_LedMode2";
            this.rb_LedMode2.Size = new System.Drawing.Size(47, 16);
            this.rb_LedMode2.TabIndex = 8;
            this.rb_LedMode2.TabStop = true;
            this.rb_LedMode2.Text = "常亮";
            this.rb_LedMode2.UseVisualStyleBackColor = true;
            this.rb_LedMode2.CheckedChanged += new System.EventHandler(this.rb_LedMode2_CheckedChanged);
            // 
            // rb_LedMode3
            // 
            this.rb_LedMode3.AutoSize = true;
            this.rb_LedMode3.Location = new System.Drawing.Point(199, 174);
            this.rb_LedMode3.Name = "rb_LedMode3";
            this.rb_LedMode3.Size = new System.Drawing.Size(47, 16);
            this.rb_LedMode3.TabIndex = 9;
            this.rb_LedMode3.TabStop = true;
            this.rb_LedMode3.Text = "常灭";
            this.rb_LedMode3.UseVisualStyleBackColor = true;
            this.rb_LedMode3.CheckedChanged += new System.EventHandler(this.rb_LedMode3_CheckedChanged);
            // 
            // btn_SetTime
            // 
            this.btn_SetTime.Location = new System.Drawing.Point(105, 214);
            this.btn_SetTime.Name = "btn_SetTime";
            this.btn_SetTime.Size = new System.Drawing.Size(75, 23);
            this.btn_SetTime.TabIndex = 10;
            this.btn_SetTime.Text = "同步时间";
            this.btn_SetTime.UseVisualStyleBackColor = true;
            this.btn_SetTime.Click += new System.EventHandler(this.btn_SetTime_Click);
            // 
            // HuoYanParamSet_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btn_SetTime);
            this.Controls.Add(this.rb_LedMode3);
            this.Controls.Add(this.rb_LedMode2);
            this.Controls.Add(this.rb_LedMode1);
            this.Controls.Add(this.tb_Led);
            this.Controls.Add(this.cb_Provinces);
            this.Controls.Add(this.cb_ExposureTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HuoYanParamSet_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "基本参数设置";
            this.Load += new System.EventHandler(this.ParamSet_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tb_Led)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_ExposureTime;
        private System.Windows.Forms.ComboBox cb_Provinces;
        private System.Windows.Forms.TrackBar tb_Led;
        private System.Windows.Forms.RadioButton rb_LedMode1;
        private System.Windows.Forms.RadioButton rb_LedMode2;
        private System.Windows.Forms.RadioButton rb_LedMode3;
        private System.Windows.Forms.Button btn_SetTime;
    }
}