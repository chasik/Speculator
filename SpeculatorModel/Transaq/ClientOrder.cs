using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqClientOrders")]
    public class ClientOrder
    {
        [DataMember, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TransactionId { get; set; }
    }
}

// Заявки клиентов

//  <order transactionid = "идентификатор транзакции сервера Transaq :integer" >
//      <orderno> биржевой номер заявки :integer64</orderno>
//      <secid>идентификатор бумаги :integer</secid>
//      <board> Идентификатор режима торгов :string</board>
//      <seccode>Код инструмента:string</seccode>
//      <client>идентификатор клиента :string</client>
//      <union>Код юниона :string</union>
//      <status>статус заявки (см.ниже в таблице 3) :string</status>
//      <buysell>покупка(B) / продажа(S) :string</buysell>
//      <time>время регистрации заявки биржей :date</time>
//      <expdate>дата экспирации(только для ФОРТС) :date</expdate> 
//      <origin_orderno>первоначальный биржевой номер заявки ФОРТС,в которой задана дата экспирации :integer64</origin_orderno>
//      <accepttime>время регистрации заявки сервером Transaq(только для условных заявок) :date</accepttime>
//      <brokerref>примечание :string</brokerref>
//      <value>объем заявки в копейках :double</value>
//      <accruedint>НКД :double</accruedint>
//      <settlecode>Код поставки(значение биржи, определяющее правила расчетов - смотрите подробнее в документах биржи) :string </settlecode>
//      <balance>Неудовлетворенный остаток объема заявки в лотах(контрактах) :integer</balance>
//      <price>Цена :double</price>
//      <quantity>Количество :integer</quantity>
//      <hidden>Скрытое количество :integer</hidden>
//      <yield>Доходность :double</yield>
//      <withdrawtime>Время снятия заявки, 0 для активных :date</withdrawtime>
//      <condition>Условие, см.Newcondorder :string</condition>
//      <conditionvalue>Цена для условной заявки, либо обеспеченность в процентах :double</conditionvalue>
//      <validafter>с какого момента времени действительна(см.newcondorder) :date</validafter>
//      <validbefore>до какого момента действительно(см.newcondorder) :date</validbefore>
//      <maxcomission>максимальная комиссия по сделкам заявки :double </maxcomission>
//      <result>сообщение биржи в случае отказа выставить заявку :string </result>
//  </order>

