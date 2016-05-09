using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqClientTrades")]
    public class ClientTrade
    {
        [DataMember]
        public int Id { get; set; }
    }
}

// Сделки клиентов

//<trades>
//  <trade>
//      <secid>Идентификатор бумаги :integer</secid>
//      <tradeno>Номер сделки на бирже :integer64</tradeno>
//      <orderno>Номер заявки на бирже :integer64</orderno>
//      <board>Идентификатор режима торгов :string</board>
//      <seccode>Код инструмента:string</seccode>
//      <client>Идентификатор клиента :string</client>
//      <union>Код юниона :string</union>
//      <buysell>B - покупка, S – продажа :string</buysell>
//      <time>время сделки :date</time>
//      <brokerref>примечание :string</brokerref>
//      <value>объем сделки :double</value>
//      <comission>комиссия :double</comission>
//      <price>цена :double</price>
//      <items>кол-во инструмента в сделках в штуках:integer64</items>
//      <quantity>количество лотов :integer</quantity>
//      <yield>доходность :double</yield>
//      <accruedint>НКД :double</accruedint>
//      <tradetype>тип сделки: ‘T’ – обычная   ‘N’ – РПС   ‘R’ – РЕПО   ‘P’ – размещение :string</tradetype>
//      <settlecode>код поставки :string</settlecode>
//      <currentpos>Текущая позиция :integer64</currentpos>
//  </trade>
//</trades>

