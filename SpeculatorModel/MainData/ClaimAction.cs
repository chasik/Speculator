using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MainData
{
    [DataContract, Table("ClaimActions")]
    public class ClaimAction
    {
        [DataMember, DatabaseGenerated(DatabaseGeneratedOption.None)]
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
