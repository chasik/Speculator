using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MainData
{
    [DataContract, Table("TradeDiractions")]
    public class Diraction
    {
        [DataMember, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte Id { get; set; }
        
        [DataMember, MaxLength(20)]
        public string Name { get; set; }
    }

    public enum DiractionEnum
    {
        Buy = 1,
        Sell = 2
    }
}
