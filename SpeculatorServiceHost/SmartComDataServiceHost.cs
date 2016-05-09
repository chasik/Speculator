using System.ServiceModel;
using System.ServiceProcess;
using SpeculatorServices.SmartCom;

namespace SpeculatorServiceHost
{
    public partial class SmartComDataServiceHost : ServiceBase
    {
        private ServiceHost _host;
        public SmartComDataServiceHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _host = new ServiceHost(typeof(SmartComData));
            _host.Opening += _host_Opening;
            _host.Open();
        }

        private void _host_Opening(object sender, System.EventArgs e)
        {
        }

        protected override void OnStop()
        {
            _host?.Close();
        }
    }
}
