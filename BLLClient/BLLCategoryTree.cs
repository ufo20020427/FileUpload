using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Model;

namespace BLLClient
{
    public class BLLCategoryTree
    {
        private TreeNode _nodeRoot;
        private int _indexImageRoot = 0;
        private int _indexImageFolder = 1;
        private int _indexImageFolderOpen = 2;
        private int _indexImagePage = 3;

        private int _indexId;
        private int _indexParentId;
        private int _indexName;
        private int _indexIsDetail;

        public BLLCategoryTree(TreeView treeCategory)
        {
            ImageList imageList = new ImageList();
            imageList.Images.Add(Image.FromFile("Images/root.gif"));
            imageList.Images.Add(Image.FromFile("Images/folder.gif"));
            imageList.Images.Add(Image.FromFile("Images/folderopen.gif"));
            imageList.Images.Add(Image.FromFile("Images/page.gif"));
            treeCategory.ImageList = imageList;
            treeCategory.HideSelection = false;

            _nodeRoot = new TreeNode();
            _nodeRoot.ImageIndex = _indexImageRoot;
            _nodeRoot.SelectedImageIndex = _indexImageRoot;
            _nodeRoot.Text = "菜单";
            treeCategory.Nodes.Add(_nodeRoot);
        }

        public void CategoryLoad(DataTable dt)
        {
            _indexId = dt.Columns.IndexOf("CID");
            _indexParentId = dt.Columns.IndexOf("FID");
            _indexName = dt.Columns.IndexOf("CName");
            _indexIsDetail = dt.Columns.IndexOf("IsDetail");
            GreateTree(dt, "0", _nodeRoot);
        }

        private void GreateTree(DataTable dt, string parentId, TreeNode parentNode)
        {
            DataView dv = new DataView(dt);

            dv.RowFilter = "FID = '" + parentId + "'";

            foreach (DataRowView drv in dv)
            {
                int colId = Convert.ToInt32(drv[_indexId]);
                int colParentId = Convert.ToInt32(drv[_indexParentId]);
                string colName = drv[_indexName].ToString();
                bool colIsDetail = Convert.ToBoolean(drv[_indexIsDetail]);

                Category category = new Category();
                category.Id = colId;
                category.ParentId = colParentId;
                category.Name = colName;
                category.IsDetail = colIsDetail;

                TreeNode newTreeNode = new TreeNode();
                newTreeNode.Text = colName;
                newTreeNode.Tag = category;
                newTreeNode.ImageIndex = category.IsDetail ? _indexImagePage : _indexImageFolder;
                newTreeNode.SelectedImageIndex = category.IsDetail ? _indexImagePage : _indexImageFolderOpen;
                parentNode.Nodes.Add(newTreeNode);

                GreateTree(dt, colId.ToString(), newTreeNode);
            }
        }

    }
}
