using System.Collections.ObjectModel;
using DevExpress.Mvvm.DataAnnotations;
using Speculator.SpeculatorData;
using SpeculatorModel.MainData;

namespace Speculator.ViewModels.Dialogs
{
    [POCOViewModel]
    public class ChoiceSymbolDialogViewModel
    {
        public SpeculatorDataClient SpeculatorDataClient { get; set; }
        public virtual ObservableCollection<Symbol> Symbols { get; set; }
        public virtual DataSource SelectedDataSource { get; set; }
        public virtual ObservableCollection<DataSource> DataSources { get; set; }

        public void DataSourceDblClick()
        {
            Symbols = new ObservableCollection<Symbol>(SpeculatorDataClient.GetSymbolsAsync(SelectedDataSource).Result);
        }
    }
}