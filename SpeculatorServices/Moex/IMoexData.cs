using System.Collections.Generic;
using System.ServiceModel;
using SpeculatorModel.MoexHistory;

namespace SpeculatorServices.Moex
{
    [ServiceContract]
    public interface IMoexData
    {
        [OperationContract]
        List<MoexSymbol> MoexSymbols();
    }
}
