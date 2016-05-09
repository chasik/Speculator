using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexSymbols")]
    public class MoexSymbol
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember, MaxLength(50)]
        public string Name { get; set; }
    }
}
