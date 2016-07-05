using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Speculator.SpeculatorData;
using SpeculatorModel.MainData;
using System;

namespace Speculator.ViewModels.Dialogs
{
    [POCOViewModel]
    public class ChoiceSymbolDialogViewModel
    {
        //[ServiceProperty(Key = "CurrentDialogService")]
        //protected virtual ICurrentDialogService CurrentDialogService => null;

        public UICommand SelectUiCommand { get; set; }
        public SpeculatorDataClient SpeculatorDataClient { get; set; }
        public virtual Symbol SelectedSymbol { get; set; }
        public virtual ObservableCollection<Symbol> Symbols { get; set; }
        public virtual DataSource UsedDataSource { get; set; }
        public virtual DataSource SelectedDataSource { get; set; }
        public virtual ObservableCollection<DataSource> DataSources { get; set; }

        public virtual DateTime HistoryDate { get; set; }

        public void DataSourceDblClick()
        {
            UsedDataSource = SelectedDataSource;
            var result = SpeculatorDataClient.GetSymbolsAsync(SelectedDataSource).Result;
            if (result != null)
                Symbols = new ObservableCollection<Symbol>(result);
        }

        public void SelectedSymbolDblClick()
        {
            //CurrentDialogService.Close(SelectUiCommand);
        }
    }
}