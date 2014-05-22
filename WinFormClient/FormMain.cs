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
            _bllUpload.Stop();
            (_proxy as ICommunicationObject).Close();          
        }

        #region 初始化

        private void Init()
        {
            try
            {
                ClientConfig.Init();

                NetTcpBinding binding = new NetTcpBinding();
                binding.TransferMode = TransferMode.Streamed;
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
                DataTableResponse response = _proxy.GetFileServerInfoByEndPoint(ClientConfig.Token, ClientConfig.WCFAddress, ClientConfig.Account, ClientConfig.PassWord);
                DataTable dt = response.DataTable;

                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new Exception("加载文件服务器信息失败!");
                }

                _fileServerInfo = new FileServerInfo();
                _fileServerInfo.Id = Convert.ToInt32(dt.Rows[0]["FSID"]);
                _fileServerInfo.Name = dt.Rows[0]["FSName"].ToString();
                _fileServerInfo.OriginalFileServerRootDirectory = dt.Rows[0]["OrgFilePath"].ToString();
                _fileServerInfo.ThumbFileServerRootDirectory = dt.Rows[0]["ThumbFilePath"].ToString();

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
                DataTableResponse response = _proxy.GetCategoryInfo(ClientConfig.Token, 1, "account1", "pass1");
                _bllCategoryTree = new BLLCategoryTree(treeCategory);

                if (response.DataTable != null || response.DataTable.Rows.Count > 1)
                {
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
                string isExistVector = "失量图：" + (category.IsExistVector ? "需要" : "不需");
                statusLabel.Text = string.Format("{0}   {1}   {2}", typeName, isExistVideo, isExistVector);

                if (category.IsDetail)
                {
                    if (string.IsNullOrEmpty(category.LocalDirectoryPath) || !Directory.Exists(category.LocalDirectoryPath))
                    {
                        return;
                    }

                    listBoxLocalDirectory.Items.Clear();
                    foreach (string localFile in Directory.GetDirectories(category.LocalDirectoryPath))
                    {
                        FolderInfo localFolderInfo = new FolderInfo();
                        localFolderInfo.CategoryId = category.Id;
                        localFolderInfo.CategoryType = category.Type;
                        localFolderInfo.LevelPath = category.LevelPath;
                        localFolderInfo.StoreTableName = category.StoreTableName;
                        localFolderInfo.IsExistVideo = category.IsExistVideo;
                        localFolderInfo.IsExistVector = category.IsExistVector;                     
                        localFolderInfo.LocalPath = localFile;                      
                        
                        listBoxLocalDirectory.Items.Add(localFolderInfo);
                    }
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

        #endregion 分类

        #region 公共

        private void ListBoxDrawItem(DrawItemEventArgs e, string iconFilePath, string text)
        {
            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(Color.FromArgb(150, 200, 250));
            }
            else if (e.Index % 2 == 0)
            {
                myBrush = new SolidBrush(Color.FromArgb(200, 200, 200));
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
            string isExistVector = "失量图：" + (folderInfo.IsExistVector ? "需要" : "不需");
            string checkResult = string.IsNullOrEmpty(folderInfo.CheckResult) ? string.Empty : "检查结果：" + folderInfo.CheckResult;
            statusLabel.Text = string.Format("{0}   {1}   {2}   {3}", typeName, isExistVideo, isExistVector, checkResult);
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

                if(!string.IsNullOrEmpty(folderInfo.CheckResult))
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

        private void btnUploadDirectoryAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxLocalDirectory.SelectedItems.Count == 0)
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

        private void btnUploadDirectoryRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxUploadDirectory.SelectedItems.Count == 0)
                {
                    return;
                }

                for (int index = listBoxUploadDirectory.Items.Count - 1; index >= 0; index--)
                {
                    if (listBoxUploadDirectory.GetSelected(index))
                    {
                        listBoxUploadDirectory.Items.RemoveAt(index);
                    }
                }
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

        private void contextItemLocalDirectoryOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxLocalDirectory.SelectedItems.Count != 1)
                {
                    MessageBox.Show("请选择一个目录!");
                    return;
                }

                FolderInfo folderInfo = listBoxLocalDirectory.SelectedItem as FolderInfo;
                System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                processStartInfo.Arguments = "/e," + folderInfo.LocalPath;
                System.Diagnostics.Process.Start(processStartInfo);
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

        #endregion 上传结果     

    

    
      
    }
}
