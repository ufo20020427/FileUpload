using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml.Serialization;
using DAL;

namespace BLLServer
{
    public class BLLBase
    {
        protected DataBaseAccess _dataBaseAccess;

        public BLLBase()
        {
            _dataBaseAccess = new DataBaseAccess(ServerConfig.Conn, ServerConfig.ProviderName);
        } 
    }
}
