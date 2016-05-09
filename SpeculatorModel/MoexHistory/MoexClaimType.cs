using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexClaimTypes")]
    public class MoexClaimType
    {
        [DataMember]
        public byte Id { get; set; }

        [DataMember, MaxLength(20)]
        public string Name { get; set; }
    }

    public enum ClaimTypeEnum
    {
        Buy = 1,
        Sell = 2
    }
}
