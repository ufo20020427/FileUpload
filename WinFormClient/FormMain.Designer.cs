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
            this.statusStrip.SuspendLayout();
            this.contextCategory.SuspendLayout();
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
            this.treeCategory.ContextMenuStrip = this.contextCategory;
            this.treeCategory.Dock = System.Windows.Forms.DockStyle.Left;
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
            this.contextCategory.Size = new System.Drawing.Size(153, 70);
            // 
            // ContextMenuLocalDirectoryBind
            // 
            this.ContextMenuLocalDirectoryBind.Name = "ContextMenuLocalDirectoryBind";
            this.ContextMenuLocalDirectoryBind.Size = new System.Drawing.Size(152, 22);
            this.ContextMenuLocalDirectoryBind.Text = "关联本地目录";
            this.ContextMenuLocalDirectoryBind.Click += new System.EventHandler(this.ContextMenuLocalDirectoryBind_Click);
            // 
            // ContextItemCategoryRefresh
            // 
            this.ContextItemCategoryRefresh.Name = "ContextItemCategoryRefresh";
            this.ContextItemCategoryRefresh.Size = new System.Drawing.Size(152, 22);
            this.ContextItemCategoryRefresh.Text = "刷新分类";
            this.ContextItemCategoryRefresh.Click += new System.EventHandler(this.ContextItemCategoryRefresh_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(794, 475);
            this.Controls.Add(this.treeCategory);
            this.Controls.Add(this.statusStrip);
            this.Name = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.contextCategory.ResumeLayout(false);
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
    }
}

