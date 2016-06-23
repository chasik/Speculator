using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using Speculator.SmartComData;
using SpeculatorModel.MainData;
using SpeculatorModel.SmartCom;


namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class SymbolPanelViewModel : IDataBaseCallback
    {
        public IDataBase DataBaseClient { get; set; }
        public DataSource DataSource { get; set; }
        public Symbol Symbol { get; set; }
        public virtual BindingList<SmartComBidAskValue> Glass { get; set; }

        public void StartListenDataService()
        {
            Glass = new BindingList<SmartComBidAskValue>();
            DataBaseClient = new DataBaseClient(new InstanceContext(this));
            DataBaseClient.ConnectToDataSource();
            DataBaseClient.ListenSymbol(Symbol);
        }

        public void UpdateBidOrAskEvent(SmartComSymbol symbol, SmartComBidAskValue value)
        {
            var priceValue = Glass.SingleOrDefault(g => g.Price.Equals(value.Price));
            if (priceValue == null)
            {
                Glass.Add(value);
                Glass = new BindingList<SmartComBidAskValue>(Glass.OrderBy(g => g.Price).ToList());
            }
            else
            {
                Glass[Glass.IndexOf(priceValue)] = value;
            }
        }

        public void TradeEvent(SmartComSymbol symbol, SmartComTrade trade)
        {
            
        }
    }
}