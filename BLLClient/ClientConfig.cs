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
                _vectorPictureExtenName = ConfigurationManager.AppSettings["VectorPictureExtenName"].ToString();
                _pictureExtenName = ConfigurationManager.AppSettings["PictureExtenName"].ToString();

                string[] restHours = ConfigurationManager.AppSettings["RestHours"].ToString().Split('-');
                _restHourStart = int.Parse(restHours[0]);
                _restHourEnd = int.Parse(restHours[1]);
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
        private static string _vectorPictureExtenName;
        private static string _pictureExtenName;
        private static int _restHourStart;
        private static int _restHourEnd;

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

        public static string VectorPictureExtenName
        {
            get { return _vectorPictureExtenName; }
        }

        public static string PictureExtenName
        {
            get { return _pictureExtenName; }
        }

        public static int RestHourStart
        {
            get { return _restHourStart; }
        }

        public static int RestHourEnd
        {
            get { return _restHourEnd; }
        }

    }
}
