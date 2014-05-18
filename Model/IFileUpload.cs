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
        CommonResponse FileUpLoad(FileUploadQuest request);

        [OperationContract]
        CommonResponse DirectoryCreate(DirectoryCreateQuest request);

        [OperationContract]
        CommonResponse GalleryCreate(GalleryCreateQuest request);

        [OperationContract]
        CommonResponse FileBundling(GalleryBundlingQuest request);

        [OperationContract]
        DataTableResponse GetCategoryInfo(string token, int fileServerId, string account, string passWord);

        [OperationContract]
        DataTableResponse GetFileServerInfoByEndPoint(string token, string wcfAddress, string account, string passWord);
    }
}
