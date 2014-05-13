using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FileServerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OriginalFileServerRootDirectory { get; set; }
        public string ThumbFileServerRootDirectory { get; set; }
    }
}
