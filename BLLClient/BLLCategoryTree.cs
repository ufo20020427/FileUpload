using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Model;

namespace BLLClient
{
    public enum ImageIndex
    {
        Root = 0,
        Picture = 1,
        PictureDetail = 2,
        Gallery = 3,
        GalleryDetail = 4,
        UnKnowType = 5,
        NoBind = 6
    }

    public class BLLCategoryTree
    {
        private TreeView _treeCategory;
        private string _categoryMapFilePath;        

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
            imageList.ImageSize = new Size(20, 20);
         

            _treeCategory.ImageList = imageList;
            _treeCategory.HideSelection = false;

            _categoryMapFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "CategoryMap.txt");
        }

        public void CategoryLoad(DataTable dt)
        { 
            Dictionary<int, string> dicCategoryMap = new Dictionary<int, string>();

            if (File.Exists(_categoryMapFilePath))
            {
                IEnumerable<string> listCategoryMap = File.ReadLines(_categoryMapFilePath);

                string[] columns;
                foreach (string categoryMap in listCategoryMap)
                {
                    columns = categoryMap.Split('|');
                    dicCategoryMap.Add(int.Parse(columns[0]), columns[1]);
                }              
            }
            

            _indexId = dt.Columns.IndexOf("CID");
            _indexName = dt.Columns.IndexOf("CName");
            _indexFolderName = dt.Columns.IndexOf("FolderName");
            _indexParentId = dt.Columns.IndexOf("FID");
            _indexType = dt.Columns.IndexOf("CType");
            _indexIsExistVideo = dt.Columns.IndexOf("IsVideo");
            _indexIsExistVector = dt.Columns.IndexOf("IsVector");
            _indexStoreTableName = dt.Columns.IndexOf("DataTable");
            _indexIsDetail = dt.Columns.IndexOf("IsDetail");

            Category categoryRoot = new Category();
            categoryRoot.Id = 0;
            categoryRoot.LevelPath = string.Empty;
            categoryRoot.FolderName = string.Empty;

            string localDirectoryPath;
            dicCategoryMap.TryGetValue(categoryRoot.Id, out localDirectoryPath);
            categoryRoot.LocalDirectoryPath = localDirectoryPath;

            TreeNode nodeRoot = new TreeNode();
            nodeRoot.ImageIndex = (int)ImageIndex.Root;
            nodeRoot.SelectedImageIndex = (int)ImageIndex.Root;
            nodeRoot.Text = "分类目录";
            nodeRoot.Expand();
            nodeRoot.Tag = categoryRoot;
            _treeCategory.Nodes.Clear();
            _treeCategory.Nodes.Add(nodeRoot);

            GreateTree(dt, "0", nodeRoot, ref dicCategoryMap);
        }

        private void GreateTree(DataTable dt, string parentId, TreeNode parentNode, ref Dictionary<int, string> dicCategoryMap)
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
                category.Type = (CategoryType)Convert.ToByte(drv[_indexType]);
                category.IsExistVideo = Convert.ToBoolean(_indexIsExistVideo);
                category.IsExistVector = Convert.ToBoolean(_indexIsExistVector);
                category.StoreTableName = drv[_indexStoreTableName].ToString();
                category.IsDetail = Convert.ToBoolean(drv[_indexIsDetail]);
                category.LevelPath = (parentNode.Tag as Category).LevelPath + "|" + category.FolderName;

                string localDirectoryPath;
                dicCategoryMap.TryGetValue(category.Id, out localDirectoryPath);
                category.LocalDirectoryPath = localDirectoryPath;

                int indexImage = (int)ImageIndex.UnKnowType;
                if (string.IsNullOrEmpty(category.LocalDirectoryPath) || !Directory.Exists(category.LocalDirectoryPath))
                {
                    indexImage = (int)ImageIndex.NoBind;
                }
                else if (category.Type == CategoryType.Picture)
                {
                    indexImage = category.IsDetail ? (int)ImageIndex.PictureDetail : (int)ImageIndex.Picture;
                }
                else if (category.Type == CategoryType.Gallery)
                {
                    indexImage = category.IsDetail ? (int)ImageIndex.GalleryDetail : (int)ImageIndex.Gallery;
                }

                TreeNode newTreeNode = new TreeNode();
                newTreeNode.ImageIndex = indexImage;
                newTreeNode.SelectedImageIndex = indexImage;
                newTreeNode.Text = category.Name;
                newTreeNode.Tag = category;
                parentNode.Nodes.Add(newTreeNode);

                GreateTree(dt, category.Id.ToString(), newTreeNode, ref dicCategoryMap);
            }
        }

        public void LocalDirectoryBind(TreeNode selectedNode, string parentNodeLevelPath, string selectedPath)
        {
            Category category = (selectedNode.Tag as Category);         

             if (string.IsNullOrEmpty(parentNodeLevelPath))
             {            
                 category.LocalDirectoryPath = selectedPath;
             }
             else
             {
                 category.LocalDirectoryPath = parentNodeLevelPath + "\\" + category.FolderName;

                 if (!string.IsNullOrEmpty(category.LevelPath))
                 {
                     Directory.CreateDirectory(category.LocalDirectoryPath);
                 }
             }                

            foreach (TreeNode node in selectedNode.Nodes)
            {
                LocalDirectoryBind(node, category.LocalDirectoryPath, selectedPath);
            }
        }

        private void BindSave(TreeNode parentNode, ref StringBuilder sbLocalDirectoryBind)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                Category category = (node.Tag as Category);

                sbLocalDirectoryBind.AppendLine(category.Id + "|" + category.LocalDirectoryPath);

                BindSave(node, ref sbLocalDirectoryBind);
            }
        }

        public void LocalDirectoryBindSave()
        {
            StringBuilder sbLocalDirectoryBind = new StringBuilder();

            Category categoryRoot = (_treeCategory.Nodes[0].Tag as Category);
            sbLocalDirectoryBind.AppendLine(categoryRoot.Id + "|" + categoryRoot.LocalDirectoryPath);
    
            BindSave(_treeCategory.Nodes[0], ref sbLocalDirectoryBind);        
            
            File.WriteAllText(_categoryMapFilePath, sbLocalDirectoryBind.ToString());         
        }
    }
}
