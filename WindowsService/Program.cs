using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace WindowsService
{
    static class Program
    {   
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new ServiceFileUpload() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
