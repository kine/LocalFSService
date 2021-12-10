using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using Microsoft.ServiceBus.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;

namespace Navertica.Services.NVRLocalFSService
{
    public partial class LocalFSWinService : ServiceBase
    {
        WebServiceHost host;
        public LocalFSWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (host != null)
            {
                host.Close();
            }
            string serviceNamespace = Properties.Settings.Default.RelayNameSpace;
            var serviceName = GetServiceName();
            Uri address = ServiceBusEnvironment.CreateServiceUri("https", serviceNamespace, Properties.Settings.Default.RelayServicePrefix+"/"+serviceName);

            host = new WebServiceHost(typeof(LocalFSService), address);
            host.Open();

            //host = new WebServiceHost(typeof(LocalFSService));
            foreach(var addr in host.BaseAddresses)
            {
                Console.WriteLine(addr);

            }
            //host.Open();

        }

        private string GetServiceName()
        {
            var domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var computerName = System.Environment.MachineName;
            return computerName+"."+domain;
        }

        protected override void OnStop()
        {
            host.Close();
        }

        internal void OnDebug()
        {
            OnStart(null);
        }
    }
}
