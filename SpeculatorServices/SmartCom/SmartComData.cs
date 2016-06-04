using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using SmartCOM3Lib;
using SpeculatorModel;
using SpeculatorModel.SmartCom;
using SpeculatorServices.Properties;

namespace SpeculatorServices.SmartCom
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SmartComData : ISmartComData
    {
        private const string SuffixSymbols = "-6.16_FT";
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

        public void ConnectToSmartCom()
        {
            _symbolsForSaveToDb = _symbolsForSaveToDb.Select(symb =>
            {
                symb = symb + SuffixSymbols;
                return symb;
            }).ToList();

            // фьючерс на нефть
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
            var currentSymbol = _symbolsInJob.Single(s => s.Symbol == symbol);
            var newBid = new SmartComBidAskValue
            {
                SmartComSymbolId = currentSymbol.Id,
                IsBid = true,
                RowId = (byte) row,
                Price = bid, Volume = (int)bidsize
            };
            var newAsk = new SmartComBidAskValue
            {
                SmartComSymbolId = currentSymbol.Id,
                IsBid = false,
                RowId = (byte) row,
                Price = ask,
                Volume = (int) asksize
            };

            var bidChanged = false;
            var askChanged = false;

            _glasses[currentSymbol.Symbol].GetOrAdd(bid, new SmartComBidAskValue());
            _glasses[currentSymbol.Symbol].GetOrAdd(ask, new SmartComBidAskValue());

            var oldBid = _glasses[currentSymbol.Symbol][bid];
            var oldAsk = _glasses[currentSymbol.Symbol][ask];

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
            var currentSymbol = _symbolsInJob.Single(s => s.Symbol == symbol);
            long tradenoLong;
            long.TryParse(tradeno, out tradenoLong);

            var tick = new SmartComTick
            {
                SmartComSymbolId = currentSymbol.Id,
                TradeNo = tradenoLong,
                Price = price,
                Volume = (int) volume,
                OrderAction = (byte) action,
                TradeDateTime = datetime
            };
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
            var currentSymbol = _symbolsInJob.Single(s => s.Symbol == symbol);

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
            using (var dbContext = new SpeculatorContext())
            {
                dbContext.SmartComQuotes.Add(quote);
                dbContext.SaveChanges();
            }}

        private async void _smartCom_AddSymbol(int row, int nrows, string symbol, string shortName, string longName, string type, int decimals, int lotSize, double punkt, double step, string secExtId, string secExchName, DateTime expiryDate, double daysBeforeExpiry, double strike)
        {
            var currentSymbol = new SmartComSymbol
            {
                Symbol = symbol,
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

            if (_smartComSymbols.All(s => s.Symbol != currentSymbol.Symbol))
                _dbContext.SmartComSymbols.Add(currentSymbol);

            if (row != nrows - 1) return;
            await _dbContext.SaveChangesAsync();
            _smartComSymbols = _dbContext.SmartComSymbols.ToList();
            _symbolsInJob = _smartComSymbols.Where(scs => _symbolsForSaveToDb.Contains(scs.Symbol)).DistinctBy(scs => scs.Symbol).ToList();
            _symbolsInJob.ForEach(s =>
                {
                    _glasses.GetOrAdd(s.Symbol, new ConcurrentDictionary<double, SmartComBidAskValue>());

                    _smartCom.ListenTicks(s.Symbol);
                    _smartCom.ListenQuotes(s.Symbol);
                    _smartCom.ListenBidAsks(s.Symbol);
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
