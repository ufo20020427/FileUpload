using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Model
{
    [MessageContract]
    public class DirectoryCreateQuest
    {
        [MessageHeader]
        public string Token;

        [MessageHeader]
        public int FileServerId;

        [MessageHeader]
        public string Account;

        [MessageHeader]
        public string PassWord;

        [MessageHeader]
        public string OriginalAbsoluteFileDirectory;  // f:\\素材\\Big\\一级目录\\二级目录\\130506

        [MessageHeader]
        public string ThumbAbsoluteFileDirectory;    // f:\\素材\\Small\\一级目录\\二级目录\\130506
    }
}
