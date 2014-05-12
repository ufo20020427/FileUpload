using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private  ChannelFactory<IFileUpload> channelFactory;
        private  IFileUpload _proxy;

        private TreeNode _nodeRoot;
        private string _rootNodeText = "菜单";
        private int _indexImageRoot = 0;
        private int _indexImageFolder = 1;
        private int _indexImageFolderOpen = 2;
        private int _indexImagePage = 3;

        public FormMain()
        {
            InitializeComponent();
            Init();
            CategoryLoad();
        }

        private void Init()
        {
            try
            {
                ClientConfig.Init();

                NetTcpBinding binding = new NetTcpBinding();
                binding.TransferMode = TransferMode.Streamed;
                channelFactory = new ChannelFactory<IFileUpload>(binding, ClientConfig.WCFAddress);
                _proxy = channelFactory.CreateChannel();
                (_proxy as ICommunicationObject).Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

        private void CategoryLoad()
        {
            try
            {
                treeMenu.HideSelection = false;

                ImageList imageList = new ImageList();
                imageList.Images.Add(Image.FromFile("Images/root.gif"));
                imageList.Images.Add(Image.FromFile("Images/folder.gif"));
                imageList.Images.Add(Image.FromFile("Images/folderopen.gif"));
                imageList.Images.Add(Image.FromFile("Images/page.gif"));
                treeMenu.ImageList = imageList;

                _nodeRoot = new TreeNode();
                _nodeRoot.ImageIndex = _indexImageRoot;
                _nodeRoot.SelectedImageIndex = _indexImageRoot;
                _nodeRoot.Text = _rootNodeText;
                treeMenu.Nodes.Add(_nodeRoot);

                DataTableResponseMessage response = _proxy.GetCategoryInfo(ClientConfig.Token, 1, "account1", "pass1");
              
                DataTable dt = response.DataTable;

                int indexId = dt.Columns.IndexOf("CID");
                int indexParentId = dt.Columns.IndexOf("FID");
                int indexName = dt.Columns.IndexOf("CName");
                int indexIsDetail = dt.Columns.IndexOf("IsDetail");
                GreateMenu(indexId, indexParentId, indexName, indexIsDetail, dt, "0", _nodeRoot);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

        private void GreateMenu(int indexId, int indexParentId, int indexName, int indexIsDetail, DataTable dt, string parentId, TreeNode parentNode)
        {
            DataView dv = new DataView(dt);

            dv.RowFilter = "FID = '" + parentId + "'";

            foreach (DataRowView drv in dv)
            {
                int colId = Convert.ToInt32(drv[indexId]);
                int colParentId = Convert.ToInt32(drv[indexParentId]);
                string colName = drv[indexName].ToString();
                bool colIsDetail = Convert.ToBoolean(drv[indexIsDetail]);

                Category category = new Category();
                category.Id = colId;
                category.ParentId = colParentId;
                category.Name = colName;
                category.IsDetail = colIsDetail;

                TreeNode newTreeNode = new TreeNode();
                newTreeNode.Text = colName;
                newTreeNode.Tag = category;
                newTreeNode.ImageIndex = category.IsDetail ? _indexImagePage : _indexImageFolder;
                newTreeNode.SelectedImageIndex = category.IsDetail ? _indexImagePage : _indexImageFolder;
                parentNode.Nodes.Add(newTreeNode);

                GreateMenu(indexId, indexParentId, indexName, indexIsDetail, dt, colId.ToString(), newTreeNode);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            (_proxy as ICommunicationObject).Close();
        }
    }
}
