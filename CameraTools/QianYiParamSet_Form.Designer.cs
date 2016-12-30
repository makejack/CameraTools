namespace CameraTools
{
    partial class QianYiParamSet_Form
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
            this.btn_ZoomTele = new System.Windows.Forms.Button();
            this.btn_ZoomWide = new System.Windows.Forms.Button();
            this.btn_FocusFar = new System.Windows.Forms.Button();
            this.btn_FocusNear = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_SetTime = new System.Windows.Forms.Button();
            this.cb_Provinces = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_ZoomTele
            // 
            this.btn_ZoomTele.Location = new System.Drawing.Point(173, 89);
            this.btn_ZoomTele.Name = "btn_ZoomTele";
            this.btn_ZoomTele.Size = new System.Drawing.Size(25, 23);
            this.btn_ZoomTele.TabIndex = 33;
            this.btn_ZoomTele.Text = "+";
            this.btn_ZoomTele.UseVisualStyleBackColor = true;
            this.btn_ZoomTele.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomTele_MouseDown);
            this.btn_ZoomTele.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomTele_MouseUp);
            // 
            // btn_ZoomWide
            // 
            this.btn_ZoomWide.Location = new System.Drawing.Point(120, 89);
            this.btn_ZoomWide.Name = "btn_ZoomWide";
            this.btn_ZoomWide.Size = new System.Drawing.Size(25, 23);
            this.btn_ZoomWide.TabIndex = 32;
            this.btn_ZoomWide.Text = "-";
            this.btn_ZoomWide.UseVisualStyleBackColor = true;
            this.btn_ZoomWide.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomWide_MouseDown);
            this.btn_ZoomWide.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_ZoomWide_MouseUp);
            // 
            // btn_FocusFar
            // 
            this.btn_FocusFar.Location = new System.Drawing.Point(173, 48);
            this.btn_FocusFar.Name = "btn_FocusFar";
            this.btn_FocusFar.Size = new System.Drawing.Size(25, 23);
            this.btn_FocusFar.TabIndex = 31;
            this.btn_FocusFar.Text = "+";
            this.btn_FocusFar.UseVisualStyleBackColor = true;
            this.btn_FocusFar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_FocusFar_MouseDown);
            this.btn_FocusFar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_FocusFar_MouseUp);
            // 
            // btn_FocusNear
            // 
            this.btn_FocusNear.Location = new System.Drawing.Point(120, 48);
            this.btn_FocusNear.Name = "btn_FocusNear";
            this.btn_FocusNear.Size = new System.Drawing.Size(25, 23);
            this.btn_FocusNear.TabIndex = 30;
            this.btn_FocusNear.Text = "-";
            this.btn_FocusNear.UseVisualStyleBackColor = true;
            this.btn_FocusNear.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_FocusNear_MouseDown);
            this.btn_FocusNear.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_FocusNear_MouseUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 29;
            this.label6.Text = "变 倍";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "聚 焦";
            // 
            // btn_SetTime
            // 
            this.btn_SetTime.Location = new System.Drawing.Point(105, 190);
            this.btn_SetTime.Name = "btn_SetTime";
            this.btn_SetTime.Size = new System.Drawing.Size(75, 23);
            this.btn_SetTime.TabIndex = 27;
            this.btn_SetTime.Text = "同步时间";
            this.btn_SetTime.UseVisualStyleBackColor = true;
            this.btn_SetTime.Click += new System.EventHandler(this.btn_SetTime_Click);
            // 
            // cb_Provinces
            // 
            this.cb_Provinces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_Provinces.FormattingEnabled = true;
            this.cb_Provinces.Location = new System.Drawing.Point(98, 139);
            this.cb_Provinces.Name = "cb_Provinces";
            this.cb_Provinces.Size = new System.Drawing.Size(153, 20);
            this.cb_Provinces.TabIndex = 22;
            this.cb_Provinces.SelectedIndexChanged += new System.EventHandler(this.cb_Provinces_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "预设省份";
            // 
            // QianYiParamSet_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btn_ZoomTele);
            this.Controls.Add(this.btn_ZoomWide);
            this.Controls.Add(this.btn_FocusFar);
            this.Controls.Add(this.btn_FocusNear);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_SetTime);
            this.Controls.Add(this.cb_Provinces);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QianYiParamSet_Form";
            this.Text = "基本参数修改";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_ZoomTele;
        private System.Windows.Forms.Button btn_ZoomWide;
        private System.Windows.Forms.Button btn_FocusFar;
        private System.Windows.Forms.Button btn_FocusNear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_SetTime;
        private System.Windows.Forms.ComboBox cb_Provinces;
        private System.Windows.Forms.Label label2;


    }
}