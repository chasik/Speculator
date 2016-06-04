using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.SmartCom
{
    [DataContract, Table("SmartComTicks")]
    public class SmartComTick
    {
        [DataMember, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TradeNo { get; set; }

        [DataMember]
        public int SmartComSymbolId { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public int Volume { get; set; }

        [DataMember]
        public byte OrderAction { get; set; }

        [DataMember, Column(TypeName = "datetime2")]
        public DateTime TradeDateTime { get; set; }

        [DataMember, Column(TypeName = "datetime2")]
        public DateTime TradeAdded { get; set; }


        [DataMember]
        public virtual SmartComSymbol SmartComSymbol { get; set; }
    }
}
