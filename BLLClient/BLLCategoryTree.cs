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
        private int _indexImagePicture = 1;
        private int _indexImagePictureDetail = 2;
        private int _indexImageGallery = 3;
        private int _indexImageGalleryDetail = 4;
        private int _indexImageUnKnowType = 5;
        private int _indexImageNoBind = 6;

        private int _indexId;
        private int _indexName;
        private int _indexFolderName;
        private int _indexParentId;
        private int _indexType;
        private int _indexIsExistVideo;
        private int _indexIsExistVector;
        private int _indexStoreTableName;
        private int _indexIsDetail;

        public BLLCategoryTree(TreeView treeCategory)
        {
            _treeCategory = treeCategory;

            ImageList imageList = new ImageList();
            imageList.Images.Add(Image.FromFile("Images/root.gif"));
            imageList.Images.Add(Image.FromFile("Images/Picture.ico"));
            imageList.Images.Add(Image.FromFile("Images/PictureDetail.ico"));
            imageList.Images.Add(Image.FromFile("Images/Gallery.ico"));
            imageList.Images.Add(Image.FromFile("Images/GalleryDetail.ico"));
            imageList.Images.Add(Image.FromFile("Images/UnKnowType.ico"));
            imageList.Images.Add(Image.FromFile("Images/NoBind.ico"));

            _treeCategory.ImageList = imageList;
            _treeCategory.HideSelection = false;          
        }

        public void CategoryLoad(DataTable dt)
        {
            _indexId = dt.Columns.IndexOf("CID");
            _indexName = dt.Columns.IndexOf("CName");
            _indexFolderName = dt.Columns.IndexOf("FolderName");
            _indexParentId = dt.Columns.IndexOf("FID");
            _indexType = dt.Columns.IndexOf("CType");
            _indexIsExistVideo = dt.Columns.IndexOf("IsVideo");
            _indexIsExistVector = dt.Columns.IndexOf("IsVector");
            _indexStoreTableName = dt.Columns.IndexOf("DataTable");
            _indexIsDetail = dt.Columns.IndexOf("IsDetail");          

            TreeNode nodeRoot = new TreeNode();
            nodeRoot.ImageIndex = _indexImageRoot;
            nodeRoot.SelectedImageIndex = _indexImageRoot;
            nodeRoot.Text = "分类目录";
            nodeRoot.Expand();
            nodeRoot.Tag = new Category();
            _treeCategory.Nodes.Add(nodeRoot);

            GreateTree(dt, "0", nodeRoot);
        }

        private void GreateTree(DataTable dt, string parentId, TreeNode parentNode)
        {
            DataView dv = new DataView(dt);

            dv.RowFilter = "FID = '" + parentId + "'";

            foreach (DataRowView drv in dv)
            {
                Category category = new Category();
                category.Id = Convert.ToInt32(drv[_indexId]);
                category.Name = drv[_indexName].ToString();
                category.FolderName = drv[_indexFolderName].ToString(); 
                category.ParentId = Convert.ToInt32(drv[_indexParentId]); 
                category.Type = Convert.ToByte(drv[_indexType]);
                category.IsExistVideo = Convert.ToBoolean(_indexIsExistVideo);
                category.IsExistVector = Convert.ToBoolean(_indexIsExistVector);
                category.StoreTableName = drv[_indexStoreTableName].ToString();
                category.IsDetail = Convert.ToBoolean(drv[_indexIsDetail]);

                if (parentNode.Tag == null)
                {
                    category.LevelPath = category.FolderName;
                }
                else
                {
                    category.LevelPath = (parentNode.Tag as Category).LevelPath + "|" + category.FolderName;
                }

                int indexImage = 0;
                if (string.IsNullOrEmpty(category.LocalDirectoryPath))
                {
                    indexImage = _indexImageNoBind;
                }
                else if (category.Type == 1)
                {
                    indexImage = category.IsDetail ? _indexImagePictureDetail : _indexImagePicture;
                }
                else if (category.Type == 2)
                {
                    indexImage = category.IsDetail ? _indexImageGalleryDetail : _indexImageGallery;
                }
                else
                {
                    indexImage = _indexImageUnKnowType;
                }

                TreeNode newTreeNode = new TreeNode();
                newTreeNode.ImageIndex = indexImage;
                newTreeNode.SelectedImageIndex = indexImage;                
                newTreeNode.Text = category.Name;
                newTreeNode.Tag = category;     
                parentNode.Nodes.Add(newTreeNode);

                GreateTree(dt, category.Id.ToString(), newTreeNode);
            }
        }

    }
}
