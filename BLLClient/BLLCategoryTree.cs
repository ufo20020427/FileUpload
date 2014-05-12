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
        private TreeView _treeCategory;       

        private int _indexImageRoot = 0;
        private int _indexImagePage = 1;
        private int _indexImageFolderPicture = 2;
        private int _indexImageFolderGallery = 3;  

        private int _indexId;
        private int _indexParentId;
        private int _indexName;
        private int _indexIsDetail;
        private int _indexType;

        public BLLCategoryTree(TreeView treeCategory)
        {
            _treeCategory = treeCategory;

            ImageList imageList = new ImageList();
            imageList.Images.Add(Image.FromFile("Images/root.gif"));
            imageList.Images.Add(Image.FromFile("Images/page.gif"));
            imageList.Images.Add(Image.FromFile("Images/FolderPicture.ico"));
            imageList.Images.Add(Image.FromFile("Images/FolderGallery.ico"));

            _treeCategory.ImageList = imageList;
            _treeCategory.HideSelection = false;          
        }

        public void CategoryLoad(DataTable dt)
        {
            _indexId = dt.Columns.IndexOf("CID");
            _indexParentId = dt.Columns.IndexOf("FID");
            _indexName = dt.Columns.IndexOf("CName");
            _indexIsDetail = dt.Columns.IndexOf("IsDetail");
            _indexType = dt.Columns.IndexOf("CType");

            TreeNode nodeRoot = new TreeNode();
            nodeRoot.ImageIndex = _indexImageRoot;
            nodeRoot.SelectedImageIndex = _indexImageRoot;
            nodeRoot.Text = "目录";
            nodeRoot.Expand();
            _treeCategory.Nodes.Add(nodeRoot);

            GreateTree(dt, "0", nodeRoot);
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
                byte colType = Convert.ToByte(drv[_indexType]);

                Category category = new Category();
                category.Id = colId;
                category.ParentId = colParentId;
                category.Name = colName;
                category.IsDetail = colIsDetail;

                TreeNode newTreeNode = new TreeNode();
                newTreeNode.Text = colName;
                newTreeNode.Tag = category;

                int indexImage = 0;
                if (colIsDetail)
                {
                     indexImage = _indexImagePage;                    
                }
                else if (colType == 1)
                {
                    indexImage = _indexImageFolderPicture;
                }
                else
                {
                    indexImage = _indexImageFolderGallery;
                }

                newTreeNode.ImageIndex =  indexImage;
                newTreeNode.SelectedImageIndex = indexImage;
             
                parentNode.Nodes.Add(newTreeNode);

                GreateTree(dt, colId.ToString(), newTreeNode);
            }
        }

    }
}
