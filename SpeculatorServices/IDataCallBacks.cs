using System;
using System.ServiceModel;
using SpeculatorModel.SmartCom;
using SpeculatorServices.SmartCom;
using SpeculatorServices.Trading;


namespace SpeculatorServices
{
    [ServiceContract]
    public interface IDataCallBacks
    {
        [OperationContract(IsOneWay = true)]
        void ReturnHistoryData(SmartComSymbol symbol, HistoryDataRow[] historyData);

        [OperationContract(IsOneWay = true)]
        void UpdateBidOrAskEvent(SmartComSymbol symbol, SmartComBidAskValue value);

        [OperationContract(IsOneWay = true)]
        void TradeEvent(SmartComSymbol symbol, SmartComTrade trade);

        [OperationContract(IsOneWay = true)]
        void QuoteEvent(SmartComSymbol symbol, SmartComQuote quote);



        [OperationContract(IsOneWay = true)]
        void OrderSucceeded(int cookie, string orderid);

        [OperationContract(IsOneWay = true)]
        void OrderFailed(int cookie, string orderid, string reason);

        [OperationContract(IsOneWay = true)]
        void OrderMoveSucceeded(string orderid);

        [OperationContract(IsOneWay = true)]
        void OrderMoveFailed(string orderid);

        [OperationContract(IsOneWay = true)]
        void OrderCancelSucceeded(string orderid);

        [OperationContract(IsOneWay = true)]
        void OrderCancelFailed(string orderid);

        [OperationContract(IsOneWay = true)]
        void UpdatePosition(string portfolio, string symbol, double avprice, double amount, double planned);

        [OperationContract(IsOneWay = true)]
        void UpdateOrder(string portfolio, TradingOrder order, double filled, DateTime datetime, string orderid, string orderno, int status_mask);

        [OperationContract(IsOneWay = true)]
        void AddTrade(string portfolio, string symbol, string orderid, double price, double amount, DateTime datetime, string tradeno);
    }
}
