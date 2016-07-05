using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SpeculatorModel.MainData;

namespace SpeculatorModel.SmartCom
{
    [DataContract, Table("SmartComTrades")]
    public class SmartComTrade : Trade
    {
        [DataMember]
        public int SmartComSymbolId { get; set; }

        [DataMember, Column(TypeName = "datetime2")]
        public DateTime TradeDateTime { get; set; }

        [DataMember, Column(TypeName = "datetime2")]
        public DateTime TradeAdded { get; set; }


        [DataMember]
        public virtual SmartComSymbol SmartComSymbol { get; set; }
    }
}
