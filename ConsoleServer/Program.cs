using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using BLLServer;
using Common;
using Model;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
          
            try
            {
                ServerConfig.Init();
                using (ServiceHost host = new ServiceHost(typeof(FileUploadProcess)))
                {
                    host.Open();
                    Console.WriteLine("服务已启动。");
                    Console.ReadKey();
                    host.Close();
                }           
            }           
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
            }
        }
    }
}
