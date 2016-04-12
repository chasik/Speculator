﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using SpeculatorService.Properties;
using SmartCOM3Lib;
using SpeculatorModel;

namespace SpeculatorServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SmartComData : ISmartComData
    {
        private const string SuffixSymbols = "-6.16_FT";
        private List<string> _symbolsForSaveToDb = new List<string> {"RTS", "Si", "Eu", "ED", "SBRF", "LKOH", "GAZR", "ROSN", "VTBR", "GOLD"};
        private List<SmartComSymbol> _symbolsInJob;
        private Dictionary<string, Dictionary<double, double>> _glasses = new Dictionary<string, Dictionary<double, double>>();

        private StServerClass _smartCom;
        private SpeculatorContext _dbContext;
        private List<SmartComSymbol> _smartComSymbols;
        private int _smartComBidOrAskAddedCounter;

        public void ConnectToSmartCom()
        {
            _symbolsForSaveToDb = _symbolsForSaveToDb.Select(symb =>
            {
                symb = symb + SuffixSymbols;
                return symb;
            }).ToList();

            _smartCom = new StServerClass();
            _smartCom.Connected += SmartCom_Connected;
            _smartCom.Disconnected += SmartCom_Disconnected;
            _smartCom.AddSymbol += _smartCom_AddSymbol;

            _smartCom.UpdateQuote += _smartCom_UpdateQuote;
            _smartCom.UpdateBidAsk += _smartCom_UpdateBidAsk;
            _smartCom.AddTick += _smartCom_AddTick;

            _dbContext = new SpeculatorContext();
            _smartComSymbols = _dbContext.SmartComSymbols.ToList();

            _smartCom.connect(Settings.Default.SmartComHost, Settings.Default.SmartComPort, Settings.Default.SmartComLogin, Settings.Default.SmartComPassword);
        }

        private void _smartCom_UpdateBidAsk(string symbol, int row, int nrows, double bid, double bidsize, double ask, double asksize)
        {
            var currentSymbol = _symbolsInJob.Single(s => s.Symbol == symbol);
            var newBid = new SmartComBidAskValue { SmartComSymbolId = currentSymbol.Id, IsBid = true, RowId = (byte)row, Price = bid, Volume = (int)bidsize };
            var newAsk = new SmartComBidAskValue { SmartComSymbolId = currentSymbol.Id, IsBid = false, RowId = (byte)row, Price = ask, Volume = (int)asksize };

            var bidChanged = true;
            var askChanged = true;

            var oldBid = _glasses[currentSymbol.Symbol]?[bid];
            var oldAsk = _glasses[currentSymbol.Symbol]?[ask];

            if (oldBid != bidsize) bidChanged = false;
            if (oldAsk != asksize) askChanged = false;

            if (bidChanged)
                _dbContext.SmartComBidAskValues.Add(newBid);
            if (askChanged)
                _dbContext.SmartComBidAskValues.Add(newAsk);

            _smartComBidOrAskAddedCounter++;
            if (_smartComBidOrAskAddedCounter%200 == 0)
                _dbContext.SaveChangesAsync();
        }

        private void _smartCom_AddTick(string symbol, DateTime datetime, double price, double volume, string tradeno,
            [System.Runtime.InteropServices.ComAliasName("SmartCOM3Lib.StOrder_Action")] StOrder_Action action)
        {
            throw new NotImplementedException();
        }

        private void _smartCom_UpdateQuote(string symbol, DateTime datetime, double open, double high, double low,
            double close, double last, double volume, double size, double bid, double ask, double bidsize,
            double asksize, double openInt, double goBuy, double goSell, double goBase, double goBaseBacked,
            double highLimit, double lowLimit, int tradingStatus, double volat, double theorPrice)
        {
            throw new NotImplementedException();
        }

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
                    //_smartCom.ListenTicks(s.Symbol);
                    //_smartCom.ListenQuotes(s.Symbol);
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
