using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
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
using SpeculatorServices.SmartCom;
using SpeculatorServices.Trading;
using Timer = System.Timers.Timer;

namespace Speculator.ViewModels
{
    [POCOViewModel]
    public class SymbolPanelViewModel : IDataBaseCallback
    {
        private Timer _mainTimer;
        public virtual int LotForTradeCount { get; set; }

        public virtual bool TradingEnabled
        {
            get { return _tradingEnabled; }
            set
            {
                _tradingEnabled = value;
                TradeButtonContent = value ? "НЕ ТОРГОВАТЬ" : "ТОРГОВАТЬ";
            }
        }

        public virtual string TradeButtonContent { get; set; }

        public IDataBase DataBaseClient { get; set; }
        public DataSource DataSource { get; set; }
        public Symbol Symbol { get; set; }
        public virtual short Zoom { get; set; }


        private List<TradePairInfo> AllTradePairInfo { get; set; }
        private ObservableCollection<TradingOrder> AllOrders { get; set; }
        private List<SmartComBidAskValue> _glass { get; set; }
        public virtual BindingList<SmartComBidAskValue> Glass { get; set; }

        private SmartComTrade _lastTrade { get; set; }
        public virtual ObservableCollection<SmartComTrade> Trades { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesBuy { get; set; }
        public virtual ObservableCollection<SmartComTrade> TradesSell { get; set; }

        private OpenInterest _lastOpenInterest;
        public virtual ObservableCollection<OpenInterest> OpenInterest { get; set; }

        private int _lastDeltaBetweenAskAndBid;
        public virtual ObservableCollection<AskBidCountDifferent> AskAndBidCountsDifferent { get; set; }
        public virtual int MaxAskBidCountDelta { get; set; }

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
        public virtual DateTime? HistoryDate { get; set; }

        private TradingState _tradingState;
        private double _lastAsk;
        private double _lastBid;

        private List<TradingOrder> _tradingOrders;
        private bool _tradingEnabled;
        private const int TradeUpIndicatorLevel = 300;
        private const int TradeDownIndicatorLevel = -300;
        private const int StepForOrderCookie = 100000;

        public async void StartListenDataService()
        {
            TradingEnabled = false;
            LotForTradeCount = 1;
            _tradingState = TradingState.Free;

            var dispatcher = Dispatcher.CurrentDispatcher;
            _mainTimer = new Timer(HistoryDate == DateTime.MinValue ? 200 : 300);
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
                }, DispatcherPriority.Loaded);
            };

            _mainTimer.Start();

            Zoom = 1;
            StartHeightGlassValueParam = 1;
            FinishHeightGlassValueParam = 20;
            Indicator = new IchIndicator
            {
                Parameters = new List<double> {StartHeightGlassValueParam, FinishHeightGlassValueParam}
            };
            Indicator2 = new IchIndicator
            {
                Parameters = new List<double> {StartHeightGlassValueParam, 28}
            };
            Indicator3 = new IchIndicator
            {
                Parameters = new List<double> {StartHeightGlassValueParam, 49}
            };
            Indicator4 = new IchIndicator
            {
                Parameters = new List<double> {25, 49}
            };

            OpenInterest = new ObservableCollection<OpenInterest>();
            AskAndBidCountsDifferent = new ObservableCollection<AskBidCountDifferent>();

            TradesBuy = new ObservableCollection<SmartComTrade>();
            TradesSell = new ObservableCollection<SmartComTrade>();

            _tradesBuyBuffer = new List<SmartComTrade>();
            _tradesSellBuffer = new List<SmartComTrade>();

            Glass = new BindingList<SmartComBidAskValue>();
            _glass = new List<SmartComBidAskValue>();

            AllOrders = new ObservableCollection<TradingOrder>();
            AllTradePairInfo = new List<TradePairInfo>();

            DataBaseClient = new DataBaseClient(new InstanceContext(this));

            if (HistoryDate == DateTime.MinValue)
            {
                await DataBaseClient.ConnectToDataSourceAsync();
                DataBaseClient.ListenSymbol(Symbol);
            }
            else
                await DataBaseClient.ConnectToHistoryDataSourceAsync(Symbol, HistoryDate, null, false);
        }

        public void ReturnHistoryData(SmartComSymbol symbol, HistoryDataRow[] historyData)
        {
            throw new NotImplementedException();
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
                        _glass.Add(new SmartComBidAskValue
                        {
                            Price = value.Price + i*(double) symbol.Step,
                            Volume = 0,
                            IsBid = true
                        });
                        _glass.Add(new SmartComBidAskValue
                        {
                            Price = value.Price - i*(double) symbol.Step,
                            Volume = 0,
                            IsBid = false
                        });
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

            if (trade.DiractionId == (byte) DiractionEnum.Buy)
                _tradesBuyBuffer.Add(trade);
            else if (trade.DiractionId == (byte) DiractionEnum.Sell)
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

        public void AllowTrade()
        {
            TradingEnabled = !TradingEnabled;
        }

        private void DoTrading(SmartComSymbol symbol, double price, IchIndicator indicator, IchIndicator indicator2,
            IchIndicator indicator3, IchIndicator indicator4)
        {
            if (!TradingEnabled || _tradingState != TradingState.Free || (indicator.LastAddedGlassShear.Value2 < TradeUpIndicatorLevel && indicator.LastAddedGlassShear.Value2 > TradeDownIndicatorLevel))
                return;

            var localIdOrder = (int) DateTime.Now.TimeOfDay.TotalSeconds;
            var order = new TradingOrder
            {
                Id = localIdOrder,
                Symbol = symbol.Name,
                Type = StOrder_Type.StOrder_Type_Market,
                Validity = StOrder_Validity.StOrder_Validity_Day,
                Amount = LotForTradeCount,
                Stop = 0,
                Cookie = StepForOrderCookie + localIdOrder
            };

            _tradingState = TradingState.TryDoMainOrder;

            if (indicator.LastAddedGlassShear.Value2 > TradeUpIndicatorLevel && symbol?.Step != null)
            {
                order.Action = StOrder_Action.StOrder_Action_Buy;
                //order.Price = _lastAsk + 10*(double) symbol.Step;
            }
            else if (indicator.LastAddedGlassShear.Value2 < TradeDownIndicatorLevel && symbol?.Step != null)
            {
                order.Action = StOrder_Action.StOrder_Action_Sell;
                //order.Price = _lastBid - 10*(double) symbol.Step;
            }

            AllOrders.Add(order);
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

            if (_lastOpenInterest.Amount != quote.OpenInterest)
            {
                _lastOpenInterest = new OpenInterest {Time = quote.QuoteAdded, Amount = quote.OpenInterest};
                OpenInterest.Add(_lastOpenInterest);
            }

            int lastAskBidDelta = quote.BidSize - quote.AskSize;// /quote.BidSize*100;
            if (lastAskBidDelta != _lastDeltaBetweenAskAndBid)
            {
                MaxAskBidCountDelta = Math.Max(Math.Abs(lastAskBidDelta), MaxAskBidCountDelta);

                _lastDeltaBetweenAskAndBid = lastAskBidDelta;
                AskAndBidCountsDifferent.Add(new AskBidCountDifferent {Time = quote.QuoteAdded, Volume = lastAskBidDelta});
            }

        }

        public void OrderSucceeded(int cookie, string orderid)
        {
            if (cookie/StepForOrderCookie == 1)
            {
                _tradingState = TradingState.MainOrderSucceeded;
                var order = AllOrders.FirstOrDefault(o => o.Id == cookie - StepForOrderCookie);
                if (order == null)
                    return;
                order.OrderId = orderid;
            }
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
        }

        public void OrderCancelFailed(string orderid)
        {
            throw new NotImplementedException();
        }

        public void UpdatePosition(string portfolio, string symbol, double avprice, double amount, double planned)
        {
            //throw new NotImplementedException();
        }

        public void UpdateOrder(string portfolio, TradingOrder order, double filled, DateTime datetime, string orderid, string orderno, int status_mask)
        {
            var currentOrder =
                AllOrders.FirstOrDefault(o => o.Id == order.Cookie % StepForOrderCookie);
            if (currentOrder == null)
                return;

            order.Id = currentOrder.Id;
            currentOrder.OrderNo = orderno;
            currentOrder.State = order.State;

            Task.Factory.StartNew(() =>
            {
                var counterInteration = 0;
                var tradeInfo = new TradePairInfo();
                do
                {
                    tradeInfo = AllTradePairInfo.FirstOrDefault(p => p.OrderId == orderno);
                    if (order.State == StOrder_State.StOrder_State_Filled && tradeInfo.Price == 0)
                        Thread.Sleep(50);
                    counterInteration++;
                }
                while (order.State == StOrder_State.StOrder_State_Filled && tradeInfo.Price == 0);

                if (tradeInfo.Price != 0) // если найдено совпадение
                {
                    currentOrder.OrderNo = tradeInfo.OrderId;
                    currentOrder.RealPrice = tradeInfo.Price;
                }


                switch (order.State)
                {
                    case StOrder_State.StOrder_State_Filled:
                        if (order.Cookie/StepForOrderCookie == 1)
                        {
                            // устновка первоначальных loss и profit уровней
                            _tradingState = TradingState.MainOrderSucceeded;
                            DoProfitAndLossLimits(order, currentOrder.RealPrice);
                            DoProfitAndLossLimits(order, currentOrder.RealPrice, isLoss: true);
                        }
                        else if (order.Cookie/StepForOrderCookie == 2)
                        {
                            // ProfitShot(); если исполнилась профитная лимитка
                            var parentOrder = AllOrders.First(o => o.Id == currentOrder.ParentId);
                            var lossOrder =
                                AllOrders.First(o => o.ParentId == parentOrder.Id && o.Cookie/StepForOrderCookie == 3);
                            DataBaseClient.CancelOrder(Symbol.Name, lossOrder.OrderNo);
                        }
                        else if (order.Cookie/StepForOrderCookie == 3)
                        {
                            //LossShot(); если исполнилась лимитка на доливку/усреднение
                            var parentOrder = AllOrders.First(o => o.Id == currentOrder.ParentId);
                            var profitOrder =
                                AllOrders.First(o => o.ParentId == parentOrder.Id && o.Cookie/StepForOrderCookie == 2);
                            DataBaseClient.CancelOrder(Symbol.Name, profitOrder.OrderNo);
                        }
                        else if (order.Cookie/StepForOrderCookie == 4)
                        {
                            //если это профит после доливки
                        }
                        else if (order.Cookie/StepForOrderCookie == 5)
                        {
                            // если это лосс после доливки
                        }
                        break;
                    case StOrder_State.StOrder_State_ContragentReject:
                        break;
                    case StOrder_State.StOrder_State_Submited:
                        break;
                    case StOrder_State.StOrder_State_Pending:
                        break;
                    case StOrder_State.StOrder_State_Open:
                        break;
                    case StOrder_State.StOrder_State_Expired:
                        break;
                    case StOrder_State.StOrder_State_Cancel:
                        if (currentOrder.Cookie / StepForOrderCookie == 1)
                        {
                        }
                        else if (currentOrder.Cookie / StepForOrderCookie == 2)
                        {
                            var localIdOrder = GetCookieForNewOrder();
                            var newOrder = new TradingOrder
                            {
                                Id = localIdOrder,
                                ParentId = currentOrder.ParentId,
                                Symbol = Symbol.Name,
                                Type = StOrder_Type.StOrder_Type_Limit,
                                Validity = StOrder_Validity.StOrder_Validity_Day,
                                Amount = 2 * LotForTradeCount,
                                Cookie = 4 * StepForOrderCookie + localIdOrder,
                                Price = currentOrder.RealPrice
                            };

                            if (currentOrder.Action == StOrder_Action.StOrder_Action_Buy)
                                newOrder.Action = StOrder_Action.StOrder_Action_Sell;
                            else if (currentOrder.Action == StOrder_Action.StOrder_Action_Sell)
                                newOrder.Action = StOrder_Action.StOrder_Action_Buy;

                            AllOrders.Add(newOrder);
                            DataBaseClient.PlaceOrder(newOrder);
                        }
                        else if (currentOrder.Cookie / StepForOrderCookie == 3)
                        {
                            //_tradingState = TradingState.Free;
                        }
                        else if (currentOrder.Cookie / StepForOrderCookie == 4)
                        {
                        }
                        else if (currentOrder.Cookie / StepForOrderCookie == 5)
                        {
                        }
                        break;
                    case StOrder_State.StOrder_State_Partial:
                        break;
                    case StOrder_State.StOrder_State_ContragentCancel:
                        break;
                    case StOrder_State.StOrder_State_SystemReject:
                        break;
                    case StOrder_State.StOrder_State_SystemCancel:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        public void AddTrade(string portfolio, string symbol, string orderid, double price, double amount, DateTime datetime, string tradeno)
        {
            AllTradePairInfo.Add(new TradePairInfo
            {
                OrderId = orderid,
                TradeNo = tradeno,
                Price = price,
                Amount = amount
            });
        }

        private void DoProfitAndLossLimits (TradingOrder order, double realPrice, bool isLoss = false)
        {
            int localIdOrder = GetCookieForNewOrder();

            var newOrder = new TradingOrder
            {
                Id = localIdOrder,
                ParentId = order.Id,
                Symbol = Symbol.Name,
                Type = StOrder_Type.StOrder_Type_Limit,
                Validity = StOrder_Validity.StOrder_Validity_Day,
                Amount = isLoss ? 2*LotForTradeCount : LotForTradeCount,
                Cookie = (isLoss ? 3*StepForOrderCookie : 2*StepForOrderCookie) + localIdOrder
            };

            if (order.Action == StOrder_Action.StOrder_Action_Buy)
            {
                newOrder.Action = isLoss ? StOrder_Action.StOrder_Action_Buy : StOrder_Action.StOrder_Action_Sell;
                newOrder.Price = realPrice + (isLoss ? -3 : 1)*6*(double) Symbol.Step;
            }
            else if (order.Action == StOrder_Action.StOrder_Action_Sell)
            {
                newOrder.Action = isLoss ? StOrder_Action.StOrder_Action_Sell : StOrder_Action.StOrder_Action_Buy;
                newOrder.Price = realPrice - (isLoss ? -3 : 1)*6*(double) Symbol.Step;
            }
            AllOrders.Add(newOrder);
            DataBaseClient.PlaceOrder(newOrder);
        }

        private int GetCookieForNewOrder ()
        {
            var localIdOrder = (int) DateTime.Now.TimeOfDay.TotalSeconds;
            while (AllOrders.Any(o => o.Id == localIdOrder))
            {
                localIdOrder++;
            }
            return localIdOrder;
        }

        public void ChartControlLoaded (XamDataChart chart)
        {
            _chart = chart;
        }

        public void TableViewLoaded (RoutedEventArgs eventArgs)
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

        private void GlassScrollViewer_ScrollChanged (object sender, ScrollChangedEventArgs e)
        {
            var tableView = (e.Source as ScrollViewer)?.TemplatedParent as TableView;
            var gridControl = tableView?.Parent as GridControl;

            if ((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) > 0 ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) > 0)
            {
                Zoom += (short) (Math.Sign(e.VerticalChange)*1);
                if (Zoom < 1)
                    Zoom = 1;
                else if (Zoom > 4)
                    Zoom = 4;
                gridControl?.RefreshData();
            }

            GetVisibleRowsOnScreen(gridControl, tableView, e.ViewportHeight, e.VerticalOffset);
        }


        public void GetVisibleRowsOnScreen (GridControl grid, TableView view, double viewPortHeight, double verticalOffset)
        {
            if (grid == null)
                return;
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

    public struct TradePairInfo
    {
        public string OrderId;
        public string TradeNo;
        public double Price;
        public double Amount;
    }

    public struct OpenInterest
    {
        public DateTime Time { get; set; }
        public int Amount { get; set; }
    }

    public struct AskBidCountDifferent
    {
        public DateTime Time { get; set; }
        public int Volume { get; set; }
    }
}