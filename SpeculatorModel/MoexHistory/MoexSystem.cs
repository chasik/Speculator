using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexSystems")]
    public class MoexSystem
    {
        [DataMember]
        public byte Id { get; set; }
        [DataMember, MaxLength(50)]
        public string Name { get; set; }
    }

    public enum MoexSystemEnum
    {
        Future = 1,
        Call = 2,
        Put = 3
    }
}
