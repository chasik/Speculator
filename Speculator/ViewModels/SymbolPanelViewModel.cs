using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using Infragistics.Controls.Charts;
using SmartCOM3Lib;
using Speculator.Indicators;
using Speculator.SmartComData;
using SpeculatorModel.MainData;
using SpeculatorModel.SmartCom;
using SpeculatorServices.Trading;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class SymbolPanelViewModel : IDataBaseCallback
    {
        private Timer _mainTimer;
        public virtual bool TradingEnabled { get; set; }

        public IDataBase DataBaseClient { get; set; }
        public DataSource DataSource { get; set; }
        public Symbol Symbol { get; set; }
        public virtual short Zoom { get; set; }

        private List<SmartComBidAskValue> _glass { get; set; }
        public virtual BindingList<SmartComBidAskValue> Glass { get; set; }

        private SmartComTrade _lastTrade { get; set; }
        public virtual ObservableCollection<SmartComTrade> Trades { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesBuy { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesSell { get; set; }

        private OpenInterest _lastOpenInterest;
        public virtual ObservableCollection<OpenInterest> OpenInterest { get; set; }


        private List<SmartComTrade> _tradesBuffer;
        private List<SmartComTrade> _tradesBuyBuffer;
        private List<SmartComTrade> _tradesSellBuffer;
        private GridControl _glassGridControl;
        private XamDataChart _chart;

        public IchIndicator Indicator { get; set; }
        public IchIndicator Indicator2 { get; set; }
        public IchIndicator Indicator3 { get; set; }
        public IchIndicator Indicator4 { get; set; }

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

        private TradingState _tradingState;
        private double _lastAsk;
        private double _lastBid;
        private int _lastOrderId;
        private List<TradingOrder> _tradingOrders;
        private const int TradeUpIndicatorLevel = 100;
        private const int TradeDownIndicatorLevel = -100;

        public async void StartListenDataService()
        {
            _tradingState = TradingState.Free;

            var dispatcher = Dispatcher.CurrentDispatcher;
            _mainTimer = new Timer(HistoryDate == DateTime.MinValue ? 200 : 500);
            _mainTimer.Elapsed += (sender, args) =>
            {
                if (_tradesBuffer == null || _tradesBuffer.Count == 0)
                    return;

                dispatcher.Invoke(() =>
                {
                    //_chart.Diagram.Series.ForEach(s => s.Points.BeginInit());

                    try
                    {

                        FinishDateValue = _tradesBuffer.Max(t => t.TradeAdded);
                        MaximumPrice = Math.Max(MaximumPrice, _tradesBuffer.Max(t => t.Price));
                        MinimumPrice = Math.Min(MinimumPrice, _tradesBuffer.Min(t => t.Price));

                        Trades.AddRange(_tradesBuffer);
                        _tradesBuffer.Clear();
                        TradesBuy.AddRange(_tradesBuyBuffer);
                        _tradesBuyBuffer.Clear();
                        TradesSell.AddRange(_tradesSellBuffer);
                        _tradesSellBuffer.Clear();
                        this.RaisePropertyChanged(vm => vm.Indicator);

                        this.RaisePropertyChanged(vm => vm.Indicator2);
                        this.RaisePropertyChanged(vm => vm.Indicator3);
                        this.RaisePropertyChanged(vm => vm.Indicator4);

                        Glass = new BindingList<SmartComBidAskValue>(_glass);
                    }
                    catch (Exception)
                    {
                        //ignore
                    }

                    //_chart.Diagram.Series.ForEach(s => s.Points.EndInit());
                }, DispatcherPriority.Render);
            };

            _mainTimer.Start();

            Zoom = 1;
            StartHeightGlassValueParam = 1;
            FinishHeightGlassValueParam = 11;
            Indicator = new IchIndicator
            {
                Parameters = new List<double> {StartHeightGlassValueParam, FinishHeightGlassValueParam}
            };
            Indicator2 = new IchIndicator
            {
                Parameters = new List<double> { StartHeightGlassValueParam, 28 }
            };
            Indicator3 = new IchIndicator
            {
                Parameters = new List<double> { StartHeightGlassValueParam, 49 }
            };
            Indicator4 = new IchIndicator
            {
                Parameters = new List<double> { 25, 49 }
            };

            OpenInterest = new ObservableCollection<OpenInterest>();

            TradesBuy = new ObservableCollection<SmartComTrade>();
            TradesSell = new ObservableCollection<SmartComTrade>();

            _tradesBuyBuffer = new List<SmartComTrade>();
            _tradesSellBuffer = new List<SmartComTrade>();

            Glass = new BindingList<SmartComBidAskValue>();
            _glass = new List<SmartComBidAskValue>();

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
            var priceValue = _glass.SingleOrDefault(g => Math.Abs(g.Price - value.Price) < 0.00001);
            if (priceValue == null)
            {
                _glass.Add(value);
                if (_glass.Count == 1)
                {
                    for (var i = 1; i < 250; i++)
                    {
                        _glass.Add(new SmartComBidAskValue {Price = value.Price + i * (double)symbol.Step, Volume = 0, IsBid = true});
                        _glass.Add(new SmartComBidAskValue {Price = value.Price - i * (double)symbol.Step, Volume = 0, IsBid = false});
                    }
                }
                _glass = new List<SmartComBidAskValue>(_glass.OrderByDescending(g => g.Price).ToList());
            }
            else
            {
                _glass[_glass.IndexOf(priceValue)] = value;
            }
        }

        public void TradeEvent(SmartComSymbol symbol, SmartComTrade trade)
        {
            if (Trades == null)
            {
                Trades = new ObservableCollection<SmartComTrade>();
                _tradesBuffer = new List<SmartComTrade>();
                _lastTrade = new SmartComTrade();

                StartDateValue = trade.TradeAdded;
                MinimumPrice = MaximumPrice = trade.Price;
            }

            _tradesBuffer.Add(trade);
            if (_lastTrade.DiractionId == trade.DiractionId && Math.Abs(_lastTrade.Price - trade.Price) < 0.0001)
            {
                _lastTrade = trade;
                return;
            }

            if (trade.DiractionId == (byte)DiractionEnum.Buy)
                _tradesBuyBuffer.Add(trade);
            else if (trade.DiractionId == (byte)DiractionEnum.Sell)
                _tradesSellBuffer.Add(trade);



            Indicator.AddGlassShear(_glass, symbol, trade.TradeAdded);
            Indicator.CalcLastValue();
            if (Math.Abs(Indicator.LastAddedGlassShear.Value2) > 0.00001)
                Indicator.Values.Add(Indicator.LastAddedGlassShear);


            Indicator2.AddGlassShear(Glass, symbol, trade.TradeAdded);
            Indicator2.CalcLastValue();
            if (Math.Abs(Indicator2.LastAddedGlassShear.Value2) > 0.00001)
                Indicator2.Values.Add(Indicator2.LastAddedGlassShear);

            Indicator3.AddGlassShear(Glass, symbol, trade.TradeAdded);
            Indicator3.CalcLastValue();
            if (Math.Abs(Indicator3.LastAddedGlassShear.Value2) > 0.00001)
                Indicator3.Values.Add(Indicator3.LastAddedGlassShear);

            Indicator4.AddGlassShear(Glass, symbol, trade.TradeAdded);
            Indicator4.CalcLastValue();
            if (Math.Abs(Indicator4.LastAddedGlassShear.Value2) > 0.00001)
                Indicator4.Values.Add(Indicator4.LastAddedGlassShear);

            DoTrading(symbol, trade.Price, Indicator, Indicator2, Indicator3, Indicator4);
        }


        private void DoTrading(SmartComSymbol symbol, double price, IchIndicator indicator, IchIndicator indicator2, IchIndicator indicator3, IchIndicator indicator4)
        {
            if (!TradingEnabled || _tradingState != TradingState.Free
                || (indicator.LastAddedGlassShear.Value2 < TradeUpIndicatorLevel && indicator.LastAddedGlassShear.Value2 > TradeDownIndicatorLevel))
                return;

            var order = new TradingOrder
            {
                Symbol = symbol.Name,
                Type = StOrder_Type.StOrder_Type_Limit,
                Validity = StOrder_Validity.StOrder_Validity_Day,
                Amount = 1,
                Stop = 0,
                Cookie = 100000 + _lastOrderId + 1
            };
            _lastOrderId += 1;

            if (indicator.LastAddedGlassShear.Value2 > TradeUpIndicatorLevel && symbol?.Step != null)
            {
                _tradingState = TradingState.TryDoMainOrder;
                order.Action = StOrder_Action.StOrder_Action_Buy;
                order.Price = _lastAsk - 20*(double) symbol.Step;
            }
            else if (indicator.LastAddedGlassShear.Value2 < TradeDownIndicatorLevel && symbol?.Step != null)
            {
                _tradingState = TradingState.TryDoMainOrder;
                order.Action = StOrder_Action.StOrder_Action_Sell;
                order.Price = _lastBid + 20*(double) symbol.Step;
            }

            DataBaseClient.PlaceOrder(order);
        }

        public void QuoteEvent(SmartComSymbol symbol, SmartComQuote quote)
        {
            _lastAsk = quote.Ask;
            _lastBid = quote.Bid;

            _glass.Where(v => v.Price < quote.Bid && !v.IsBid).ForEach(v => v.IsBid = true);
            //_glass.Where(v => v.Price < quote.Bid - 50*symbol.Step).ForEach(v => v.RowId = null);

            _glass.Where(v => v.Price > quote.Ask && v.IsBid).ForEach(v => v.IsBid = false);
            //_glass.Where(v => v.Price > quote.Ask + 50*symbol.Step).ForEach(v => v.RowId = null);

            if (_lastOpenInterest.Volume != quote.OpenInterest)
            {
                _lastOpenInterest = new OpenInterest {Time = quote.QuoteAdded, Volume = quote.OpenInterest};
                OpenInterest.Add(_lastOpenInterest);
            }
        }

        public void OrderSucceeded(int cookie, string orderid)
        {
            if (cookie < 100000)
                _tradingState = TradingState.MainOrderSucceeded;
        }

        public void OrderFailed(int cookie, string orderid, string reason)
        {
            throw new NotImplementedException();
        }

        public void OrderMoveSucceeded(string orderid)
        {
            throw new NotImplementedException();
        }

        public void OrderMoveFailed(string orderid)
        {
            throw new NotImplementedException();
        }

        public void OrderCancelSucceeded(string orderid)
        {
            throw new NotImplementedException();
        }

        public void OrderCancelFailed(string orderid)
        {
            throw new NotImplementedException();
        }

        public void UpdatePosition(string portfolio, string symbol, double avprice, double amount, double planned)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(string portfolio, TradingOrder order, double filled, DateTime datetime, string orderid, string orderno,
            int status_mask)
        {
            throw new NotImplementedException();
        }

        public void AddTrade(string portfolio, string symbol, string orderid, double price, double amount, DateTime datetime,
            string tradeno)
        {
            throw new NotImplementedException();
        }

        public void ChartControlLoaded(XamDataChart chart)
        {
            _chart = chart;
        }

        public void TableViewLoaded(RoutedEventArgs eventArgs)
        {
            _glassGridControl = (eventArgs.Source as TableView).Parent as GridControl;
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

    public struct OpenInterest
    {
        public DateTime Time { get; set; }
        public int Volume { get; set; }
    }
}