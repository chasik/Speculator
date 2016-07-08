using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using SmartCOM3Lib;
using SpeculatorModel;
using SpeculatorModel.MainData;
using SpeculatorModel.SmartCom;
using SpeculatorServices.Properties;

namespace SpeculatorServices.SmartCom
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SmartComData : DataServiceBase, ISmartComData, IDataBase
    {
        public bool OnlyDuplexForClient { get; private set; }
        private const string SuffixSymbols = "-9.16_FT";
        private const string SuffixSymbolsForOil = "-8.16_FT";
        private List<string> _symbolsForSaveToDb = new List<string> {"RTS", "Si", "Eu", "ED", "SBRF", "LKOH", "GAZR", "ROSN", "VTBR", "GOLD"};
        private List<SmartComSymbol> _symbolsInJob;

        private readonly ConcurrentDictionary<string, ConcurrentDictionary<double, SmartComBidAskValue>> _glasses =
            new ConcurrentDictionary<string, ConcurrentDictionary<double, SmartComBidAskValue>>();

        private StServerClass _smartCom;
        private SpeculatorContext _dbContext;
        private List<SmartComSymbol> _smartComSymbols;

        static SmartComData()
        {
            #if !DEBUG
                if (!EventLog.SourceExists("SmartComDataServiceHost"))
                    EventLog.CreateEventSource("SmartComDataServiceHost", "Application");
                EventLog.WriteEntry("SmartComDataServiceHost", $"Run Service Release Mode!{Settings.Default.SmartComHost} {Settings.Default.SmartComPort} {Settings.Default.SmartComLogin} {Settings.Default.SmartComPassword}");
                new SmartComData().ConnectToSmartCom();
            #endif
        }

        public SmartComData() : base()
        {
            
        }

        public void ConnectToDataSource()
        {
            OnlyDuplexForClient = true;
            ConnectToSmartCom();
        }

        public void ConnectToHistoryDataSource(Symbol symbol, DateTime dayDateTime)
        {
            var smartComSymbol = new SmartComSymbol
            {
                Id = symbol.Id,
                Name = symbol.Name,
                Step = symbol.Step,
                Punkt = symbol.Punkt,
                LongName = symbol.LongName,
                LotSize = symbol.LotSize,
                ShortName = symbol.ShortName
            };
            RegisterClientWithCallBack(new[] { symbol.Name });
            List<SmartComTrade> allTicks;
            List<SmartComBidAskValue> allBidAsk;
            using (var dbContext = new SpeculatorContext())
            {
                allTicks = dbContext.SmartComTicks.Where(t =>
                    t.SmartComSymbolId == symbol.Id && t.TradeAdded >= dayDateTime &&
                    t.TradeAdded <= dayDateTime.AddDays(1)).OrderBy(t => t.TradeAdded).ToList();
                allBidAsk = dbContext.SmartComBidAskValues.Where(ba =>
                    ba.SmartComSymbolId == symbol.Id && ba.Added >= dayDateTime &&
                    ba.Added <= dayDateTime.AddDays(1)).OrderBy(ba => ba.Added).ToList();


                var leftOuterJoin =
                    from tick in allTicks
                    join bidask in allBidAsk on tick.TradeAdded equals bidask.Added into temp
                    from bidask in temp.DefaultIfEmpty()
                    select new {EventDateTime = tick.TradeAdded, Tick = tick, BidAsk = bidask ?? new SmartComBidAskValue()};

                var rightOuterJoin =
                    from bidask in allBidAsk
                    join tick in allTicks on bidask.Added equals tick.TradeAdded into temp
                    from tick in temp.DefaultIfEmpty()
                    select new {EventDateTime = bidask.Added, Tick = tick ?? new SmartComTrade(), BidAsk = bidask};

                var fullOuterJoin = leftOuterJoin.Union(rightOuterJoin).OrderBy(x => x.EventDateTime);

                var fullResultEvents = fullOuterJoin.ToList();




                var minDateTime = new DateTime(Math.Min(allTicks.Min(t => t.TradeAdded).Ticks, allBidAsk.Min(ba => ba.Added).Ticks));
                var maxDateTime = new DateTime(Math.Max(allTicks.Max(t => t.TradeAdded).Ticks, allBidAsk.Max(ba => ba.Added).Ticks));

                fullResultEvents.ForEach(smartComEvent =>
                {
                    if (smartComEvent.Tick.TradeNo != 0)
                        TradeEvent(smartComSymbol, smartComEvent.Tick);
                    if (smartComEvent.BidAsk.Id != 0)
                        UpdateBidAskEvent(smartComSymbol, smartComEvent.BidAsk, smartComEvent.BidAsk.IsBid);
                });

            }
        }

        public void DefaultOperation()
        {
            ConnectToSmartCom();
        }

        public void ListenSymbol(Symbol symbol)
        {
            RegisterClientWithCallBack(new[] { symbol.Name });
            RunListenSymbolEvents(new List<SmartComSymbol>
            {
                new SmartComSymbol
                {
                    Name = symbol.Name,
                    Step = symbol.Step,
                    LotSize = symbol.LotSize,
                    Punkt = symbol.Punkt
                }
            });
        }

        public void ConnectToSmartCom()
        {
            if (_smartCom != null)
                return;

            // добавляем для записи в базу инструменты
            _symbolsForSaveToDb = _symbolsForSaveToDb.Select(symb =>
            {
                symb = symb + SuffixSymbols;
                return symb;
            }).ToList();

            // и отдельно добави фьючерс на нефть
            _symbolsForSaveToDb.Add("BR" + SuffixSymbolsForOil);

            _smartCom = new StServerClass();
            _smartCom.Connected += SmartCom_Connected;
            _smartCom.Disconnected += SmartCom_Disconnected;
            _smartCom.AddSymbol += _smartCom_AddSymbol;

            _smartCom.UpdateQuote += _smartCom_UpdateQuote;
            _smartCom.UpdateBidAsk += _smartCom_UpdateBidAsk;
            _smartCom.AddTick += _smartCom_AddTick;

            _dbContext = new SpeculatorContext();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            _smartComSymbols = _dbContext.SmartComSymbols.ToList();

            if (!Directory.Exists(Settings.Default.LogPathSmartCom)) 
                Directory.CreateDirectory(Settings.Default.LogPathSmartCom);

            _smartCom.ConfigureClient("logFilePath=" + Settings.Default.LogPathSmartCom);
            _smartCom.ConfigureServer("logFilePath=" + Settings.Default.LogPathSmartCom);
            _smartCom.connect(Settings.Default.SmartComHost, Settings.Default.SmartComPort, Settings.Default.SmartComLogin, Settings.Default.SmartComPassword);
            //throw new FaultException($"!{Settings.Default.SmartComHost} {Settings.Default.SmartComPort} {Settings.Default.SmartComLogin} {Settings.Default.SmartComPassword}");
            if (!EventLog.SourceExists("SmartComDataServiceHost"))
                EventLog.CreateEventSource("SmartComDataServiceHost", "Application");
             EventLog.WriteEntry("SmartComDataServiceHost", $"!{Settings.Default.SmartComHost} {Settings.Default.SmartComPort} {Settings.Default.SmartComLogin} {Settings.Default.SmartComPassword}");
        }

        private void _smartCom_UpdateBidAsk(string symbol, int row, int nrows, double bid, double bidsize, double ask, double asksize)
        {
            var currentSymbol = _symbolsInJob.Single(s => s.Name == symbol);
            var newBid = new SmartComBidAskValue
            {
                Added = DateTime.Now,
                SmartComSymbolId = currentSymbol.Id,
                IsBid = true,
                RowId = (byte) row,
                Price = bid,
                Volume = (int)bidsize
            };
            var newAsk = new SmartComBidAskValue
            {
                Added = DateTime.Now,
                SmartComSymbolId = currentSymbol.Id,
                IsBid = false,
                RowId = (byte) row,
                Price = ask,
                Volume = (int) asksize
            };

            var bidChanged = false;
            var askChanged = false;

            var oldBid = _glasses[currentSymbol.Name].GetOrAdd(bid, new SmartComBidAskValue());
            var oldAsk = _glasses[currentSymbol.Name].GetOrAdd(ask, new SmartComBidAskValue());

            if (oldBid != null && (Math.Abs(oldBid.Volume - bidsize) > 0.00001 || !oldBid.IsBid))
            {
                oldBid.Volume = (int)bidsize;
                oldBid.IsBid = true;
                bidChanged = true;
            }
            if (oldAsk != null && (Math.Abs(oldAsk.Volume - asksize) > 0.00001 || oldAsk.IsBid))
            {
                oldAsk.Volume = (int)asksize;
                oldAsk.IsBid = false;
                askChanged = true;
            }

            if (bidChanged)
                UpdateBidAskEvent(currentSymbol, newBid, isBid: true);
            if (askChanged)
                UpdateBidAskEvent(currentSymbol, newAsk, isBid: false);
            //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom UpdateBidAsk. Symbol: {currentSymbol.Name}    OnlyDubplexForClient:{OnlyDuplexForClient}");
            if (OnlyDuplexForClient)
                return;

            using (var dbContext = new SpeculatorContext())
            {
                if (bidChanged)
                    dbContext.SmartComBidAskValues.Add(newBid);
                if (askChanged)
                    dbContext.SmartComBidAskValues.Add(newAsk);
                dbContext.SaveChanges();
                //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom UpdateBidAsk SAVED. Symbol: {currentSymbol.Name}    OnlyDubplexForClient:{OnlyDuplexForClient}");
            }
        }

        private void _smartCom_AddTick(string symbol, DateTime datetime, double price, double volume, string tradeno,
            [System.Runtime.InteropServices.ComAliasName("SmartCOM3Lib.StOrder_Action")] StOrder_Action action)
        {
            var currentSymbol = _symbolsInJob.Single(s => s.Name == symbol);
            long tradenoLong;
            long.TryParse(tradeno, out tradenoLong);

            var tick = new SmartComTrade
            {
                SmartComSymbolId = currentSymbol.Id,
                TradeNo = tradenoLong,
                Price = price,
                Volume = (int) volume,
                DiractionId = (byte) action,
                TradeDateTime = datetime,
                TradeAdded = DateTime.Now
            };

            TradeEvent(currentSymbol, tick);
            //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom AddTick. Symbol: {currentSymbol.Name}    OnlyDubplexForClient:{OnlyDuplexForClient}");
            if (OnlyDuplexForClient)
                return;

            using (var dbContext = new SpeculatorContext())
            {
                dbContext.SmartComTicks.Add(tick);
                dbContext.SaveChanges();
                //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom AddTick SAVED. Symbol: {currentSymbol.Name}    OnlyDubplexForClient:{OnlyDuplexForClient}");
            }
        }

        private void _smartCom_UpdateQuote(string symbol, DateTime datetime, double open, double high, double low,
            double close, double last, double volume, double size, double bid, double ask, double bidsize,
            double asksize, double openInt, double goBuy, double goSell, double goBase, double goBaseBacked,
            double highLimit, double lowLimit, int tradingStatus, double volat, double theorPrice)
        {
            var currentSymbol = _symbolsInJob.Single(s => s.Name == symbol);

            var quote = new SmartComQuote
            {
                SmartComSymbolId = currentSymbol.Id,
                LastTradeDateTime = datetime,
                LastTradePrice = last,
                LastTradeVolume = (int) size,
                Bid = bid,
                Ask = ask,
                BidSize = (int) bidsize,
                AskSize = (int) asksize,
                OpenInterest = (int) openInt,
                Volatility = volat
            };
            //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom UpdateQuote. Symbol: {currentSymbol.Name}    OnlyDubplexForClient:{OnlyDuplexForClient}");
            if (OnlyDuplexForClient)
                return;

            using (var dbContext = new SpeculatorContext())
            {
                dbContext.SmartComQuotes.Add(quote);
                dbContext.SaveChanges();
                //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom UpdateQuote SAVED. Symbol: {currentSymbol.Name}    OnlyDubplexForClient:{OnlyDuplexForClient}");
            }
        }

        private async void _smartCom_AddSymbol(int row, int nrows, string symbol, string shortName, string longName, string type, int decimals, int lotSize, double punkt, double step, string secExtId, string secExchName, DateTime expiryDate, double daysBeforeExpiry, double strike)
        {
            if (OnlyDuplexForClient)
                return;

            // если это запущена служба для записи данных в базу, то работаем с полученным инструментом
            var currentSymbol = new SmartComSymbol
            {
                Name = symbol,
                ShortName = shortName,
                LongName = longName,
                Type = type,
                Decimals = decimals,
                LotSize = lotSize,
                Punkt = punkt,
                SecExtId = secExtId,
                SecExchName = secExchName,
                ExpiryDate = expiryDate,
                Strike = strike,
                Step = double.IsNaN(step) ? (double?) null : step
            };

            if (_smartComSymbols.All(s => s.Name != currentSymbol.Name))
                _dbContext.SmartComSymbols.Add(currentSymbol);

            if (row != nrows - 1)
                return;
            await _dbContext.SaveChangesAsync();
            _smartComSymbols = _dbContext.SmartComSymbols.ToList();
            _symbolsInJob = _smartComSymbols.Where(scs => _symbolsForSaveToDb.Contains(scs.Name)).DistinctBy(scs => scs.Name).ToList();

            RunListenSymbolEvents(_symbolsInJob);
        }

        private void RunListenSymbolEvents(List<SmartComSymbol> symbols)
        {
            //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom RunListenSymbolEvents. SymbolsCount: {symbols.Count}");
            if (_symbolsInJob == null)
                _symbolsInJob = new List<SmartComSymbol>(symbols);
            else 
                _symbolsInJob.AddRange(symbols.Where(s => _symbolsInJob.All(sinjob => sinjob.Name != s.Name)));

            symbols.ForEach(s =>
            {
                //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom RunListenSymbolEvents in foreach. Symbol: {s.Name + " " + s.ShortName}");
                _glasses.GetOrAdd(s.Name, new ConcurrentDictionary<double, SmartComBidAskValue>());
                var error = false;
                do
                {
                    error = false;
                    try
                    {
                        //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom RunListenSymbolEvents Listen. Symbol: {s.Name + " " + s.ShortName}");
                        _smartCom.ListenTicks(s.Name);
                        _smartCom.ListenQuotes(s.Name);
                        _smartCom.ListenBidAsks(s.Name);
                        //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom RunListenSymbolEvents Listen-2. Symbol: {s.Name}");
                    }
                    catch (Exception)
                    {
                        //EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom RunListenSymbolEvents Listen Error!!!. Symbol: {s.Name + " " + s.ShortName}");
                        error = true;
                        Thread.Sleep(2000);
                    }
                } while (error);
            });
        }

        private void SmartCom_Disconnected(string reason)
        {
            EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom Discronnected event. Reason: {reason}");
            throw new NotImplementedException();
        }

        private void SmartCom_Connected()
        {
            EventLog.WriteEntry("SmartComDataServiceHost", $"SmartCom Connected event!");
            _smartCom.GetSymbols();
        }
    }

    public static class SmartComExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }
}
