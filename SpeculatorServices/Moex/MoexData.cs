using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SpeculatorModel;
using SpeculatorModel.MoexHistory;

namespace SpeculatorServices.Moex
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MoexData : IMoexData
    {
        public List<MoexSymbol> MoexSymbols()
        {
            using (var dbContext = new SpeculatorContext())
            {
                return dbContext.MoexSymbols.ToList();
            }
        }
    }
}
