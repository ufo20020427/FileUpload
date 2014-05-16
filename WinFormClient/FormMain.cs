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

        private Color RowBackColorSel = Color.FromArgb(150, 200, 250);

        public FormMain()
        {
            InitializeComponent();

            try
            {
                Init();
                FileServerInfoLoad();
                CategoryTreeLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }

        private void Init()
        {
            try
            {
                listBoxLocalDirectory.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxLocalDirectory.ItemHeight = 20;

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

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            (_proxy as ICommunicationObject).Close();
        }

        private void treeCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode selectedNode = treeCategory.SelectedNode;
                Category category = selectedNode.Tag as Category;
                statusLabel.Text = "本地目录：" + category.LocalDirectoryPath;

                if (category.IsDetail)
                {
                    List<LocalDirectory> listLocalDirectory = new List<LocalDirectory>();

                    foreach (string file in Directory.GetDirectories(category.LocalDirectoryPath))
                    {
                        LocalDirectory localDirectory = new LocalDirectory();
                        localDirectory.FileName = Path.GetFileName(file);
                        localDirectory.FilePath = file;
                        listLocalDirectory.Add(localDirectory);
                    }

                    listBoxLocalDirectory.DisplayMember = "FileName";
                    listBoxLocalDirectory.ValueMember = "FilePath";
                    listBoxLocalDirectory.DataSource = listLocalDirectory;
                    listBoxLocalDirectory.ClearSelected();
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

        private void listBoxLocalDirectory_DrawItem(object sender, DrawItemEventArgs e)
        {
            LocalDirectory localDirectory = listBoxLocalDirectory.Items[e.Index] as LocalDirectory;

            Brush myBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(RowBackColorSel);
            }
            else
            {
                myBrush = new SolidBrush(Color.White);
            }

            e.Graphics.FillRectangle(myBrush, e.Bounds);
            e.DrawFocusRectangle();

            Image image = Image.FromFile("images/root.gif");
            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;
            Rectangle imageRect = new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height);
            Rectangle textRect = new Rectangle(imageRect.Right, bounds.Y, bounds.Width - imageRect.Right, bounds.Height);

            if (image != null)
            {
                g.DrawImage(image, imageRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }

            StringFormat strFormat = new StringFormat();
            strFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(localDirectory.FileName, e.Font, new SolidBrush(e.ForeColor), textRect, strFormat);

        }

    }
}
