using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.SmartCom
{
    [DataContract, Table("SmartComBidAskValues")]
    public class SmartComBidAskValue
    {
        [DataMember, Key]
        public int Id { get; set; }
        [DataMember]
        public int SmartComSymbolId { get; set; }
        [DataMember]
        public byte? RowId { get; set; }
        [DataMember]
        public bool IsBid { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public int Volume { get; set; }
        [DataMember, Column(TypeName = "datetime2")]
        public DateTime Added { get; set; }

        public virtual SmartComSymbol SmartComSymbol { get; set; }
    }
}
