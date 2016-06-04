using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
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

        public void AddSymbol(object commandParams){
            //var docPanel = new DocumentPanel
            //{//    Caption = "document uuuuu",
            //    Content = new SymbolPanelView {DataContext = new SymbolPanelViewModel()}
            //};
            //(commandParams as DocumentGroup)?.Items.Add(docPanel);
            var selectCommand = new UICommand
            {
                Caption = "Выбрать",
                Command = new DelegateCommand<CancelEventArgs>(x => { }, x => true)
            };
            var cancelCommand = new UICommand
            {Caption = "Отмена",
                IsDefault = true,
                IsCancel = false
            };
            var dialogViewModel = new ChoiceSymbolDialogViewModel();
            var resultChoice = ChoiceSymbolDialogService.ShowDialog(new List<UICommand>{ selectCommand, cancelCommand}, "Выбор инструмента", dialogViewModel);
            if (resultChoice == selectCommand)
            {
                
            }
        }}
}