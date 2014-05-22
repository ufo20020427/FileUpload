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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
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
            this.groupBoxFailFile = new System.Windows.Forms.GroupBox();
            this.textBoxFailDetail = new System.Windows.Forms.TextBox();
            this.groupBoxFailDirectory = new System.Windows.Forms.GroupBox();
            this.listBoxFailDirectory = new System.Windows.Forms.ListBox();
            this.groupBoxSucessfulDirectory = new System.Windows.Forms.GroupBox();
            this.listBoxSucessfulDirectory = new System.Windows.Forms.ListBox();
            this.contextItemLocalDirectoryOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.contextCategory.SuspendLayout();
            this.contextLocalDirectory.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabUpload.SuspendLayout();
            this.groupBoxUploadDirectory.SuspendLayout();
            this.groupBoxLocalDirectory.SuspendLayout();
            this.tabResult.SuspendLayout();
            this.groupBoxFailFile.SuspendLayout();
            this.groupBoxFailDirectory.SuspendLayout();
            this.groupBoxSucessfulDirectory.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 653);
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
            this.contextItemSetCanUpload,
            this.contextItemLocalDirectoryOpen});
            this.contextLocalDirectory.Name = "contextLocalDirectory";
            this.contextLocalDirectory.Size = new System.Drawing.Size(153, 70);
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
            this.tabs.Size = new System.Drawing.Size(1194, 653);
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
            this.tabUpload.Size = new System.Drawing.Size(1186, 627);
            this.tabUpload.TabIndex = 0;
            this.tabUpload.Text = "文件上传";
            this.tabUpload.UseVisualStyleBackColor = true;
            // 
            // btnUploadDirectoryRemove
            // 
            this.btnUploadDirectoryRemove.Location = new System.Drawing.Point(682, 213);
            this.btnUploadDirectoryRemove.Name = "btnUploadDirectoryRemove";
            this.btnUploadDirectoryRemove.Size = new System.Drawing.Size(30, 23);
            this.btnUploadDirectoryRemove.TabIndex = 8;
            this.btnUploadDirectoryRemove.Text = "<<";
            this.btnUploadDirectoryRemove.UseVisualStyleBackColor = true;
            this.btnUploadDirectoryRemove.Click += new System.EventHandler(this.btnUploadDirectoryRemove_Click);
            // 
            // btnUploadDirectoryAdd
            // 
            this.btnUploadDirectoryAdd.Location = new System.Drawing.Point(682, 164);
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
            this.groupBoxUploadDirectory.Location = new System.Drawing.Point(721, 6);
            this.groupBoxUploadDirectory.Name = "groupBoxUploadDirectory";
            this.groupBoxUploadDirectory.Size = new System.Drawing.Size(462, 615);
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
            this.listBoxUploadDirectory.Size = new System.Drawing.Size(456, 595);
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
            this.groupBoxLocalDirectory.Size = new System.Drawing.Size(460, 615);
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
            this.listBoxLocalDirectory.Size = new System.Drawing.Size(454, 595);
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
            this.treeCategory.Size = new System.Drawing.Size(200, 615);
            this.treeCategory.TabIndex = 2;
            this.treeCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeCategory_AfterSelect);
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.groupBoxFailFile);
            this.tabResult.Controls.Add(this.groupBoxFailDirectory);
            this.tabResult.Controls.Add(this.groupBoxSucessfulDirectory);
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Name = "tabResult";
            this.tabResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabResult.Size = new System.Drawing.Size(1186, 627);
            this.tabResult.TabIndex = 1;
            this.tabResult.Text = "上传结果";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // groupBoxFailFile
            // 
            this.groupBoxFailFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBoxFailFile.Controls.Add(this.textBoxFailDetail);
            this.groupBoxFailFile.Location = new System.Drawing.Point(601, 319);
            this.groupBoxFailFile.Name = "groupBoxFailFile";
            this.groupBoxFailFile.Size = new System.Drawing.Size(582, 302);
            this.groupBoxFailFile.TabIndex = 7;
            this.groupBoxFailFile.TabStop = false;
            this.groupBoxFailFile.Text = "上传失败明细";
            // 
            // textBoxFailDetail
            // 
            this.textBoxFailDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFailDetail.Location = new System.Drawing.Point(3, 17);
            this.textBoxFailDetail.Multiline = true;
            this.textBoxFailDetail.Name = "textBoxFailDetail";
            this.textBoxFailDetail.Size = new System.Drawing.Size(576, 282);
            this.textBoxFailDetail.TabIndex = 0;
            // 
            // groupBoxFailDirectory
            // 
            this.groupBoxFailDirectory.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBoxFailDirectory.Controls.Add(this.listBoxFailDirectory);
            this.groupBoxFailDirectory.Location = new System.Drawing.Point(601, 6);
            this.groupBoxFailDirectory.Name = "groupBoxFailDirectory";
            this.groupBoxFailDirectory.Size = new System.Drawing.Size(582, 300);
            this.groupBoxFailDirectory.TabIndex = 6;
            this.groupBoxFailDirectory.TabStop = false;
            this.groupBoxFailDirectory.Text = "上传失败目录";
            // 
            // listBoxFailDirectory
            // 
            this.listBoxFailDirectory.ContextMenuStrip = this.contextLocalDirectory;
            this.listBoxFailDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFailDirectory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxFailDirectory.FormattingEnabled = true;
            this.listBoxFailDirectory.HorizontalScrollbar = true;
            this.listBoxFailDirectory.ItemHeight = 20;
            this.listBoxFailDirectory.Location = new System.Drawing.Point(3, 17);
            this.listBoxFailDirectory.Name = "listBoxFailDirectory";
            this.listBoxFailDirectory.ScrollAlwaysVisible = true;
            this.listBoxFailDirectory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxFailDirectory.Size = new System.Drawing.Size(576, 280);
            this.listBoxFailDirectory.TabIndex = 3;
            this.listBoxFailDirectory.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxFailDirectory_DrawItem);
            this.listBoxFailDirectory.SelectedIndexChanged += new System.EventHandler(this.listBoxFailDirectory_SelectedIndexChanged);
            // 
            // groupBoxSucessfulDirectory
            // 
            this.groupBoxSucessfulDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSucessfulDirectory.Controls.Add(this.listBoxSucessfulDirectory);
            this.groupBoxSucessfulDirectory.Location = new System.Drawing.Point(8, 6);
            this.groupBoxSucessfulDirectory.Name = "groupBoxSucessfulDirectory";
            this.groupBoxSucessfulDirectory.Size = new System.Drawing.Size(587, 615);
            this.groupBoxSucessfulDirectory.TabIndex = 5;
            this.groupBoxSucessfulDirectory.TabStop = false;
            this.groupBoxSucessfulDirectory.Text = "上传成功目录";
            // 
            // listBoxSucessfulDirectory
            // 
            this.listBoxSucessfulDirectory.ContextMenuStrip = this.contextLocalDirectory;
            this.listBoxSucessfulDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSucessfulDirectory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxSucessfulDirectory.FormattingEnabled = true;
            this.listBoxSucessfulDirectory.HorizontalScrollbar = true;
            this.listBoxSucessfulDirectory.ItemHeight = 20;
            this.listBoxSucessfulDirectory.Location = new System.Drawing.Point(3, 17);
            this.listBoxSucessfulDirectory.Name = "listBoxSucessfulDirectory";
            this.listBoxSucessfulDirectory.ScrollAlwaysVisible = true;
            this.listBoxSucessfulDirectory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSucessfulDirectory.Size = new System.Drawing.Size(581, 595);
            this.listBoxSucessfulDirectory.TabIndex = 3;
            this.listBoxSucessfulDirectory.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxSucessfulDirectory_DrawItem);
            this.listBoxSucessfulDirectory.SelectedIndexChanged += new System.EventHandler(this.listBoxSucessfulDirectory_SelectedIndexChanged);
            // 
            // contextItemLocalDirectoryOpen
            // 
            this.contextItemLocalDirectoryOpen.Name = "contextItemLocalDirectoryOpen";
            this.contextItemLocalDirectoryOpen.Size = new System.Drawing.Size(152, 22);
            this.contextItemLocalDirectoryOpen.Text = "打开目录";
            this.contextItemLocalDirectoryOpen.Click += new System.EventHandler(this.contextItemLocalDirectoryOpen_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1194, 675);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.statusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.tabResult.ResumeLayout(false);
            this.groupBoxFailFile.ResumeLayout(false);
            this.groupBoxFailFile.PerformLayout();
            this.groupBoxFailDirectory.ResumeLayout(false);
            this.groupBoxSucessfulDirectory.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBoxSucessfulDirectory;
        private System.Windows.Forms.ListBox listBoxSucessfulDirectory;
        private System.Windows.Forms.GroupBox groupBoxFailDirectory;
        private System.Windows.Forms.ListBox listBoxFailDirectory;
        private System.Windows.Forms.GroupBox groupBoxFailFile;
        private System.Windows.Forms.TextBox textBoxFailDetail;
        private System.Windows.Forms.ToolStripMenuItem contextItemLocalDirectoryOpen;
        
    }
}

