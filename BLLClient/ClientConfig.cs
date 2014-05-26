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
                _thumbPictureWidth = int.Parse(ConfigurationManager.AppSettings["ThumbPictureWidth"]);
                _thumbPictureHeight = int.Parse(ConfigurationManager.AppSettings["ThumbPictureHeight"]);
                _vectorPictureExtenName = ConfigurationManager.AppSettings["VectorPictureExtenName"].ToString();
                _pictureExtenName = ConfigurationManager.AppSettings["PictureExtenName"].ToString();
                _maxThread = int.Parse(ConfigurationManager.AppSettings["MaxThread"]);
                _sendTimeout = int.Parse(ConfigurationManager.AppSettings["SendTimeout"]);

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
        private static int _thumbPictureWidth;
        private static int _thumbPictureHeight;
        private static string _vectorPictureExtenName;
        private static string _pictureExtenName;
        private static int _restHourStart;
        private static int _restHourEnd;
        private static int _maxThread;
        private static int _sendTimeout;      

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

        public static int ThumbPictureWidth
        {
            get { return _thumbPictureWidth; }
        }

        public static int ThumbPictureHeight
        {
            get { return _thumbPictureHeight; }
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

        public static int MaxThread
        {
            get { return _maxThread; }
        }

        public static int SendTimeout
        {
            get { return _sendTimeout; }
        }
    }
}
