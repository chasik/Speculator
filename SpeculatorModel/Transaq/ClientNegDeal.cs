using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqClientNegDeals")]
    public class ClientNegDeal
    {
        [DataMember, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TransactionId { get; set; }
    }
}

// содержит информацию об адресных заявках

//  <negdeal transactionid = "идентификатор транзакции сервера Transaq :integer">
//      <secid>идентификатор бумаги :integer</secid>
//      <inout> 1 - отправленная, 2 – полученная, 3 – внутренняя :integer</inout>
//      <orderno> биржевой номер заявки :integer64</orderno>
//      <buysell> направление заявки: ’B’-покупка ’S’- продажа) :string</buysell>
//      <board> идентификатор режима торгов :string </board>
//      <seccode>Код инструмента:string</seccode>
//      <price2> цена выкупа РЕПО :double </price2>
//      <repovalue> сумма РЕПО :double </repovalue>
//      <repo2value> стоимость выкупа РЕПО :double </repo2value>
//      <matchref> ссылка:string </matchref>
//      <client>код клиента в СБО TRANSAQ :string </client>
//      <union>Код юниона :string</union>
//      <price>цена заявки :double </price>
//      <items>кол-во инструмента в сделке(шт.) :integer64</items>
//      <volume>объем сделки без учета НКД(руб) :double </volume>
//      <accruedint>НКД на единицу инструмента(руб) :double</accruedint>
//      <maxcomission>максимальная комиссия по сделкам заявки(руб) :double </maxcomission>
//      <time>дата и время регистрации заявки на Бирже :data</time>
//      <author>идентификатор трейдера :string </author>
//      <brokerref>примечание трейдера, подавшего заявку :string</brokerref>
//      <settlecode>код поставки :string </settlecode>
//      <cpfirmid>Идентификатор фирмы-контрагента :string </cpfirmid>
//      <repoterm>срок РЕПО, дней :integer</repoterm>
//      <reporate>ставка РЕПО :double </reporate>
//      <startdiscount>начальный дисконт :double </startdiscount>
//      <lowerdiscount>нижний предел дисконта :double </lowerdiscount>
//      <upperdiscount>верхний предел дисконта :double </upperdiscount>
//      <activationtime>дата и время активации заявки :data</activationtime>
//      <blocksecurities>признак блокировки бумаг на время РЕПО ‘Y’/’N’ :string </blocksecurities>
//      <withdrawtime>дата и время снятия заявки :data</withdrawtime>
//      <ordertype> тип адресной заявки:
//          •	RPS – РПС
//          •	EXTCREPO
//          •	EXTREPO - модифицированное РЕПО
//          •	REPO - классическое РЕПО
//      </ordertype>
//      <status> статус заявки (см.ниже в таблице 3) :string</status>
//  </negdeal>