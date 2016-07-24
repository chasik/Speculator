using System;
using System.ServiceModel;
using SpeculatorModel.MainData;
using SpeculatorServices.SmartCom;
using SpeculatorServices.Trading;

namespace SpeculatorServices
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDataCallBacks))]
    public interface IDataBase
    {
        [OperationContract]
        void ConnectToDataSource();

        [OperationContract(IsOneWay = true)]
        void ConnectToHistoryDataSource(Symbol symbol, DateTime? startDateTime, DateTime? finishDateTime = null, bool returnAllData = false);

        [OperationContract]
        void ListenSymbol(Symbol symbol);

        [OperationContract(IsOneWay = true)]
        void PlaceOrder(TradingOrder order);

        [OperationContract(IsOneWay = true)]
        void CancelOrder(string symbol, string orderId);
    }
}
