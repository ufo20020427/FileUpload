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
        CommonResponse FileUpLoad(FileUploadRequest request);

        [OperationContract]
        CommonResponse DirectoryCreate(DirectoryCreateRequest request);

        [OperationContract]
        CommonResponse GalleryCreate(GalleryCreateRequest request);

        [OperationContract]
        CommonResponse FileBundling(GalleryBundlingRequest request);

        [OperationContract]
        DataTableResponse GetCategoryInfo(string token, int fileServerId, string account, string passWord);

        [OperationContract]
        DataTableResponse GetFileServerInfoByEndPoint(string token, string wcfAddress, string account, string passWord);

        [OperationContract]
        int Add(int a, int b);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginAdd(int a, int b, AsyncCallback callBack, object state); 
        
        int EndAdd(IAsyncResult ar); 
    }
}
