using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel
{
    [DataContract, Table("SmartComQuotes")]
    public class SmartComQuote
    {
        [DataMember, Key]
        public int Id { get; set; }
        [DataMember]
        public int SmartComSymbolId { get; set; }
        [DataMember]
        public double LastTradePrice { get; set; }
        [DataMember] 
        public int LastTradeVolume { get; set; }
        [DataMember]
        public double Bid { get; set; }
        [DataMember]
        public double Ask { get; set; }
        [DataMember]
        public int BidSize { get; set; }
        [DataMember]
        public int AskSize { get; set; }
        [DataMember]
        public int OpenInterest { get; set; }
        [DataMember]
        public double Volatility { get; set; }
        [DataMember, Column(TypeName = "datetime2")]
        public DateTime LastTradeDateTime { get; set; }
        [DataMember, Column(TypeName = "datetime2")]
        public DateTime QuoteAdded { get; set; }

        [DataMember]
        public virtual SmartComSymbol SmartComSymbol { get; set; }
    }
}
