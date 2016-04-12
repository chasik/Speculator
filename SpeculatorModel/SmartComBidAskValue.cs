using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel
{
    [DataContract, Table("SmartComBidAskValues")]
    public class SmartComBidAskValue
    {
        public SmartComBidAskValue()
        {
            
        }

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int SmartComSymbolId { get; set; }
        [DataMember, Column(TypeName = "datetime2")]
        public DateTime Added { get; set; }
        [DataMember]
        public byte RowId { get; set; }
        [DataMember]
        public bool IsBid { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public int Volume { get; set; }

        public virtual SmartComSymbol SmartComSymbol { get; set; }
    }
}
