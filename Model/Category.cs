using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public enum CategoryType
    {
        Picture = 1,
        Gallery = 2
    }

    public class Category
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
        public int ParentId { get; set; }
        public CategoryType Type { get; set; }
        public bool IsExistVideo { get; set; }
        public bool IsExistVector { get; set; }
        public string StoreTableName { get; set; }
        public bool IsDetail { get; set; }

        public string LevelPath { get; set; }
        public string LevelCategory { get; set; }
        public string LocalDirectoryPath { get; set; }
    }
}
