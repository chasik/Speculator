using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SpeculatorModel.Transaq
{
    [DataContract, Table("TransaqSecurities")]
    public class Security
    {
        [DataMember, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public string SecCode { get; set; }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public int BoardId { get; set; }
        [DataMember]
        public int MarketId { get; set; }
        [DataMember]
        public byte Decimals { get; set; }
        [DataMember]
        public double MinStep { get; set; }
        [DataMember]
        public int LotSize { get; set; }
        [DataMember]
        public double PointCost { get; set; }

        //TODO opmask 

        //TODO sectype

        //TODO sec_tz

        //TODO quotestype


        [DataMember]
        public Board Board { get; set; }
        [DataMember]
        public Market Market { get; set; }
    }
}

//Список инструментов

//<securities>
// <security secid =”внутренний код :integer” active= "true/false :string" >
//  <seccode> Код инструмента:string </seccode>
//  <board>Идентификатор режима торгов по умолчанию:string</board>
//  <market>Идентификатор рынка :integer(:string в случае ошибки) </market>
//  <shortname>Наименование бумаги :string</shortname>
//  <decimals>Количество десятичных знаков в цене :integer</decimals>
//  <minstep>Шаг цены :double</minstep>
//  <lotsize>Размер лота :integer</lotsize>
//  <point_cost>Стоимость пункта цены :double</point_cost>

//  <opmask usecredit = "yes/no :string" bymarket="yes/no :string" nosplit="yes/no :string" immorcancel="yes/no :string" cancelbalance="yes/no :string"/>
//  <sectype>Тип бумаги :string</sectype>
//  <sec_tz> имя таймзоны инструмента(типа "Russian Standard Time", "USA=Eastern Standard Time"),содержит секцию CDATA :string</sec_tz>

//  <quotestype>
//       0 - без стакана
//       1 - стакан типа OrderBook
//       2 - стакан типа Level2
//  </quotestype>
//  </security>
//</securities>

