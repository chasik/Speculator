using System;
using System.ServiceModel;
using SpeculatorModel.MainData;

namespace SpeculatorServices
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDataCallBacks))]
    public interface IDataBase
    {
        [OperationContract]
        void ConnectToDataSource();

        [OperationContract(IsOneWay = true)]
        void ConnectToHistoryDataSource(Symbol symbol, DateTime dayDateTime);

        [OperationContract]
        void ListenSymbol(Symbol symbol);
    }
}
