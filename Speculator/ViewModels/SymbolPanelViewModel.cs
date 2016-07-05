using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using Speculator.Indicators;
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
        public virtual short Zoom { get; set; }
        public virtual BindingList<SmartComBidAskValue> Glass { get; set; }
        public virtual ObservableCollection<SmartComTrade> Trades { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesBuy { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesSell { get; set; }

        public IchIndicator Indicator { get; set; }

        public virtual int TopRowIndex { get; set; }
        public virtual DateTime StartDateValue { get; set; }
        public virtual DateTime FinishDateValue { get; set; }
        public virtual double MinimumPrice { get; set; }
        public virtual double MaximumPrice { get; set; }
        public virtual double MinimumVisiblePriceValue { get; set; }
        public virtual double MaximumVisiblePriceValue { get; set; }

        public virtual double StartHeightGlassValueParam { get; set; }
        public virtual double FinishHeightGlassValueParam { get; set; }
        public virtual DateTime HistoryDate { get; set; }

        public async void StartListenDataService()
        {
            Zoom = 2;
            StartHeightGlassValueParam = 0;
            FinishHeightGlassValueParam = 30;
            Indicator = new IchIndicator
            {
                Parameters = new List<double> {StartHeightGlassValueParam, FinishHeightGlassValueParam}
            };

            TradesBuy = new ObservableCollection<SmartComTrade>();
            TradesSell = new ObservableCollection<SmartComTrade>();
            Glass = new BindingList<SmartComBidAskValue>();
            DataBaseClient = new DataBaseClient(new InstanceContext(this));

            if (HistoryDate == DateTime.MinValue)
            {
                await DataBaseClient.ConnectToDataSourceAsync();
                DataBaseClient.ListenSymbol(Symbol);
            }
            else
                await DataBaseClient.ConnectToHistoryDataSourceAsync(Symbol, HistoryDate);
        }

        public void UpdateBidOrAskEvent(SmartComSymbol symbol, SmartComBidAskValue value)
        {
            var priceValue = Glass.SingleOrDefault(g => Math.Abs(g.Price - value.Price) < 0.00001);
            if (priceValue == null)
            {
                Glass.Add(value);
                if (Glass.Count == 1)
                {
                    for (var i = 1; i < 200; i++)
                    { 
                        Glass.Add(new SmartComBidAskValue {Price = value.Price + i * (double)symbol.Step, Volume = 0, IsBid = true});
                        Glass.Add(new SmartComBidAskValue {Price = value.Price - i * (double)symbol.Step, Volume = 0, IsBid = false});
                    }
                }
                Glass = new BindingList<SmartComBidAskValue>(Glass.OrderByDescending(g => g.Price).ToList());
            }
            else
            {
                Glass[Glass.IndexOf(priceValue)] = value;
            }
        }

        public void TradeEvent(SmartComSymbol symbol, SmartComTrade trade)
        {
            if (Trades == null)
            {
                Trades = new ObservableCollection<SmartComTrade>();
                StartDateValue = trade.TradeAdded;
                MinimumPrice = MaximumPrice = trade.Price;
            }
            FinishDateValue = trade.TradeAdded;
            Trades.Add(trade);
            if (trade.DiractionId == (byte)DiractionEnum.Buy)
                TradesBuy.Add(trade);
            else if (trade.DiractionId == (byte)DiractionEnum.Sell)
                TradesSell.Add(trade);

            MaximumPrice = Math.Max(MaximumPrice, trade.Price);
            MinimumPrice = Math.Min(MinimumPrice, trade.Price);

            Indicator.AddGlassShear(Glass, symbol);
            Indicator.CalcLastValue();
            Indicator.Values.Add(Indicator.LastAddedGlassShear);
            this.RaisePropertyChanged(vm => vm.Indicator);
        }

        public void TableViewLoaded(RoutedEventArgs eventArgs)
        {
            // подписываемся на прокрутку стакана
            var visualTreeEnumerator = new VisualTreeEnumerator(eventArgs.Source as TableView);
            while (visualTreeEnumerator.MoveNext())
            {
                var glassScrollViewer = visualTreeEnumerator.Current as ScrollViewer;
                if (glassScrollViewer != null)
                {
                    glassScrollViewer.ScrollChanged += GlassScrollViewer_ScrollChanged;
                }
            }
        }

        private void GlassScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var tableView = (e.Source as ScrollViewer)?.TemplatedParent as TableView;
            var gridControl = tableView?.Parent as GridControl;

            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) > 0 ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) > 0)
            {
                Zoom += (short) (Math.Sign(e.VerticalChange) * 1);
                if (Zoom < 1)
                    Zoom = 1;
                else if (Zoom > 4)
                    Zoom = 4;
                gridControl?.RefreshData();
            }

            GetVisibleRowsOnScreen(gridControl, tableView, e.ViewportHeight, e.VerticalOffset);
        }


        public void GetVisibleRowsOnScreen(GridControl grid, TableView view, double viewPortHeight, double verticalOffset)
        {
            var topRowHandle = grid.GetRowHandleByListIndex(view.TopRowIndex);
            var bottomRowHandle = grid.GetRowHandleByVisibleIndex(Convert.ToInt32(viewPortHeight + verticalOffset));

            if (grid.IsValidRowHandle(bottomRowHandle))
            {
                MaximumVisiblePriceValue = (grid.GetRow(topRowHandle) as SmartComBidAskValue).Price;
                MinimumVisiblePriceValue = (grid.GetRow(bottomRowHandle) as SmartComBidAskValue).Price;
            }
            else
            {
                var topVisible = grid.GetRow(topRowHandle) as SmartComBidAskValue;
                if (topVisible != null)
                    MaximumVisiblePriceValue = topVisible.Price;

                var bottomVisible = grid.GetRow(grid.VisibleRowCount - 1) as SmartComBidAskValue;
                if (bottomVisible != null)
                    MinimumVisiblePriceValue = bottomVisible.Price;
            }
        }
    }
}