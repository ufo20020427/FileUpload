using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Common;

namespace BLLClient
{
    public static class ClientConfig
    {
        public static void Init()
        {
            try
            {
                _wcfAddress = ConfigurationManager.AppSettings["WCFAddress"].ToString();
                _account = ConfigurationManager.AppSettings["Account"].ToString();
                _passWord = ConfigurationManager.AppSettings["PassWord"].ToString();
                _token = ConfigurationManager.AppSettings["Token"].ToString();
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                throw;                
            }
        }

        private static string _wcfAddress;
        private static string _account;
        private static string _passWord;
        private static string _token;

        public static string WCFAddress
        {
            get { return _wcfAddress; }
        }

        public static string Account
        {
            get { return _account; }           
        }       

        public static string PassWord
        {
            get { return _passWord; }          
        }

        public static string Token
        {
            get { return _token; }
        } 

    }
}
