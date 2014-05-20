namespace WinFormClient
{
    partial class FormMain
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextCategory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenuLocalDirectoryBind = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextItemCategoryRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.contextLocalDirectory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextItemSetCanUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabUpload = new System.Windows.Forms.TabPage();
            this.btnUploadDirectoryRemove = new System.Windows.Forms.Button();
            this.btnUploadDirectoryAdd = new System.Windows.Forms.Button();
            this.groupBoxUploadDirectory = new System.Windows.Forms.GroupBox();
            this.listBoxUploadDirectory = new System.Windows.Forms.ListBox();
            this.groupBoxLocalDirectory = new System.Windows.Forms.GroupBox();
            this.listBoxLocalDirectory = new System.Windows.Forms.ListBox();
            this.treeCategory = new System.Windows.Forms.TreeView();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.statusStrip.SuspendLayout();
            this.contextCategory.SuspendLayout();
            this.contextLocalDirectory.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabUpload.SuspendLayout();
            this.groupBoxUploadDirectory.SuspendLayout();
            this.groupBoxLocalDirectory.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 553);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1194, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // contextCategory
            // 
            this.contextCategory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuLocalDirectoryBind,
            this.ContextItemCategoryRefresh});
            this.contextCategory.Name = "contextCategory";
            this.contextCategory.Size = new System.Drawing.Size(143, 48);
            // 
            // ContextMenuLocalDirectoryBind
            // 
            this.ContextMenuLocalDirectoryBind.Name = "ContextMenuLocalDirectoryBind";
            this.ContextMenuLocalDirectoryBind.Size = new System.Drawing.Size(142, 22);
            this.ContextMenuLocalDirectoryBind.Text = "关联本地目录";
            this.ContextMenuLocalDirectoryBind.Click += new System.EventHandler(this.ContextMenuLocalDirectoryBind_Click);
            // 
            // ContextItemCategoryRefresh
            // 
            this.ContextItemCategoryRefresh.Name = "ContextItemCategoryRefresh";
            this.ContextItemCategoryRefresh.Size = new System.Drawing.Size(142, 22);
            this.ContextItemCategoryRefresh.Text = "刷新分类";
            this.ContextItemCategoryRefresh.Click += new System.EventHandler(this.ContextItemCategoryRefresh_Click);
            // 
            // contextLocalDirectory
            // 
            this.contextLocalDirectory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextItemSetCanUpload});
            this.contextLocalDirectory.Name = "contextLocalDirectory";
            this.contextLocalDirectory.Size = new System.Drawing.Size(153, 48);
            // 
            // contextItemSetCanUpload
            // 
            this.contextItemSetCanUpload.Name = "contextItemSetCanUpload";
            this.contextItemSetCanUpload.Size = new System.Drawing.Size(152, 22);
            this.contextItemSetCanUpload.Text = "设为可上传";
            this.contextItemSetCanUpload.Click += new System.EventHandler(this.contextItemSetCanUpload_Click);
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabUpload);
            this.tabs.Controls.Add(this.tabResult);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1194, 553);
            this.tabs.TabIndex = 7;
            // 
            // tabUpload
            // 
            this.tabUpload.Controls.Add(this.btnUploadDirectoryRemove);
            this.tabUpload.Controls.Add(this.btnUploadDirectoryAdd);
            this.tabUpload.Controls.Add(this.groupBoxUploadDirectory);
            this.tabUpload.Controls.Add(this.groupBoxLocalDirectory);
            this.tabUpload.Controls.Add(this.treeCategory);
            this.tabUpload.Location = new System.Drawing.Point(4, 22);
            this.tabUpload.Name = "tabUpload";
            this.tabUpload.Padding = new System.Windows.Forms.Padding(3);
            this.tabUpload.Size = new System.Drawing.Size(1186, 527);
            this.tabUpload.TabIndex = 0;
            this.tabUpload.Text = "文件上传";
            this.tabUpload.UseVisualStyleBackColor = true;
            // 
            // btnUploadDirectoryRemove
            // 
            this.btnUploadDirectoryRemove.Location = new System.Drawing.Point(680, 213);
            this.btnUploadDirectoryRemove.Name = "btnUploadDirectoryRemove";
            this.btnUploadDirectoryRemove.Size = new System.Drawing.Size(30, 23);
            this.btnUploadDirectoryRemove.TabIndex = 8;
            this.btnUploadDirectoryRemove.Text = "<<";
            this.btnUploadDirectoryRemove.UseVisualStyleBackColor = true;
            this.btnUploadDirectoryRemove.Click += new System.EventHandler(this.btnUploadDirectoryRemove_Click);
            // 
            // btnUploadDirectoryAdd
            // 
            this.btnUploadDirectoryAdd.Location = new System.Drawing.Point(680, 164);
            this.btnUploadDirectoryAdd.Name = "btnUploadDirectoryAdd";
            this.btnUploadDirectoryAdd.Size = new System.Drawing.Size(30, 23);
            this.btnUploadDirectoryAdd.TabIndex = 7;
            this.btnUploadDirectoryAdd.Text = ">>";
            this.btnUploadDirectoryAdd.UseVisualStyleBackColor = true;
            this.btnUploadDirectoryAdd.Click += new System.EventHandler(this.btnUploadDirectoryAdd_Click);
            // 
            // groupBoxUploadDirectory
            // 
            this.groupBoxUploadDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUploadDirectory.Controls.Add(this.listBoxUploadDirectory);
            this.groupBoxUploadDirectory.Location = new System.Drawing.Point(716, 6);
            this.groupBoxUploadDirectory.Name = "groupBoxUploadDirectory";
            this.groupBoxUploadDirectory.Size = new System.Drawing.Size(460, 515);
            this.groupBoxUploadDirectory.TabIndex = 5;
            this.groupBoxUploadDirectory.TabStop = false;
            this.groupBoxUploadDirectory.Text = "待上传目录";
            // 
            // listBoxUploadDirectory
            // 
            this.listBoxUploadDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUploadDirectory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxUploadDirectory.FormattingEnabled = true;
            this.listBoxUploadDirectory.HorizontalScrollbar = true;
            this.listBoxUploadDirectory.ItemHeight = 20;
            this.listBoxUploadDirectory.Location = new System.Drawing.Point(3, 17);
            this.listBoxUploadDirectory.Name = "listBoxUploadDirectory";
            this.listBoxUploadDirectory.ScrollAlwaysVisible = true;
            this.listBoxUploadDirectory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxUploadDirectory.Size = new System.Drawing.Size(454, 495);
            this.listBoxUploadDirectory.TabIndex = 4;
            this.listBoxUploadDirectory.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxUploadDirectory_DrawItem);
            this.listBoxUploadDirectory.SelectedIndexChanged += new System.EventHandler(this.listBoxUploadDirectory_SelectedIndexChanged);
            // 
            // groupBoxLocalDirectory
            // 
            this.groupBoxLocalDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxLocalDirectory.Controls.Add(this.listBoxLocalDirectory);
            this.groupBoxLocalDirectory.Location = new System.Drawing.Point(212, 6);
            this.groupBoxLocalDirectory.Name = "groupBoxLocalDirectory";
            this.groupBoxLocalDirectory.Size = new System.Drawing.Size(460, 515);
            this.groupBoxLocalDirectory.TabIndex = 4;
            this.groupBoxLocalDirectory.TabStop = false;
            this.groupBoxLocalDirectory.Text = "本地目录";
            // 
            // listBoxLocalDirectory
            // 
            this.listBoxLocalDirectory.ContextMenuStrip = this.contextLocalDirectory;
            this.listBoxLocalDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLocalDirectory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxLocalDirectory.FormattingEnabled = true;
            this.listBoxLocalDirectory.HorizontalScrollbar = true;
            this.listBoxLocalDirectory.ItemHeight = 20;
            this.listBoxLocalDirectory.Location = new System.Drawing.Point(3, 17);
            this.listBoxLocalDirectory.Name = "listBoxLocalDirectory";
            this.listBoxLocalDirectory.ScrollAlwaysVisible = true;
            this.listBoxLocalDirectory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxLocalDirectory.Size = new System.Drawing.Size(454, 495);
            this.listBoxLocalDirectory.TabIndex = 3;
            this.listBoxLocalDirectory.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxLocalDirectory_DrawItem);
            this.listBoxLocalDirectory.SelectedIndexChanged += new System.EventHandler(this.listBoxLocalDirectory_SelectedIndexChanged);
            // 
            // treeCategory
            // 
            this.treeCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeCategory.ContextMenuStrip = this.contextCategory;
            this.treeCategory.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.treeCategory.Location = new System.Drawing.Point(6, 6);
            this.treeCategory.Name = "treeCategory";
            this.treeCategory.Size = new System.Drawing.Size(200, 515);
            this.treeCategory.TabIndex = 2;
            this.treeCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeCategory_AfterSelect);
            // 
            // tabResult
            // 
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Name = "tabResult";
            this.tabResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabResult.Size = new System.Drawing.Size(1186, 527);
            this.tabResult.TabIndex = 1;
            this.tabResult.Text = "上传结果";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1194, 575);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextCategory.ResumeLayout(false);
            this.contextLocalDirectory.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabUpload.ResumeLayout(false);
            this.groupBoxUploadDirectory.ResumeLayout(false);
            this.groupBoxLocalDirectory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ContextMenuStrip contextCategory;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem ContextMenuLocalDirectoryBind;
        private System.Windows.Forms.ToolStripMenuItem ContextItemCategoryRefresh;
        private System.Windows.Forms.ContextMenuStrip contextLocalDirectory;
        private System.Windows.Forms.ToolStripMenuItem contextItemSetCanUpload;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabUpload;
        private System.Windows.Forms.GroupBox groupBoxUploadDirectory;
        private System.Windows.Forms.ListBox listBoxUploadDirectory;
        private System.Windows.Forms.GroupBox groupBoxLocalDirectory;
        private System.Windows.Forms.ListBox listBoxLocalDirectory;
        private System.Windows.Forms.TreeView treeCategory;
        private System.Windows.Forms.TabPage tabResult;
        private System.Windows.Forms.Button btnUploadDirectoryRemove;
        private System.Windows.Forms.Button btnUploadDirectoryAdd;
        
    }
}

