using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SpeculatorModel.MainData;

namespace SpeculatorModel.MoexHistory
{
    [DataContract]
    public class BaseInfo
    {
        [DataMember, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [DataMember]
        public DateTime Moment { get; set; }

        [DataMember]
        public int Volume { get; set; }

        [DataMember]
        public byte MoexSystemId { get; set; }

        [DataMember]
        public int MoexSymbolId { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public byte DiractionId { get; set; }

        [DataMember]
        public virtual MoexSystem MoexSystem { get; set; }

        [DataMember]
        public virtual MoexSymbol MoexSymbol { get; set; }

        [DataMember]
        public virtual Diraction Diraction { get; set; }
    }
}
