using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BLLClient;
using Common;
using Model;

namespace WinFormClient
{
    public partial class FormMain : Form
    {
        private IFileUpload _proxy;
        private FileServerInfo _fileServerInfo;
        private BLLCategoryTree _bllCategoryTree;
        private BLLUpload _bllUpload;
        private static Mutex mutexRun;

        #region 窗体

        public FormMain()
        {
            InitializeComponent();

            try
            {
                Init();
                FileServerInfoLoad();
                CategoryTreeLoad();
                UploadProcess();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("您确定要关闭程序？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            _bllUpload.Stop();
            (_proxy as ICommunicationObject).Close();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon.Visible = false;
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon.Visible = true;
            }
        }

        static void ProgramRepeatCheck()
        {
            bool noRun = false;
            mutexRun = new Mutex(true, ClientConfig.WCFAddress, out noRun);

            if (noRun)
            {
                mutexRun.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("请不要重复启动程序!");
                Environment.Exit(0);
            }
        }

        #endregion 窗体

        #region 初始化

        private void Init()
        {
            try
            {
                ClientConfig.Init();
                ProgramRepeatCheck();

                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.TransferMode = TransferMode.Streamed;
                binding.SendTimeout = new TimeSpan(0, ClientConfig.SendTimeout, 0);   

                ChannelFactory<IFileUpload> channelFactory = new ChannelFactory<IFileUpload>(binding, ClientConfig.WCFAddress);
                _proxy = channelFactory.CreateChannel();
                (_proxy as ICommunicationObject).Open();
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                throw new Exception("连接WCF服务端失败:" + ex.Message);
            }
        }

        private void FileServerInfoLoad()
        {
            try
            {
                BLLFileServer bllFileServer = new BLLFileServer(_proxy);
                _fileServerInfo = bllFileServer.FileServerInfoLoad();

                this.Text = string.Format("素材上传 已连上:{0}({1})", ClientConfig.WCFAddress, _fileServerInfo.Name);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                throw new Exception("加载文件服务器信息失败:" + ex.Message);
            }
        }

        private void CategoryTreeLoad()
        {
            try
            {
                DataTableResponse response = _proxy.GetCategoryInfo(ClientConfig.Token, 1, ClientConfig.Account, ClientConfig.PassWord);

                if (response.DataTable != null || response.DataTable.Rows.Count > 1)
                {
                    _bllCategoryTree = new BLLCategoryTree(treeCategory);
                    _bllCategoryTree.CategoryLoad(response.DataTable);
                }
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                throw new Exception("加载分类信息失败:" + ex.Message);
            }
        }

        private void UploadProcess()
        {
            _bllUpload = new BLLUpload(_proxy, _fileServerInfo, listBoxUploadDirectory, listBoxSucessfulDirectory, listBoxFailDirectory);
            _bllUpload.Start();
        }

        #endregion 初始化

        #region 分类

        private void treeCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode selectedNode = treeCategory.SelectedNode;
                Category category = selectedNode.Tag as Category;

                string typeName = "类型：" + (category.Type == CategoryType.Picture ? "图片" : "相册");
                string isExistVideo = "视频：" + (category.IsExistVideo ? "需要" : "不需");
                string isThumbSquare = "缩略图：" + (category.IsThumbSquare ? "正方形" : "按比例");
                string localDirectoryPath = "本地目录：" + category.LocalDirectoryPath;
                statusLabel.Text = string.Format("{0}   {1}   {2}   {3}", typeName, isExistVideo, isThumbSquare, localDirectoryPath);

                if (category.IsDetail)
                {
                    if (string.IsNullOrEmpty(category.LocalDirectoryPath) || !Directory.Exists(category.LocalDirectoryPath))
                    {
                        listBoxLocalDirectory.Items.Clear();
                        return;
                    }

                    listBoxLocalDirectory.Items.Clear();
                    foreach (string localFile in Directory.GetDirectories(category.LocalDirectoryPath))
                    {
                        FolderInfo localFolderInfo = new FolderInfo();
                        localFolderInfo.CategoryId = category.Id;
                        localFolderInfo.CategoryType = category.Type;
                        localFolderInfo.LevelPath = category.LevelPath;
                        localFolderInfo.LevelCategory = category.LevelCategory.Remove(0, 1);
                        localFolderInfo.StoreTableName = category.StoreTableName;
                        localFolderInfo.IsExistVideo = category.IsExistVideo;
                        localFolderInfo.IsThumbSquare = category.IsThumbSquare;
                        localFolderInfo.LocalPath = localFile;
                        localFolderInfo.IsRunning = false;

                        listBoxLocalDirectory.Items.Add(localFolderInfo);
                    }
                }
                else
                {
                    listBoxLocalDirectory.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void ContextMenuLocalDirectoryBind_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        _bllCategoryTree.LocalDirectoryBind(treeCategory.SelectedNode, string.Empty, folderBrowserDialog.SelectedPath);
                        _bllCategoryTree.LocalDirectoryBindSave();
                        CategoryTreeLoad();
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void ContextItemCategoryRefresh_Click(object sender, EventArgs e)
        {
            CategoryTreeLoad();
        }

        private void ContextMenuLocalDirectoryOpen_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode selectedNode = treeCategory.SelectedNode;

                if (selectedNode == null)
                {
                    return;
                }

                Category category = selectedNode.Tag as Category;

                if (string.IsNullOrEmpty(category.LocalDirectoryPath) || !Directory.Exists(category.LocalDirectoryPath))
                {
                    return;
                }

                System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                processStartInfo.Arguments = "/e," + category.LocalDirectoryPath;
                System.Diagnostics.Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        #endregion 分类

        #region 公共

        private void ListBoxDrawItem(DrawItemEventArgs e, string iconFilePath, string text)
        {
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(Color.FromArgb(150, 200, 250));
            }
            else
            {
                myBrush = new SolidBrush(Color.White);
            }

            e.Graphics.FillRectangle(myBrush, e.Bounds);
            e.DrawFocusRectangle();

            Image image = Image.FromFile(iconFilePath);
            Graphics graphics = e.Graphics;
            Rectangle bounds = e.Bounds;
            Rectangle imageRect = new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height);
            Rectangle textRect = new Rectangle(imageRect.Right, bounds.Y, bounds.Width - imageRect.Right, bounds.Height);

            if (image != null)
            {
                graphics.DrawImage(image, imageRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }

            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(text, e.Font, new SolidBrush(e.ForeColor), textRect, stringFormat);
        }

        private bool IsItemExists(ListBox.ObjectCollection items, string findText)
        {
            foreach (var uploadItem in items)
            {
                FolderInfo folderInfo = uploadItem as FolderInfo;

                if (string.Compare(folderInfo.LocalPath, findText, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void FolderInfoStatusShow(object sender)
        {
            if ((sender as ListBox).SelectedItem == null)
            {
                return;
            }

            FolderInfo folderInfo = (sender as ListBox).SelectedItem as FolderInfo;

            string typeName = "类型：" + (folderInfo.CategoryType == CategoryType.Picture ? "图片" : "相册");
            string isExistVideo = "视频：" + (folderInfo.IsExistVideo ? "需要" : "不需");
            string isThumbSquare = "缩略图：" + (folderInfo.IsThumbSquare ? "正方形" : "按比例");       
            string checkResult = string.IsNullOrEmpty(folderInfo.CheckResult) ? string.Empty : "检查结果：" + folderInfo.CheckResult;
            string waitUploadFilesCount = "等待上传文件数：" + folderInfo.WaitUploadFilesCount.ToString();
            string sucessfulUploadFilesCount = "成功上传文件数：" + folderInfo.SucessfulUploadFilesCount.ToString();
            statusLabel.Text = string.Format("{0}   {1}   {2}   {3}   {4}   {5}", typeName, isExistVideo, isThumbSquare, checkResult, waitUploadFilesCount, sucessfulUploadFilesCount);
        }

        private void ListBoxSelectedItemsRemove(ListBox listBox)
        {
            if (listBox.SelectedItems.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("确定移除选中目录？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            for (int index = listBox.Items.Count - 1; index >= 0; index--)
            {
                FolderInfo folderInfo = listBox.Items[index] as FolderInfo;
                if (listBox.GetSelected(index) && !folderInfo.IsRunning)
                {
                    listBox.Items.RemoveAt(index);
                }
            }
        }

        private void DirectoryOpen(ListBox listBox)
        {
            if (listBox.SelectedItems.Count != 1)
            {
                MessageBox.Show("只能选中一个目录！");
                return;
            }

            FolderInfo folderInfo = listBox.SelectedItem as FolderInfo;
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            processStartInfo.Arguments = "/e," + folderInfo.LocalPath;
            System.Diagnostics.Process.Start(processStartInfo);
        }

        #endregion 公共

        #region 本地目录|上传目录

        private void listBoxLocalDirectory_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index < 0)
                {
                    return;
                }

                FolderInfo folderInfo = (sender as ListBox).Items[e.Index] as FolderInfo;

                if (!string.IsNullOrEmpty(folderInfo.CheckResult))
                {
                    ListBoxDrawItem(e, "Images/CheckFail.ico", folderInfo.LocalPath);
                    return;
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(folderInfo.LocalPath);
                if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    ListBoxDrawItem(e, "Images/SucessfulDirectory.ico", folderInfo.LocalPath);
                }
                else
                {
                    ListBoxDrawItem(e, "Images/UploadDirectory.ico", folderInfo.LocalPath);
                }
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxUploadDirectory_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index < 0)
                {
                    return;
                }

                FolderInfo folderInfo = (sender as ListBox).Items[e.Index] as FolderInfo;
                ListBoxDrawItem(e, "Images/UploadDirectory.ico", folderInfo.LocalPath);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxLocalDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FolderInfoStatusShow(sender);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxUploadDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FolderInfoStatusShow(sender);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemSetCanUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxLocalDirectory.SelectedItems.Count == 0)
                {
                    return;
                }

                if (MessageBox.Show("确定把已上传目录重置为允许上传？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                foreach (var item in listBoxLocalDirectory.SelectedItems)
                {
                    FolderInfo folderInfo = item as FolderInfo;
                    DirectoryInfo directoryInfo = new DirectoryInfo(folderInfo.LocalPath);
                    if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        directoryInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemFileSetCanUpload_Click(object sender, EventArgs e)
        {
            try
            {

                if (listBoxLocalDirectory.SelectedItems.Count == 0)
                {
                    return;
                }

                if (MessageBox.Show("确定把已上传目录及其下所有文件重置为允许上传？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                foreach (var item in listBoxLocalDirectory.SelectedItems)
                {
                    FolderInfo folderInfo = item as FolderInfo;
                    DirectoryInfo directoryInfo = new DirectoryInfo(folderInfo.LocalPath);
                    if ((directoryInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        directoryInfo.Attributes = FileAttributes.Normal;
                    }

                    foreach (string file in Directory.GetFiles(folderInfo.LocalPath))
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemLocalDirectoryOpen_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryOpen(listBoxLocalDirectory);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemUploadDirectoryAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxLocalDirectory.SelectedItems.Count == 0)
                {
                    return;
                }

                if (MessageBox.Show("确定上传选中目录？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                for (int index = listBoxLocalDirectory.Items.Count - 1; index >= 0; index--)
                {
                    if (!listBoxLocalDirectory.GetSelected(index))
                    {
                        continue;
                    }

                    FolderInfo localFolderInfo = listBoxLocalDirectory.Items[index] as FolderInfo;

                    FileAttributes attributes = File.GetAttributes(localFolderInfo.LocalPath);
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        MessageBox.Show("该目录已上传过，如要重新上传，请执行“目录重置为可上传”");
                        continue;
                    }

                    if (IsItemExists(listBoxUploadDirectory.Items, localFolderInfo.LocalPath))
                    {
                        continue;
                    }

                    localFolderInfo.CheckResult = string.Empty;
                    _bllUpload.UploadDirectoryCheck(ref localFolderInfo);
                    if (!string.IsNullOrEmpty(localFolderInfo.CheckResult))
                    {
                        continue;
                    }

                    listBoxUploadDirectory.Items.Add(localFolderInfo);
                    listBoxLocalDirectory.Items.RemoveAt(index);
                }

                listBoxLocalDirectory.Invalidate();
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemUploadDirectoryRemove_Click(object sender, EventArgs e)
        {
            try
            {
                ListBoxSelectedItemsRemove(listBoxUploadDirectory);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        #endregion 本地目录|上传目录

        #region 上传结果

        private void listBoxSucessfulDirectory_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index < 0)
                {
                    return;
                }

                FolderInfo folderInfo = (sender as ListBox).Items[e.Index] as FolderInfo;
                ListBoxDrawItem(e, "Images/SucessfulDirectory.ico", folderInfo.LocalPath);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxFailDirectory_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index < 0)
                {
                    return;
                }

                FolderInfo folderInfo = (sender as ListBox).Items[e.Index] as FolderInfo;
                ListBoxDrawItem(e, "Images/FailDirectory.ico", folderInfo.LocalPath);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxSucessfulDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FolderInfoStatusShow(sender);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void listBoxFailDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as ListBox).SelectedItem == null)
                {
                    return;
                }

                FolderInfoStatusShow(sender);

                FolderInfo folderInfo = (sender as ListBox).SelectedItem as FolderInfo;

                textBoxFailDetail.Clear();
                textBoxFailDetail.Text = folderInfo.UploadResult.ToString();
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemSucessfulDirectoryRemove_Click(object sender, EventArgs e)
        {
            try
            {
                ListBoxSelectedItemsRemove(listBoxSucessfulDirectory);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemFailDirectoryRemove_Click(object sender, EventArgs e)
        {
            try
            {
                ListBoxSelectedItemsRemove(listBoxFailDirectory);
                textBoxFailDetail.Clear();
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemFailDirectoryReUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxFailDirectory.SelectedItems.Count == 0)
                {
                    return;
                }

                if (MessageBox.Show("确定上传选中目录？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                for (int index = listBoxFailDirectory.Items.Count - 1; index >= 0; index--)
                {
                    if (!listBoxFailDirectory.GetSelected(index))
                    {
                        continue;
                    }

                    FolderInfo failFolderInfo = listBoxFailDirectory.Items[index] as FolderInfo;

                    if (IsItemExists(listBoxUploadDirectory.Items, failFolderInfo.LocalPath))
                    {
                        continue;
                    }

                    failFolderInfo.CheckResult = string.Empty;
                    _bllUpload.UploadDirectoryCheck(ref failFolderInfo);
                    if (!string.IsNullOrEmpty(failFolderInfo.CheckResult))
                    {
                        continue;
                    }

                    listBoxUploadDirectory.Items.Add(failFolderInfo);
                    listBoxFailDirectory.Items.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void contextItemFailDirectoryOpen_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryOpen(listBoxFailDirectory);
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        #endregion 上传结果

    }
}
