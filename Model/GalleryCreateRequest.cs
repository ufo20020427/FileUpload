using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Model
{
    [MessageContract]
    public class GalleryCreateRequest
    {
        [MessageHeader]
        public string Token;

        [MessageHeader]
        public string Account;

        [MessageHeader]
        public string PassWord;

        [MessageHeader]
        public long CategoryId;

        [MessageHeader]
        public int FileServerId;    

        [MessageHeader]
        public string GalleryName;

        [MessageHeader]
        public string ConverRelativeFilePath; // /Small/一级目录/二级目录/相册名称/Cover.jpg

        [MessageHeader]
        public string BundlingRelativeFilePath; // /Big/一级目录/二级目录/相册名称/Package.zip    

        [MessageHeader]
        public int PageCount;

        [MessageHeader]
        public string Introudce;

        [MessageHeader]
        public string VideoRelativeFilePath;  // /Big/一级目录/二级目录/相册名称/video.rar

        [MessageHeader]
        public string Designer;

        [MessageHeader]
        public string Address;

        [MessageHeader]
        public string OriginalAbsoluteFileDirectory;  //  f:\\素材\\Big\\一级目录\\二级目录\\相册名称

        [MessageHeader]
        public string ThumbAbsoluteFileDirectory; //      f:\\素材\\Small\\一级目录\\二级目录\\相册名称

        [MessageHeader]
        public string StoreTableName; //Album_Brand
    }
}
