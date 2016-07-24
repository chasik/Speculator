using System;
using SpeculatorModel.SmartCom;

namespace SpeculatorServices.SmartCom
{
    public class HistoryDataRow
    {
        public DateTime EventDateTime { get; set; }
        public SmartComTrade Tick { get; set; }
        public SmartComBidAskValue BidAsk { get; set; }
        public SmartComQuote Quote { get; set; }
    }
}
