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

        }

        public CategoryType Type { get; set; }
        public bool IsExistVideo { get; set; }
        public bool IsExistVector { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string StoreTableName { get; set; }
    }

}



