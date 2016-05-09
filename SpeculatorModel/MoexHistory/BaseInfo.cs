using System;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract]
    public class BaseInfo
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime Moment { get; set; }

        [DataMember]
        public byte MoexSystemId { get; set; }

        [DataMember]
        public byte MoexSymbolId { get; set; }

        [DataMember]
        public virtual MoexSystem MoexSystem { get; set; }

        [DataMember]
        public virtual MoexSymbol MoexSymbol { get; set; }
    }
}
