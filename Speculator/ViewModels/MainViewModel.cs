using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Navigation;
using Speculator.Components;
using Speculator.Messages;
using Speculator.SpeculatorData;
using Speculator.ViewModels.Dialogs;
using Speculator.Views;
using SpeculatorModel.MainData;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class MainViewModel
    {
        private readonly string _pathForAddSymbolSettings = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Speculator";
        private readonly string _settingsFileName = @"\LastSymbols.xml";
        public virtual ObservableCollection<TileNavItem> LastSymbolItems { get; set; }
        public virtual ObservableCollection<SelectedSymbol> LastSelectedSymbols { get; set; }
        public virtual INavigationService NavigationService => null;

        [ServiceProperty(Key = "ChoiceSymbolDialogService")]
        public virtual IDialogService ChoiceSymbolDialogService => null;
        private DataSource[] DataSources { get; set; }
        private SpeculatorDataClient SpeculatorDataClient { get; set; }

        public MainViewModel()
        {
            if (!Directory.Exists(_pathForAddSymbolSettings))
                Directory.CreateDirectory(_pathForAddSymbolSettings);

            XDocument symbolsSettings = null;

            if (File.Exists(_pathForAddSymbolSettings + _settingsFileName))
                symbolsSettings =
                    XDocument.Load(_pathForAddSymbolSettings + _settingsFileName);

            SpeculatorDataClient = new SpeculatorDataClient();
            DataSources = SpeculatorDataClient.DataSourcesAsync().Result;

            var listItems = new List<TileNavItem>();
            var addSymbolTile = new TileNavItem {Content = "Добавить"};
            addSymbolTile.Click += (sender, args) => { AddSymbol(); };
            listItems.Add(addSymbolTile);

            symbolsSettings?.Root?.Descendants("SelectedSymbol").ForEach(item =>
            {
                var lastSymbolForSave = item.FromXElement<SelectedSymbol>();
                var ct = (DataTemplate) Application.Current.FindResource("TileNavItemTemplate");
                addSymbolTile = new TileNavItem
                {
                    DataContext = lastSymbolForSave,
                    ContentTemplate = ct
                };
                addSymbolTile.Click += (sender, args) =>
                {
                    AddSymbol(lastSymbolForSave.DataSource, lastSymbolForSave.Symbol, lastSymbolForSave.DateStart);
                };
                listItems.Add(addSymbolTile);
                if (LastSelectedSymbols == null)
                    LastSelectedSymbols = new ObservableCollection<SelectedSymbol>();
                LastSelectedSymbols.Add(lastSymbolForSave);
            });

            LastSymbolItems = new ObservableCollection<TileNavItem>(listItems);

            Messenger.Default.Register<AddSymbolDocPanelMessage>(this, message =>
            {
                var context = (message.DocPanel.Content as SymbolPanelView)?.DataContext as SymbolPanelViewModel;
                var lastItem = new SelectedSymbol
                {
                    DataSource = context?.DataSource,
                    Symbol = context?.Symbol,
                    DateStart = context?.HistoryDate
                };
                var lastXElement = lastItem.ToXElement<SelectedSymbol>();
                var oldLastItems = LastSelectedSymbols.Where(ss => !ss.Equals(lastItem));
                LastSelectedSymbols = new ObservableCollection<SelectedSymbol> {lastItem};
                if (oldLastItems != null)
                    LastSelectedSymbols.AddRange(oldLastItems.Take(7));

                var xmlUserSettings = new XDocument();
                
                xmlUserSettings.Add(new XElement("root", LastSelectedSymbols.Select(li => li.ToXElement<SelectedSymbol>()).ToList()));
                
                xmlUserSettings.Save(_pathForAddSymbolSettings + _settingsFileName);
            });
        }
        public void NaveButtonClick(string viewName)
        {
            NavigationService.Navigate(viewName, null, this);
        }

        protected void AddSymbol(DataSource dataSource, Symbol symbol, DateTime? historyDate)
        {
            var content = new SymbolPanelView();
            var contentDataContext = content.DataContext as SymbolPanelViewModel;
            contentDataContext.DataSource = dataSource;
            contentDataContext.Symbol = symbol;
            contentDataContext.HistoryDate = historyDate;

            var docPanel = new DocumentPanel
            {
                Caption = symbol.Name + " " + historyDate,
                Content = content
            };

            Messenger.Default.Send(new AddSymbolDocPanelMessage { DocPanel = docPanel });
        }

        protected void AddSymbol()
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
                SelectUiCommand = selectCommand
            });

            var resultChoice = ChoiceSymbolDialogService.ShowDialog(new List<UICommand> { selectCommand, cancelCommand },
                "Выбор инструмента", dialogViewModel);
            if (resultChoice == selectCommand && dialogViewModel.SelectedSymbol != null)
            {
                AddSymbol(dialogViewModel.UsedDataSource, dialogViewModel.SelectedSymbol, dialogViewModel.HistoryDate);
            }
        }
    }
}