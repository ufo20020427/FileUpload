using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLLClient;
using Common;

namespace WinFormClient
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
                ClientConfig.Init();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }

        private void CategoryLoad()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Tools.LogWrite(ex.ToString());
            }
        }
    }
}
