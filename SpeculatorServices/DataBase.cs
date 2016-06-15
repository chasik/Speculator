using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SpeculatorServices
{
    public class DataBase : IDataBase
    {
        protected List<IDataCallBacks> ClientsWithCallBack;
        protected Dictionary<IDataCallBacks, List<string>> ClientsWantGetSymbols;

        public DataBase()
        {
            ClientsWithCallBack = new List<IDataCallBacks>();
        }

        protected void RegisterClientWithCallBack(string[] symbols)
        {
            IDataCallBacks callBack = OperationContext.Current.GetCallbackChannel<IDataCallBacks>();
            if (!ClientsWithCallBack.Contains(callBack))
            {
                ClientsWithCallBack.Add(callBack);
                ClientsWantGetSymbols.Add(callBack, new List<string>(symbols));
            }

        }

        protected void UpdateBidAskEvent(bool IsAsk = false)
        {
            for (var i = 0; i < ClientsWithCallBack.Count; i++)
            {
                var communicationObject = ClientsWithCallBack[i] as ICommunicationObject;
                if (communicationObject == null || communicationObject.State != CommunicationState.Opened)
                    continue;
                try
                {
                    if (IsAsk)
                        ClientsWithCallBack[i].UpdateAskEvent();
                    else 
                        ClientsWithCallBack[i].UpdateBidEvent();
                }
                catch (Exception)
                {
                    ClientsWithCallBack.RemoveAt(i--);
                }
            }
        }
    }
}
