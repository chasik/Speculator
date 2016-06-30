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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class SmartComData : DataServiceBase, ISmartComData, IDataBase
    {
        public bool OnlyDuplexForClient { get; private set; }
        private const string SuffixSymbols = "-9.16_FT";
        private const string SuffixSymbolsForOil = "-7.16_FT";
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

            _glasses[currentSymbol.Name].GetOrAdd(bid, new SmartComBidAskValue());
            _glasses[currentSymbol.Name].GetOrAdd(ask, new SmartComBidAskValue());

            var oldBid = _glasses[currentSymbol.Name][bid];
            var oldAsk = _glasses[currentSymbol.Name][ask];

            if ((oldBid?.Volume != bidsize || !oldBid.IsBid) && oldBid != null)
            {
                oldBid.Volume = (int)bidsize;
                oldBid.IsBid = true;
                bidChanged = true;
            }
            if ((oldAsk?.Volume != asksize || oldAsk.IsBid) && oldAsk != null)
            {
                oldAsk.Volume = (int)asksize;
                oldAsk.IsBid = false;
                askChanged = true;
            }

            if (bidChanged)
                UpdateBidAskEvent(currentSymbol, newBid, isBid: true);
            if (askChanged)
                UpdateBidAskEvent(currentSymbol, newAsk, isBid: false);

            if (OnlyDuplexForClient)
                return;

            using (var dbContext = new SpeculatorContext())
            {
                if (bidChanged)
                    dbContext.SmartComBidAskValues.Add(newBid);
                if (askChanged)
                    dbContext.SmartComBidAskValues.Add(newAsk);
                dbContext.SaveChanges();
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

            if (OnlyDuplexForClient)
                return;

            using (var dbContext = new SpeculatorContext())
            {
                dbContext.SmartComTicks.Add(tick);
                dbContext.SaveChanges();
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

            if (OnlyDuplexForClient)
                return;

            using (var dbContext = new SpeculatorContext())
            {
                dbContext.SmartComQuotes.Add(quote);
                dbContext.SaveChanges();
            }}

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
            if (_symbolsInJob == null)
                _symbolsInJob = new List<SmartComSymbol>(symbols);
            else
                _symbolsInJob.AddRange(symbols);

            symbols.ForEach(s =>
            {
                _glasses.GetOrAdd(s.Name, new ConcurrentDictionary<double, SmartComBidAskValue>());
                var error = false;
                do
                {
                    error = false;
                    try
                    {
                        _smartCom.ListenTicks(s.Name);
                        _smartCom.ListenQuotes(s.Name);
                        _smartCom.ListenBidAsks(s.Name);
                    }
                    catch (Exception)
                    {
                        error = true;
                        Thread.Sleep(2000);
                    }
                } while (error);
            });
        }

        private void SmartCom_Disconnected(string reason)
        {
            throw new NotImplementedException();
        }

        private void SmartCom_Connected()
        {
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
