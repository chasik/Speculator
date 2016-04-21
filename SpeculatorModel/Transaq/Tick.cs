using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqTicks")]
    public class Tick
    {
    }
}

//Тиковые данные

//  <ticks>
//      <tick>
//          <secid>идентификатор бумаги :integer</secid>
//          <tradeno>номер сделки :integer64</tradeno>
//          <tradetime>время сделки :date</tradetime>
//          <price>цена :double</price>
//          <quantity>количество лотов (контрактов) :integer</quantity>
//          <period>торовый период(O - открытие, N – основные торги, C - закрытие; передается только для ММВБ) :string</period>
//          <buysell>B - покупка, S - продажа(с точки зрения того, кто инициировал сделку, приняв условия выставленной ранее заявки.Передается только когда есть такая информация) :string </buysell>
//          <openinterest>кол-во открытых позиций на срочном рынке :integer</openinterest>
//          <board>Идентификатор режима торгов :string</board>
//          <seccode>Код инструмента:string</seccode>
//      </tick>
//  </ticks>
