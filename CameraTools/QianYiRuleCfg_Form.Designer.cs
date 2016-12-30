namespace CameraTools
{
    partial class QianYiRuleCfg_Form
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
            ((System.ComponentModel.ISupportInitialize)(this.pb_img)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_img
            // 
            this.pb_img.Dock = System.Windows.Forms.DockStyle.Left;
            this.pb_img.Location = new System.Drawing.Point(0, 0);
            this.pb_img.Name = "pb_img";
            this.pb_img.Size = new System.Drawing.Size(800, 456);
            this.pb_img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_img.TabIndex = 1;
            this.pb_img.TabStop = false;
            this.pb_img.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_img_MouseDown);
            this.pb_img.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_img_MouseMove);
            this.pb_img.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_img_MouseUp);
            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_save.Location = new System.Drawing.Point(850, 212);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(87, 33);
            this.btn_save.TabIndex = 5;
            this.btn_save.Text = "保 存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // QianYiRuleCfg_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 456);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.pb_img);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QianYiRuleCfg_Form";
            this.Text = "线圈设置";
            ((System.ComponentModel.ISupportInitialize)(this.pb_img)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_img;
        private System.Windows.Forms.Button btn_save;
    }
}