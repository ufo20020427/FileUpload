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

        static void ThreadProc(Object sender)
        {

            string file = "E:\\项目前期资料\\佛山三期资料\\固定费统计分析\\输入\\201312\\abc.jpg";
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                FileUploadMessage request = new FileUploadMessage();
                request.Token = "1qaz2wsx";
                request.Account = "account1";
                request.PassWord = "pass1";
                request.CategoryId = 123;
                request.FileServerId = 1;
                request.OriginalFileServerRootDirectory = "F:\\素材\\Big";
                request.ThumbFileServerRootDirectory = "F:\\素材\\Small";
                request.CategoryAbsolutePath = "\\男装一级目录\\男装二级目录\\201405\\";
                request.CategoryRelativePath = "/男装一级目录/男装二级目录/201405/";
                request.CategoryName = "品牌画册二级目录";
                request.FileName = "abc.jpg";
                request.StoreTableName = "Files_Boy";
                request.FileData = fs;
                ResponseMessage response = _proxy.FileUpLoad(request);
                Console.WriteLine(response.IsSuccessed);
                Console.WriteLine(response.ResultMessage);
            }    
        }

        static void Test()
        {
            try
            {
                channelFactory = new ChannelFactory<IFileUpload>("FileUploadEndPoint");
                _proxy = channelFactory.CreateChannel();

                (_proxy as ICommunicationObject).Open();

                CategoryResponseMessage response = _proxy.GetCategory(ClientConfig.Token, 1, "account1", "pass1");
                Console.WriteLine(response.IsSuccessed.ToString());
                Console.WriteLine(response.ResultMessage);
                DataTable dt = response.DataTable;
                Console.WriteLine(dt.Rows.Count.ToString());

                //   ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), string.Empty);


            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }

        static void Main(string[] args)
        {
            ClientConfig.Init();
            Test();
            Console.ReadKey();
        }
    }
}
