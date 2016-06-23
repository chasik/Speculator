using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MainData
{
    [DataContract]
    public class Trade
    {
        [DataMember, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TradeNo { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public int Volume { get; set; }

        [DataMember]
        public byte DiractionId { get; set; }

        [DataMember]
        public virtual Diraction Diraction { get; set; }
    }
}
