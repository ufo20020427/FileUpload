using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Model;

namespace BLLClient
{
    public class BLLFileServer
    {
        private IFileUpload _proxy;

        public BLLFileServer(IFileUpload proxy)
        {
            _proxy = proxy;
        }

        public FileServerInfo FileServerInfoLoad()
        {
            DataTableResponse response = _proxy.GetFileServerInfoByEndPoint(ClientConfig.Token, ClientConfig.WCFAddress, ClientConfig.Account, ClientConfig.PassWord);
            DataTable dt = response.DataTable;

            if (dt == null || dt.Rows.Count == 0)
            {
                throw new Exception("加载文件服务器信息失败!");
            }

            FileServerInfo fileServerInfo = new FileServerInfo();
            fileServerInfo.Id = Convert.ToInt32(dt.Rows[0]["FSID"]);
            fileServerInfo.Name = dt.Rows[0]["FSName"].ToString();
            fileServerInfo.OriginalFileServerRootDirectory = dt.Rows[0]["OrgFilePath"].ToString();
            fileServerInfo.ThumbFileServerRootDirectory = dt.Rows[0]["ThumbFilePath"].ToString();

            return fileServerInfo;
        }
    }
}
