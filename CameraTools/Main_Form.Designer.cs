namespace CameraTools
{
    partial class Main_Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.p_Right = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_ModifyIp = new System.Windows.Forms.Button();
            this.btn_RuleCfg = new System.Windows.Forms.Button();
            this.btn_ParamSet = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_Open = new System.Windows.Forms.Button();
            this.tv_CameraList = new System.Windows.Forms.TreeView();
            this.imglist = new System.Windows.Forms.ImageList(this.components);
            this.btn_SearchCamera = new System.Windows.Forms.Button();
            this.fp_Left = new System.Windows.Forms.FlowLayoutPanel();
            this.dgv_ResultList = new System.Windows.Forms.DataGridView();
            this.c_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c_plate = new System.Windows.Forms.DataGridViewImageColumn();
            this.c_StrPlate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.p_Right.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ResultList)).BeginInit();
            this.SuspendLayout();
            // 
            // p_Right
            // 
            this.p_Right.Controls.Add(this.groupBox1);
            this.p_Right.Controls.Add(this.btn_Close);
            this.p_Right.Controls.Add(this.btn_Open);
            this.p_Right.Controls.Add(this.tv_CameraList);
            this.p_Right.Controls.Add(this.btn_SearchCamera);
            this.p_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.p_Right.Location = new System.Drawing.Point(801, 0);
            this.p_Right.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.p_Right.Name = "p_Right";
            this.p_Right.Size = new System.Drawing.Size(233, 561);
            this.p_Right.TabIndex = 1;
            this.p_Right.Paint += new System.Windows.Forms.PaintEventHandler(this.p_Right_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_ModifyIp);
            this.groupBox1.Controls.Add(this.btn_RuleCfg);
            this.groupBox1.Controls.Add(this.btn_ParamSet);
            this.groupBox1.Location = new System.Drawing.Point(7, 368);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(220, 166);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相机参数设置";
            // 
            // btn_ModifyIp
            // 
            this.btn_ModifyIp.Enabled = false;
            this.btn_ModifyIp.Location = new System.Drawing.Point(127, 24);
            this.btn_ModifyIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_ModifyIp.Name = "btn_ModifyIp";
            this.btn_ModifyIp.Size = new System.Drawing.Size(87, 33);
            this.btn_ModifyIp.TabIndex = 5;
            this.btn_ModifyIp.Text = "修改 IP";
            this.btn_ModifyIp.UseVisualStyleBackColor = true;
            this.btn_ModifyIp.Click += new System.EventHandler(this.btn_ModifyIp_Click);
            // 
            // btn_RuleCfg
            // 
            this.btn_RuleCfg.Enabled = false;
            this.btn_RuleCfg.Location = new System.Drawing.Point(6, 24);
            this.btn_RuleCfg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_RuleCfg.Name = "btn_RuleCfg";
            this.btn_RuleCfg.Size = new System.Drawing.Size(87, 33);
            this.btn_RuleCfg.TabIndex = 4;
            this.btn_RuleCfg.Text = "线圈设置";
            this.btn_RuleCfg.UseVisualStyleBackColor = true;
            this.btn_RuleCfg.Click += new System.EventHandler(this.btn_RuleCfg_Click);
            // 
            // btn_ParamSet
            // 
            this.btn_ParamSet.Enabled = false;
            this.btn_ParamSet.Location = new System.Drawing.Point(6, 77);
            this.btn_ParamSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_ParamSet.Name = "btn_ParamSet";
            this.btn_ParamSet.Size = new System.Drawing.Size(87, 33);
            this.btn_ParamSet.TabIndex = 6;
            this.btn_ParamSet.Text = "参数设置";
            this.btn_ParamSet.UseVisualStyleBackColor = true;
            this.btn_ParamSet.Click += new System.EventHandler(this.btn_ParamSet_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Enabled = false;
            this.btn_Close.Location = new System.Drawing.Point(140, 314);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(87, 33);
            this.btn_Close.TabIndex = 3;
            this.btn_Close.Text = "关 闭";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_Open
            // 
            this.btn_Open.Enabled = false;
            this.btn_Open.Location = new System.Drawing.Point(6, 314);
            this.btn_Open.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(87, 33);
            this.btn_Open.TabIndex = 2;
            this.btn_Open.Text = "打 开";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // tv_CameraList
            // 
            this.tv_CameraList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tv_CameraList.FullRowSelect = true;
            this.tv_CameraList.ImageIndex = 0;
            this.tv_CameraList.ImageList = this.imglist;
            this.tv_CameraList.Location = new System.Drawing.Point(6, 55);
            this.tv_CameraList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tv_CameraList.Name = "tv_CameraList";
            this.tv_CameraList.SelectedImageIndex = 0;
            this.tv_CameraList.Size = new System.Drawing.Size(221, 250);
            this.tv_CameraList.TabIndex = 1;
            this.tv_CameraList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_CameraList_AfterSelect);
            // 
            // imglist
            // 
            this.imglist.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglist.ImageStream")));
            this.imglist.TransparentColor = System.Drawing.Color.Transparent;
            this.imglist.Images.SetKeyName(0, "delete.png");
            this.imglist.Images.SetKeyName(1, "success.png");
            // 
            // btn_SearchCamera
            // 
            this.btn_SearchCamera.Location = new System.Drawing.Point(6, 7);
            this.btn_SearchCamera.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_SearchCamera.Name = "btn_SearchCamera";
            this.btn_SearchCamera.Size = new System.Drawing.Size(222, 40);
            this.btn_SearchCamera.TabIndex = 0;
            this.btn_SearchCamera.Text = "搜 索 像 机";
            this.btn_SearchCamera.UseVisualStyleBackColor = true;
            this.btn_SearchCamera.Click += new System.EventHandler(this.btn_SearchCamera_Click);
            // 
            // fp_Left
            // 
            this.fp_Left.AutoScroll = true;
            this.fp_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fp_Left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fp_Left.Location = new System.Drawing.Point(0, 0);
            this.fp_Left.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fp_Left.Name = "fp_Left";
            this.fp_Left.Size = new System.Drawing.Size(801, 363);
            this.fp_Left.TabIndex = 2;
            this.fp_Left.Paint += new System.Windows.Forms.PaintEventHandler(this.fp_Left_Paint);
            this.fp_Left.Resize += new System.EventHandler(this.fp_Left_Resize);
            // 
            // dgv_ResultList
            // 
            this.dgv_ResultList.AllowUserToAddRows = false;
            this.dgv_ResultList.AllowUserToDeleteRows = false;
            this.dgv_ResultList.AllowUserToResizeColumns = false;
            this.dgv_ResultList.AllowUserToResizeRows = false;
            this.dgv_ResultList.BackgroundColor = System.Drawing.Color.White;
            this.dgv_ResultList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_ResultList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_ResultList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_ResultList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_ResultList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_ResultList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.c_id,
            this.c_IP,
            this.c_plate,
            this.c_StrPlate,
            this.c_Time,
            this.Column5,
            this.Column6});
            this.dgv_ResultList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv_ResultList.EnableHeadersVisualStyles = false;
            this.dgv_ResultList.Location = new System.Drawing.Point(0, 363);
            this.dgv_ResultList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgv_ResultList.MultiSelect = false;
            this.dgv_ResultList.Name = "dgv_ResultList";
            this.dgv_ResultList.ReadOnly = true;
            this.dgv_ResultList.RowHeadersVisible = false;
            this.dgv_ResultList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_ResultList.RowTemplate.Height = 36;
            this.dgv_ResultList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_ResultList.Size = new System.Drawing.Size(801, 198);
            this.dgv_ResultList.TabIndex = 0;
            this.dgv_ResultList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_ResultList_CellClick);
            this.dgv_ResultList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_ResultList_CellFormatting);
            this.dgv_ResultList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgv_ResultList_RowsAdded);
            // 
            // c_id
            // 
            this.c_id.HeaderText = "编号";
            this.c_id.Name = "c_id";
            this.c_id.ReadOnly = true;
            this.c_id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c_id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // c_IP
            // 
            this.c_IP.HeaderText = "IP 地址";
            this.c_IP.Name = "c_IP";
            this.c_IP.ReadOnly = true;
            this.c_IP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // c_plate
            // 
            this.c_plate.HeaderText = "抓拍车牌";
            this.c_plate.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.c_plate.Name = "c_plate";
            this.c_plate.ReadOnly = true;
            this.c_plate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c_plate.Width = 150;
            // 
            // c_StrPlate
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.c_StrPlate.DefaultCellStyle = dataGridViewCellStyle2;
            this.c_StrPlate.HeaderText = "识别车牌";
            this.c_StrPlate.Name = "c_StrPlate";
            this.c_StrPlate.ReadOnly = true;
            this.c_StrPlate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c_StrPlate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.c_StrPlate.Width = 150;
            // 
            // c_Time
            // 
            dataGridViewCellStyle3.Format = "F";
            dataGridViewCellStyle3.NullValue = null;
            this.c_Time.DefaultCellStyle = dataGridViewCellStyle3;
            this.c_Time.HeaderText = "识别时间";
            this.c_Time.Name = "c_Time";
            this.c_Time.ReadOnly = true;
            this.c_Time.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c_Time.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.c_Time.Width = 180;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "车牌类型";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "车牌颜色";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 561);
            this.Controls.Add(this.fp_Left);
            this.Controls.Add(this.dgv_ResultList);
            this.Controls.Add(this.p_Right);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Main_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "识别像机配置工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.p_Right.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ResultList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel p_Right;
        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.TreeView tv_CameraList;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.FlowLayoutPanel fp_Left;
        private System.Windows.Forms.DataGridView dgv_ResultList;
        private System.Windows.Forms.ImageList imglist;
        private System.Windows.Forms.Button btn_ParamSet;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_SearchCamera;
        private System.Windows.Forms.Button btn_RuleCfg;
        private System.Windows.Forms.Button btn_ModifyIp;
        private System.Windows.Forms.DataGridViewTextBoxColumn c_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn c_IP;
        private System.Windows.Forms.DataGridViewImageColumn c_plate;
        private System.Windows.Forms.DataGridViewTextBoxColumn c_StrPlate;
        private System.Windows.Forms.DataGridViewTextBoxColumn c_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}

