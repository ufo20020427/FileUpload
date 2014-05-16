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
            this.treeCategory = new System.Windows.Forms.TreeView();
            this.contextCategory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenuLocalDirectoryBind = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextItemCategoryRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxLocalDirectory = new System.Windows.Forms.GroupBox();
            this.listBoxLocalDirectory = new WinFormClient.ListBoxDirectory();
            this.statusStrip.SuspendLayout();
            this.contextCategory.SuspendLayout();
            this.groupBoxLocalDirectory.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 453);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(794, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // treeCategory
            // 
            this.treeCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeCategory.ContextMenuStrip = this.contextCategory;
            this.treeCategory.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.treeCategory.Location = new System.Drawing.Point(0, 0);
            this.treeCategory.Name = "treeCategory";
            this.treeCategory.Size = new System.Drawing.Size(219, 453);
            this.treeCategory.TabIndex = 1;
            this.treeCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeCategory_AfterSelect);
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
            // groupBoxLocalDirectory
            // 
            this.groupBoxLocalDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLocalDirectory.Controls.Add(this.listBoxLocalDirectory);
            this.groupBoxLocalDirectory.Location = new System.Drawing.Point(227, 4);
            this.groupBoxLocalDirectory.Name = "groupBoxLocalDirectory";
            this.groupBoxLocalDirectory.Size = new System.Drawing.Size(555, 207);
            this.groupBoxLocalDirectory.TabIndex = 3;
            this.groupBoxLocalDirectory.TabStop = false;
            this.groupBoxLocalDirectory.Text = "本地目录";
            // 
            // listBoxLocalDirectory
            // 
            this.listBoxLocalDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLocalDirectory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxLocalDirectory.FormattingEnabled = true;
            this.listBoxLocalDirectory.ItemHeight = 20;
            this.listBoxLocalDirectory.Location = new System.Drawing.Point(12, 20);
            this.listBoxLocalDirectory.Name = "listBoxLocalDirectory";
            this.listBoxLocalDirectory.ScrollAlwaysVisible = true;
            this.listBoxLocalDirectory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxLocalDirectory.Size = new System.Drawing.Size(472, 164);
            this.listBoxLocalDirectory.TabIndex = 3;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(794, 475);
            this.Controls.Add(this.groupBoxLocalDirectory);
            this.Controls.Add(this.treeCategory);
            this.Controls.Add(this.statusStrip);
            this.Name = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextCategory.ResumeLayout(false);
            this.groupBoxLocalDirectory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.TreeView treeCategory;
        private System.Windows.Forms.ContextMenuStrip contextCategory;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem ContextMenuLocalDirectoryBind;
        private System.Windows.Forms.ToolStripMenuItem ContextItemCategoryRefresh;
        private System.Windows.Forms.GroupBox groupBoxLocalDirectory;   
        private ListBoxDirectory listBoxLocalDirectory;
        
    }
}

