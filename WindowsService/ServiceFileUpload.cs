using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using BLLServer;
using Common;

namespace WindowsService
{
    public partial class ServiceFileUpload : ServiceBase
    {
        private ServiceHost _host;

        public ServiceFileUpload()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ServerConfig.Init();
                _host = new ServiceHost(typeof(FileUploadProcess));
                _host.Open();
                Tools.LogWrite("服务成功启动！");
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
                _host.Close();
            }
        }

        protected override void OnStop()
        {
            try
            {
                _host.Close();
                Tools.LogWrite("服务成功关闭！");
            }
            catch (Exception ex)
            {
                Tools.LogWrite(ex.ToString());
            }
        }
    }
}
