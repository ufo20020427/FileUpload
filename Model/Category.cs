using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FolderName { get; set; }
        public int ParentId { get; set; }
        public byte Type { get; set; }
        public bool IsExistVideo { get; set; }
        public bool IsExistVector { get; set; }
        public string StoreTableName { get; set; }
        public bool IsDetail { get; set; }
    }
}
