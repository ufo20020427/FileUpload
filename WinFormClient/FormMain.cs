using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using BLLClient;
using Common;
using Model;

namespace WinFormClient
{
    public partial class FormMain : Form
    {       
        private  IFileUpload _proxy;
      
        public FormMain()
        {
            InitializeComponent();
            Init();
            CategoryTreeLoad();
        }

        private void Init()
        {
            try
            {
                ClientConfig.Init();

                NetTcpBinding binding = new NetTcpBinding();
                binding.TransferMode = TransferMode.Streamed;
                ChannelFactory<IFileUpload> channelFactory = new ChannelFactory<IFileUpload>(binding, ClientConfig.WCFAddress);
                _proxy = channelFactory.CreateChannel();
                (_proxy as ICommunicationObject).Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

        private void CategoryTreeLoad()
        {
            try
            {
                DataTableResponse response = _proxy.GetCategoryInfo(ClientConfig.Token, 1, "account1", "pass1");
                BLLCategoryTree bllCategoryTree = new BLLCategoryTree(treeCategory);
                bllCategoryTree.CategoryLoad(response.DataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

     

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            (_proxy as ICommunicationObject).Close();
        }
    }
}
