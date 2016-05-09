using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqQuotes")]
    public class Quote
    {
        public int Id { get; set; }
    }
}

// Глубина рынка по инструментам(“стакан”)

//  <quotes>
//      <quote secid =”внутренний код :integer”>
//          <board> Идентификатор режима торгов :string <board>
//          <seccode>Код инструмента:string</seccode>
//          <price>цена :double</price>
//          <source> Источник котировки(маркетмейкер):string </source>
//          <yield>доходность(актуально только для облигаций) :integer</yield>
//          <buy>количество бумаг к покупке :integer</buy>
//          <sell>количество бумаг к продаже :integer</sell>
//      </quote>
//  </quotes>
