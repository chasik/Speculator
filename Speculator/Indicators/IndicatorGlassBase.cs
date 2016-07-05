using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SpeculatorModel.MainData;
using SpeculatorModel.SmartCom;

namespace Speculator.Indicators
{
    public class IndicatorGlassBase
    {
        private DateTime LastAddedTime { get; set; }
        public Symbol Symbol { get; set; }
        public ObservableCollection<GlassShear> Values { get; set; }
        public GlassShear LastAddedGlassShear { get; set; }
        public List<double> Parameters { get; set; }

        public IndicatorGlassBase()
        {
            Values = new ObservableCollection<GlassShear>();
        }

        public void AddGlassShear(ICollection<SmartComBidAskValue> glass, Symbol symbol)
        {
            LastAddedGlassShear = new GlassShear(glass, symbol);
        }
    }


    public class GlassShear
    {
        public GlassShear(ICollection<SmartComBidAskValue> glass, Symbol symbol)
        {
            if (glass.Count < 20)
                return;

            Time = DateTime.Now;

            var ask = glass.Where(g => !g.IsBid && g.RowId == 0).Max(g => g.Price);
            var bid = glass.Where(g => g.IsBid && g.RowId == 0).Min(g => g.Price);
            if (Math.Abs(ask - bid) > 200)
            {
                ask = glass.Where(g => !g.IsBid && g.RowId == 0).Min(g => g.Price);
                bid = glass.Where(g => g.IsBid && g.RowId == 0).Max(g => g.Price);
            }

            //var askIndex = Glass.FindIndex(g => Math.Abs(g.Price - ask) < 0.001);
            //var bidIndex = Glass.FindIndex(g => Math.Abs(g.Price - bid) < 0.001);

            // выбрали и упорядочили
            AskValues = glass.Where(g => g.Price >= ask && g.Price <= ask + 50 * symbol.Step).OrderBy(g => g.Price).ToDictionary(g => g.Price, g => g.Volume);
            BidValues = glass.Where(g => g.Price <= bid && g.Price >= bid - 50 * symbol.Step).OrderByDescending(g => g.Price).ToDictionary(g => g.Price, g => g.Volume);
        }

        public Dictionary<double, int> AskValues { get; set; }

        public Dictionary<double, int> BidValues { get; set; }

        public List<int> AverageValues { get; set; }

        public DateTime Time { get; set; }

        public double Value { get; set; }
        public double Value2 { get; set; }

        public int AverageFull { get; set; }

        public double GetAskAverage(byte count)
        {
            return AskValues != null && AskValues.Any() ? AskValues.Take(count).Average(a => a.Value) : 0;
        }

        public double GetBidAverage(byte count)
        {
            return BidValues != null && BidValues.Any() ? BidValues.Take(count).Average(b => b.Value) : 0;
        }

        public void CalcAverageForEachRow()
        {
            if (AskValues == null || BidValues == null)
                return;

            AverageValues = new List<int>();

            var averageSummAsk = 0;
            var averageSummBid = 0;

            AverageFull = (AskValues.Take(50).Sum(ask => ask.Value) + BidValues.Take(50).Sum(bid => bid.Value)) / 100;

            for (var i = 0; i < 50; i++)
            {
                //averageSummAsk += (int)AskValues.Take(i).Average(a => a.Value < AverageFull * 3 ? a.Value : AverageFull * 3);
                //averageSummBid += (int)BidValues.Take(i).Average(a => a.Value < AverageFull * 3 ? a.Value : AverageFull * 3);

                var ask = AskValues.Skip(i).FirstOrDefault().Value;
                var bid = BidValues.Skip(i).FirstOrDefault().Value;

                averageSummAsk += ask < AverageFull * 5 ? ask : AverageFull * 5;
                averageSummBid += bid < AverageFull * 5 ? bid : AverageFull * 5;

                var summ = averageSummAsk + averageSummBid;
                if (summ == 0)
                    continue;
                var value = (averageSummAsk - averageSummBid)*100/summ;
                //if (AverageValues.Count > 0)
                //    AverageValues.Add((value + (int)AverageValues.Average())/2);
                //else 
                    AverageValues.Add(value);
            }
        }
    }
}
