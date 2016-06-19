using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SpeculatorModel.MainData;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexClaims")]
    public class MoexClaim : BaseInfo
    {
        [DataMember]
        public long? MoexTradeId { get; set; }

        [DataMember]
        public decimal? PriceDeal { get; set; }

        [DataMember]
        public byte ClaimActionId { get; set; }

        [DataMember]
        public virtual MoexTrade MoexTrade { get; set; }

        [DataMember]
        public virtual ClaimAction ClaimAction { get; set; }
    }
}
