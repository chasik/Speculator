using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
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
        public virtual byte Zoom { get; set; }
        public virtual BindingList<SmartComBidAskValue> Glass { get; set; }
        public virtual ObservableCollection<SmartComTrade> Trades { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesBuy { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesSell { get; set; }

        public virtual int TopRowIndex { get; set; }
        public virtual double MinimumVisiblePriceValue { get; set; }
        public virtual double MaximumVisiblePriceValue { get; set; }

        public async void StartListenDataService()
        {
            Trades = new ObservableCollection<SmartComTrade>();
            TradesBuy = new ObservableCollection<SmartComTrade>();
            TradesSell = new ObservableCollection<SmartComTrade>();
            Glass = new BindingList<SmartComBidAskValue>();
            DataBaseClient = new DataBaseClient(new InstanceContext(this));

            await DataBaseClient.ConnectToDataSourceAsync();
            DataBaseClient.ListenSymbol(Symbol);
        }

        public void UpdateBidOrAskEvent(SmartComSymbol symbol, SmartComBidAskValue value)
        {
            var priceValue = Glass.SingleOrDefault(g => g.Price.Equals(value.Price));
            if (priceValue == null)
            {
                Glass.Add(value);
                Glass = new BindingList<SmartComBidAskValue>(Glass.OrderByDescending(g => g.Price).ToList());
            }
            else
            {
                Glass[Glass.IndexOf(priceValue)] = value;
            }
        }

        public void TradeEvent(SmartComSymbol symbol, SmartComTrade trade)
        {
            Trades.Add(trade);
            if (trade.DiractionId == (byte)DiractionEnum.Buy)
                TradesBuy.Add(trade);
            else if (trade.DiractionId == (byte)DiractionEnum.Sell)
                TradesSell.Add(trade);
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
                    glassScrollViewer.ScrollChanged += GlassScrollViewer_ScrollChanged; ;
                }
            }
        }

        private void GlassScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var tableView = (e.Source as ScrollViewer).TemplatedParent as TableView;
            var gridControl = tableView.Parent as GridControl;

            GetVisibleRowsOnScreen(gridControl, tableView, e.ViewportHeight, e.VerticalOffset);
           
        }


        public void GetVisibleRowsOnScreen(GridControl grid, TableView view, double viewPortHeight, double verticalOffset)
        {
            var topRowHandle = grid.GetRowHandleByListIndex(view.TopRowIndex);
            var bottomRowHandle = grid.GetRowHandleByVisibleIndex(Convert.ToInt32(viewPortHeight + verticalOffset));

            SmartComBidAskValue oneRow;
            if (grid.IsValidRowHandle(bottomRowHandle))
            {
                MaximumVisiblePriceValue = (grid.GetRow(topRowHandle) as SmartComBidAskValue).Price;
                MinimumVisiblePriceValue = (grid.GetRow(bottomRowHandle) as SmartComBidAskValue).Price;
                //for (var i = topRowHandle; i <= bottomRowHandle; i++)
                //{
                //    oneRow = grid.GetRow(i) as SmartComBidAskValue;
                //}
            }
            else
            {
                MaximumVisiblePriceValue = (grid.GetRow(topRowHandle) as SmartComBidAskValue).Price;
                MinimumVisiblePriceValue = (grid.GetRow(grid.VisibleRowCount - 1) as SmartComBidAskValue).Price;
                //for (var i = topRowHandle; i < grid.VisibleRowCount; i++)
                //{
                //    oneRow = grid.GetRow(i) as SmartComBidAskValue;
                //    MinimumVisiblePriceValue = MaximumVisiblePriceValue = oneRow.Price;
                //}
            }
        }
    }
}