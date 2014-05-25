using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Common;

namespace BLLServer
{
    public static class ServerConfig
    {
        public static void Init()
        {
            try
            {
                _conn = ConfigurationManager.ConnectionStrings["FileServerConnString"].ConnectionString;
                _providerName = ConfigurationManager.ConnectionStrings["FileServerConnString"].ProviderName;
                _token = ConfigurationManager.AppSettings["Token"].ToString();
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                throw;
            }
        }

        private static string _conn;
        private static string _providerName;
        private static string _token;

        public static string Conn
        {
            get { return _conn; }
        }

        public static string ProviderName
        {
            get { return _providerName; }
        }

        public static string Token
        {
            get { return _token; }
        }
    }
}
