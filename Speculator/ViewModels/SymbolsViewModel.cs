using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Docking;
using Speculator.SpeculatorData;
using Speculator.ViewModels.Dialogs;
using Speculator.Views;
using SpeculatorModel.MainData;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class SymbolsViewModel
    {
        [ServiceProperty(Key = "ChoiceSymbolDialogService")]
        public virtual IDialogService ChoiceSymbolDialogService => null;
        private DataSource[] DataSources { get; set; }
        private SpeculatorDataClient SpeculatorDataClient { get; set; }

        public SymbolsViewModel()
        {
            SpeculatorDataClient = new SpeculatorDataClient();
            DataSources = SpeculatorDataClient.DataSourcesAsync().Result;
        }

        public void AddSymbol(object commandParams)
        {
            var selectCommand = new UICommand
            {
                Caption = "Выбрать",
                Command = new DelegateCommand<CancelEventArgs>(x => { }, x => true)
            };
            var cancelCommand = new UICommand
            {
                Caption = "Отмена",
                IsDefault = true,
                IsCancel = false
            };
            var dialogViewModel = ViewModelSource.Create(() => new ChoiceSymbolDialogViewModel
            {
                DataSources = new ObservableCollection<DataSource>(DataSources),
                SpeculatorDataClient = SpeculatorDataClient,
                SelectUiCommand = selectCommand});

            var resultChoice = ChoiceSymbolDialogService.ShowDialog(new List<UICommand> {selectCommand, cancelCommand},
                "Выбор инструмента", dialogViewModel);
            if (resultChoice == selectCommand && dialogViewModel.SelectedSymbol != null)
            {
                var content = new SymbolPanelView();
                (content.DataContext as SymbolPanelViewModel).DataSource = dialogViewModel.UsedDataSource;
                (content.DataContext as SymbolPanelViewModel).Symbol = dialogViewModel.SelectedSymbol;
                (content.DataContext as SymbolPanelViewModel).HistoryDate = dialogViewModel.HistoryDate;

                var docPanel = new DocumentPanel
                {
                    Caption = dialogViewModel.SelectedSymbol.Name,
                    Content = content
                };
                (commandParams as DocumentGroup)?.Items.Add(docPanel);
            }
        }
    }
}