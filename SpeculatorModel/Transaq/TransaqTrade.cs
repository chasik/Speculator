using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SpeculatorModel.MainData;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqTrades")]
    public class TransaqTrade : Trade
    {
    }
}

// Сделки рынка по инструментам

//  <alltrades>
//    <trade secid =”внутренний код :integer”>
//        <seccode>Код инструмента:string</seccode>
//        <tradeno>биржевой номер сделки :integer64</tradeno>
//        <time>время сделки :date</time>
//        <board> Идентификатор режима торгов :string </board>
//        <price>цена сделки :double</price>
//        <quantity>объем сделки :integer</quantity>
//        <buysell>покупка (B) / продажа(S) :string</buysell>
//        <openinterest>... :integer</openinterest>
//        <period>Период торгов(O - открытие, N - торги, С - закрытие) :string </period>
//    </trade>
//  </alltrades>
