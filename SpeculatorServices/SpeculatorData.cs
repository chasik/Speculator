using System;
using System.ServiceModel;

namespace SpeculatorServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SpeculatorData : ISpeculatorData
    {
        public void GetHistory(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
