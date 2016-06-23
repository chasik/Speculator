using System.ServiceModel;
using SpeculatorModel.SmartCom;

namespace SpeculatorServices
{
    [ServiceContract]
    public interface IDataCallBacks
    {
        [OperationContract(IsOneWay = true)]
        void UpdateBidOrAskEvent(SmartComSymbol symbol, SmartComBidAskValue value);

        [OperationContract(IsOneWay = true)]
        void TradeEvent(SmartComSymbol symbol, SmartComTrade trade);
    }
}
