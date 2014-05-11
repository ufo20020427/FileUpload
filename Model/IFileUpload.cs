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
        CommonResponseMessage FileUpLoad(FileUploadQuestMessage requestMessage);

        [OperationContract]
        CommonResponseMessage DirectoryCreate(DirectoryCreateQuestMessage requestMessage);

        [OperationContract]
        CommonResponseMessage GalleryCreate(GalleryCreateQuestMessage requestMessage);

        [OperationContract]
        CommonResponseMessage FileBundling(GalleryBundlingQuestMessage requestMessage);

        [OperationContract]
        DataTableResponseMessage GetCategoryInfo(string token, int fileServerId, string account, string passWord);

        [OperationContract]
        DataTableResponseMessage GetFileServerInfoByEndPoint(string token, string wcfAddress, string account, string passWord);
    }
}
