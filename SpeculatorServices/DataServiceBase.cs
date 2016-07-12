using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SpeculatorModel.SmartCom;

namespace SpeculatorServices
{
    public class DataServiceBase
    {
        protected List<IDataCallBacks> CommunicationObjectsForDelete;
        protected List<IDataCallBacks> ClientsWithCallBack;
        protected Dictionary<IDataCallBacks, List<string>> ClientsWantGetSymbols;

        public DataServiceBase()
        {
            CommunicationObjectsForDelete = new List<IDataCallBacks>();
            ClientsWithCallBack = new List<IDataCallBacks>();
            ClientsWantGetSymbols = new Dictionary<IDataCallBacks, List<string>>();
        }
        protected void RegisterClientWithCallBack(string[] symbols)
        {
            var callBack = OperationContext.Current.GetCallbackChannel<IDataCallBacks>();
            if (!ClientsWithCallBack.Contains(callBack))
            {
                ClientsWithCallBack.Add(callBack);
                ClientsWantGetSymbols.Add(callBack, new List<string>(symbols));
            }
            else 
            {
                // добавляем инструменты, за исключением добавленных ранее
                ClientsWantGetSymbols[callBack].AddRange(symbols.Except(ClientsWantGetSymbols[callBack]));}
        }

        protected void UpdateBidAskEvent(SmartComSymbol symbol, SmartComBidAskValue value, bool isBid = false)
        {
            for (var i = 0; i < ClientsWithCallBack.Count; i++)
            {
                var communicationObject = ClientsWithCallBack[i] as ICommunicationObject;
                if (communicationObject == null || communicationObject.State != CommunicationState.Opened ||
                    !ClientsWantGetSymbols[ClientsWithCallBack[i]].Contains(symbol.Name))
                    continue;
                try
                {
                    ClientsWithCallBack[i].UpdateBidOrAskEvent(symbol, value);
                }
                catch (Exception)
                {
                    ClientsWithCallBack.RemoveAt(i--);
                }
            }
        }

        protected void TradeEvent(SmartComSymbol symbol, SmartComTrade trade)
        {
            for (var i = 0; i < ClientsWithCallBack.Count; i++)
            {
                var communicationObject = ClientsWithCallBack[i] as ICommunicationObject;
                if (communicationObject == null || communicationObject.State != CommunicationState.Opened ||
                    !ClientsWantGetSymbols[ClientsWithCallBack[i]].Contains(symbol.Name))
                    continue;
                try
                {
                    ClientsWithCallBack[i].TradeEvent(symbol, trade);
                }
                catch (Exception)
                {
                    ClientsWithCallBack.RemoveAt(i--);
                }
            }
        }

        protected void QuoteEvent(SmartComSymbol symbol, SmartComQuote quote)
        {
            for (var i = 0; i < ClientsWithCallBack.Count; i++)
            {
                var communicationObject = ClientsWithCallBack[i] as ICommunicationObject;
                if (communicationObject == null || communicationObject.State != CommunicationState.Opened ||
                    !ClientsWantGetSymbols[ClientsWithCallBack[i]].Contains(symbol.Name))
                    continue;
                try
                {
                    ClientsWithCallBack[i].QuoteEvent(symbol, quote);
                }
                catch (Exception)
                {
                    ClientsWithCallBack.RemoveAt(i--);
                }
            }
        }

        protected List<IDataCallBacks> GetCommunicationObjects()
        {
            return (from t in ClientsWithCallBack
                let communicationObject = t as ICommunicationObject
                where communicationObject != null && communicationObject.State == CommunicationState.Opened
                select t).ToList();
        }

        protected void RemoveFailedCommunicationcObjects()
        {
            CommunicationObjectsForDelete.ForEach(o => ClientsWithCallBack.Remove(o));
            CommunicationObjectsForDelete.Clear();
        }
    }
}
