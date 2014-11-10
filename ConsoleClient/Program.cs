using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BLLClient;
using Common;
using Model;

namespace Client
{
    class Program
    {
        static ChannelFactory<IFileUpload> channelFactory;
        static IFileUpload _proxy;

        static void EndAdd(IAsyncResult ar)
        {
           int result = _proxy.EndAdd(ar);
           Console.WriteLine("执行结果：" + result.ToString());
           Console.WriteLine("执行结果时间:" + DateTime.Now.ToString());
        }

        static void Test()
        {
            try
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.TransferMode = TransferMode.Streamed;
                binding.SendTimeout = new TimeSpan(0, 0, 2);
                
                channelFactory = new ChannelFactory<IFileUpload>(binding, ClientConfig.WCFAddress);

                _proxy = channelFactory.CreateChannel();

                (_proxy as ICommunicationObject).Open();

                Console.WriteLine("主方法开始执行:" + DateTime.Now.ToString());
                _proxy.BeginAdd(1, 2, EndAdd, null);
                Console.WriteLine("主方法结束:" + DateTime.Now.ToString());


            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        static void Main(string[] args)
        {
            //ClientConfig.Init();
            //Test();
            //Console.ReadKey();
            try
            {
              // Tools.CreatePictureThumbFromCenter("f:\\a.jpg", "f:\\b.jpg",240,320);
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
