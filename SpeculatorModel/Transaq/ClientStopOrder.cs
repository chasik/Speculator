using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqClientStopOrders")]
    public class ClientStopOrder
    {
        [DataMember, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TransactionId { get; set; }
    }
}

// Cтатусы стоп-заявок и обычных заявок различаются - смотрите их список в конце документации.
// Любое из полей может отсутствовать, если оно неактуально или не было задано при выставлении заявки.

//  <stoporder transactionid = "идентификатор стопа" >
//      <activeorderno>:integer64</activeorderno>
//      <secid>:integer</secid>
//      <board>:string</board>
//      <seccode>Код инструмента:string</seccode>
//      <client>:string</client>
//      <union>Код юниона :string</union>
//      <buysell>:string</buysell>
//      <canceller>:string</canceller>
//      <alltradeno>:integer64</alltradeno>
//      <validbefore>:data</validbefore>
//      <author>:string</author>
//      <accepttime>:data</accepttime>
//      <linkedorderno>:integer64</linkedorderno>
//      <expdate>:data</expdate>
//      <status>:string</status>
//      <stoploss usecredit = "yes/no :string" >
//          <activationprice>:double</activationprice>
//          <guardtime>:data</guardtime>
//          <brokerref>:string</brokerref>
//          <quantity>:integer(:double в случае %)</quantity>
//          <bymarket/>
//          <orderprice>:double </orderprice>
//      </stoploss>
//      <takeprofit>
//          <activationprice>:double</activationprice>
//          <guardtime>:data</guardtime>
//          <brokerref>:string</brokerref>
//          <quantity>:integer(:double в случае %)</quantity>
//          <extremum>:double</extremum>
//          <level>:double</level>
//          <correction>:double</correction>
//          <guardspread>:double</guardspread>
//      </takeprofit>
//  </stoporder>