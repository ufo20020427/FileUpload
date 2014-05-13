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
        CommonResponse FileUpLoad(FileUploadQuest requestMessage);

        [OperationContract]
        CommonResponse DirectoryCreate(DirectoryCreateQuest requestMessage);

        [OperationContract]
        CommonResponse GalleryCreate(GalleryCreateQuest requestMessage);

        [OperationContract]
        CommonResponse FileBundling(GalleryBundlingQuest requestMessage);

        [OperationContract]
        DataTableResponse GetCategoryInfo(string token, int fileServerId, string account, string passWord);

        [OperationContract]
        DataTableResponse GetFileServerInfoByEndPoint(string token, string wcfAddress, string account, string passWord);
    }
}
