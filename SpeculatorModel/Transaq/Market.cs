using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqMarkets")]
    public class Market
    {
        [DataMember,Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [DataMember, MaxLength(30)]
        public string Name { get; set; }
    }
}

//Доступные рынки

//<markets>
//  <market id = "внутренний код рынка :integer"> название рынка :string </market>
//</markets>