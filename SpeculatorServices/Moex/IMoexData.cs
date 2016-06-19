using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using SpeculatorModel.MoexHistory;

namespace SpeculatorServices.Moex
{
    [ServiceContract]
    public interface IMoexData
    {
        [OperationContract]
        MoexSystem AddSystem(MoexSystem system);

        [OperationContract]
        MoexSymbol AddSymbol(MoexSymbol symbol);

        [OperationContract]
        void AddClaims(MoexClaim[] claims);

        [OperationContract]
        void AddTrades(MoexTrade[] trades);

        [OperationContract]
        MoexSystem[] Systems();

        [OperationContract]
        MoexSymbol[] Symbols();
    }
}
