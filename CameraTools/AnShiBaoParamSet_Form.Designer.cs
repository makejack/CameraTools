namespace CameraTools
{
    partial class AnShiBaoParamSet_Form
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
            this.tb_ServerIp = new System.Windows.Forms.TextBox();
            this.cb_Provinces = new System.Windows.Forms.ComboBox();
            this.btn_SetTime = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.l_Ip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "接收地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "预设省份";
            // 
            // tb_ServerIp
            // 
            this.tb_ServerIp.Location = new System.Drawing.Point(95, 82);
            this.tb_ServerIp.MaxLength = 16;
            this.tb_ServerIp.Name = "tb_ServerIp";
            this.tb_ServerIp.Size = new System.Drawing.Size(153, 21);
            this.tb_ServerIp.TabIndex = 2;
            // 
            // cb_Provinces
            // 
            this.cb_Provinces.FormattingEnabled = true;
            this.cb_Provinces.Items.AddRange(new object[] {
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
            this.cb_Provinces.Location = new System.Drawing.Point(95, 135);
            this.cb_Provinces.Name = "cb_Provinces";
            this.cb_Provinces.Size = new System.Drawing.Size(153, 20);
            this.cb_Provinces.TabIndex = 6;
            // 
            // btn_SetTime
            // 
            this.btn_SetTime.Location = new System.Drawing.Point(56, 200);
            this.btn_SetTime.Name = "btn_SetTime";
            this.btn_SetTime.Size = new System.Drawing.Size(75, 23);
            this.btn_SetTime.TabIndex = 11;
            this.btn_SetTime.Text = "同步时间";
            this.btn_SetTime.UseVisualStyleBackColor = true;
            this.btn_SetTime.Click += new System.EventHandler(this.btn_SetTime_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(147, 200);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 12;
            this.btn_Save.Text = "保 存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // l_Ip
            // 
            this.l_Ip.AutoSize = true;
            this.l_Ip.Location = new System.Drawing.Point(36, 38);
            this.l_Ip.Name = "l_Ip";
            this.l_Ip.Size = new System.Drawing.Size(41, 12);
            this.l_Ip.TabIndex = 13;
            this.l_Ip.Text = "label3";
            // 
            // AnShiBaoParamSet_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.l_Ip);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_SetTime);
            this.Controls.Add(this.cb_Provinces);
            this.Controls.Add(this.tb_ServerIp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnShiBaoParamSet_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "基本参数设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnShiBaoParamSet_Form_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_ServerIp;
        private System.Windows.Forms.ComboBox cb_Provinces;
        private System.Windows.Forms.Button btn_SetTime;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Label l_Ip;
    }
}