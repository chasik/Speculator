using System;
using SpeculatorServices.Properties;

namespace SpeculatorServices
{
    public class TransaqData : ITransaqData
    {
        public void ConnectToTransaq()
        {
            const string logPath = ".\0";

            if (TransaqConnector.ConnectorInitialize(logPath, 3))
            {
                //TransaqConnector.statusDisconnected.Set();
            }

            TransaqConnector.ConnectorSetCallback();

            var cmd = "<command id=\"connect\">"
                        + "<login>" + Settings.Default.TransaqLogin + "</login>"
                        + "<password>" + Settings.Default.TransaqPassword + "</password>"
                        + "<host>" + Settings.Default.TransaqHost + "</host>"
                        + "<port>" + Settings.Default.TransaqPort + "</port>"
                        + "<rqdelay>100</rqdelay>"
                        + "<session_timeout>25</session_timeout>"
                        + "<request_timeout>10</request_timeout>"
                    + "</command>";

            //TXmlConnector.statusDisconnected.Reset();
            var res = TransaqConnector.ConnectorSendCommand(cmd);
        }

        public void DisconnectFromTransaq()
        {
            var cmd = "<command id=\"disconnect\"/>";

            //TXmlConnector.statusDisconnected.Reset();
            var res = TransaqConnector.ConnectorSendCommand(cmd);

            TransaqConnector.ConnectorUnInitialize();
        }
    }
}
