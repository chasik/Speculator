using System.ServiceModel;
using System.ServiceProcess;
using SpeculatorServices;

namespace SpeculatorServiceHost
{
    partial class SpeculatorServiceHost : ServiceBase
    {
        private ServiceHost _host;
        public SpeculatorServiceHost()
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
            throw new System.NotImplementedException();
        }

        protected override void OnStop()
        {
            _host?.Close();
        }
    }
}
