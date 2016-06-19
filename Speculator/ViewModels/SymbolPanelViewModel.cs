using System.ServiceModel;
using DevExpress.Mvvm.DataAnnotations;
using Speculator.SmartComData;
using SpeculatorModel.MainData;
using SpeculatorServices;
using IDataBase = Speculator.SmartComData.IDataBase;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class SymbolPanelViewModel : IDataCallBacks
    {
        public IDataBase DataServiceClient { get; set; }
        public DataSource DataSource { get; set; }
        public Symbol Symbol { get; set; }

        public void StartListenDataService()
        {
            DataServiceClient = new DataBaseClient(new InstanceContext(this));
            DataServiceClient.ListenSymbol(Symbol);
        }

        public void UpdateAskEvent()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateBidEvent()
        {
            throw new System.NotImplementedException();
        }
    }
}