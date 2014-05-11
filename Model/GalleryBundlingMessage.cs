using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Model
{
    [MessageContract]
    public class GalleryBundlingMessage
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
        public string OriginalAbsoluteFileDirectory;  //  f:\\素材\\Big\\一级目录\\二级目录\\相册名称
    }
}
