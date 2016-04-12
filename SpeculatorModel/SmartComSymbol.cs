using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel
{
    [DataContract, Table("SmartComSymbols")]
    public class SmartComSymbol
    {
        //string symbol, string shortName, string longName, 
        //string type, int decimals, int lotSize, double punkt, double step, string secExtId, string secExchName, DateTime expiryDate, double daysBeforeExpiry, double strike
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string LongName { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Decimals { get; set; }

        [DataMember]
        public int LotSize { get; set; }

        [DataMember]
        public double? Punkt { get; set; }

        [DataMember]
        public double? Step { get; set; }

        [DataMember]
        public string SecExtId { get; set; }

        [DataMember]
        public string SecExchName { get; set; }

        [DataMember]
        public DateTime ExpiryDate { get; set; }

        [DataMember]
        public double? Strike { get; set; }
    }
}
