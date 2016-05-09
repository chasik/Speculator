using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqBoards")]
    public class Board
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember, MaxLength(200)]
        public string InnerId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int? MarketId { get; set; }

        [DataMember]
        public byte TypeId { get; set; }

        [DataMember]
        public virtual Market Market { get; set; }
    }
}

//Справочник режимов торгов

//<boards>
// <board id = "Идентификатор режима торгов :string" >
//   <name> Наименование режима торгов :string </name>
//   <market>Внутренний код рынка :integer</market>
//   <type> тип режима торгов 0=FORTS, 1=Т+, 2= Т0: integer</type>
// </board>
//</boards>