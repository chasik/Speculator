using System;
using System.ServiceModel;

namespace SpeculatorServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SmartComData : ISmartComData
    {
        public void ConnectToSmartCom()
        {
            
        }
    }
}
