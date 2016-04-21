using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqCandleKinds")]
    public class CandleKind
    {
        [DataMember, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [DataMember]
        public int Period { get; set; }
        [DataMember, Column(TypeName = "nvarchar(20)")]
        public string PeriodName { get; set; }
    }
}

//Информация о доступных периодах свечей

//<candlekinds>
//  <kind>
//      <id>идентификатор периода :integer</id>
//      <period>количество секунд в периоде :integer</period>
//      <name>наименование периода :string</name>
//  </kind>
//</candlekinds>
