﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Model
{
    [MessageContract]
    public class FileUploadRequest
    {
        [MessageHeader]
        public string Token;

        [MessageHeader]
        public string Account;

        [MessageHeader]
        public string PassWord;

        [MessageHeader]
        public int ThumbPictureWidth;

        [MessageHeader]
        public int ThumbPictureHeight;

        [MessageHeader]
        public string VectorPictureExtenName;

        [MessageHeader]
        public CategoryType CategoryType;

        [MessageHeader]
        public int CategoryId;

        [MessageHeader]
        public int FileServerId;    

         // /Big/一级目录/二级目录/130506/文件名.jpg
         // /Small/一级目录/二级目录/130506/sm_文件名.jpg
        //  f:\\素材\\Big\\一级目录\\二级目录\\130506\\文件名.jpg
        //  f:\\素材\\Small\\一级目录\\二级目录\\130506\\sm_文件名.jpg

        [MessageHeader]
        public string OriginalFileServerRootDirectory; // F:\\素材\\Big

        [MessageHeader]
        public string ThumbFileServerRootDirectory;    // F:\\素材\\Small


        [MessageHeader]
        public string CategoryAbsolutePath; // \\一级目录\\二级目录\\130506\\

        [MessageHeader]
        public string CategoryRelativePath; // /一级目录/二级目录/130506/

        [MessageHeader]
        public string LevelCategoryName;     //  一级目录_二级目录_三级目录

        [MessageHeader]
        public string StoreTableName; //Files_Boy

        [MessageHeader]
        public string FileName;

        [MessageHeader]
        public string VectorFileName; //abc.cdi|abc.ai   

        [MessageHeader]
        public bool IsVector;

        [MessageHeader]
        public bool IsThumbSquare; //True：缩略图为正方形  False：缩略图按配置绝对值

        [MessageBodyMember]
        public Stream FileData;
    }
}
