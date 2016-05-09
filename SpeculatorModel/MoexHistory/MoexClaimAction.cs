using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexClaimActions")]
    public class MoexClaimAction
    {
        [DataMember]
        public byte Id { get; set; }

        [DataMember, MaxLength(20)]
        public string Name { get; set; }

    }

    public enum ClaimActionEnum
    {
        Removed = 1,
        Added = 2,
        Trade = 3
    }
}
