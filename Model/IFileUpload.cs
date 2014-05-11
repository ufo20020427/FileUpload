using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [ServiceContract]
    public interface IFileUpload
    {
        [OperationContract]
        ResponseMessage FileUpLoad(FileUploadMessage requestMessage);

        [OperationContract]
        ResponseMessage DirectoryCreate(DirectoryCreateMessage requestMessage);

        [OperationContract]
        ResponseMessage GalleryCreate(GalleryCreateMessage requestMessage);

        [OperationContract]
        ResponseMessage FileBundling(GalleryBundlingMessage requestMessage);

        [OperationContract]
        CategoryResponseMessage GetCategory(string token, int fileServerId, string account, string passWord);
    }
}
