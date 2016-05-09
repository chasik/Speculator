using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexTradeDiractions")]
    public class MoexTradeDiraction
    {
        [DataMember]
        public byte Id { get; set; }
        
        [DataMember, MaxLength(20)]
        public string Name { get; set; }
    }

    public enum MoexTradeDiractionEnum
    {
        Buy = 1,
        Sell = 2
    }
}
