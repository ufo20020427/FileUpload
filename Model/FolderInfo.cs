using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FolderInfo
    {
        public FolderInfo()
        {
            UploadResult = new StringBuilder();
        }

        public int CategoryId { get; set; }
        public CategoryType CategoryType { get; set; }
        public bool IsExistVideo { get; set; }
        public bool IsExistVector { get; set; }
        public string StoreTableName { get; set; }
        public string LocalPath { get; set; }       
        public string LevelPath { get; set; }
        public string LevelCategory { get; set; }        

        public string GalleryName { get; set; }
        public int PageCount { get; set; }
        public string Introudce { get; set; }
        public string Designer { get; set; }
        public string Address { get; set; }

        public string CheckResult { get; set; }
        public StringBuilder UploadResult { get; set; }

        public int GalleryId { get; set; } //数据库返回的GalleryId       
        public bool IsRunning { get; set; }       
    }
}



