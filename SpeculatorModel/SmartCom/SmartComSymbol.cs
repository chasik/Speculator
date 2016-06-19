using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SpeculatorModel.MainData;

namespace SpeculatorModel.SmartCom
{
    [DataContract, Table("SmartComSymbols")]
    public class SmartComSymbol : Symbol
    {
        //string symbol, string shortName, string longName, 
        //string type, int decimals, int lotSize, double punkt, double step, string secExtId, string secExchName, DateTime expiryDate, double daysBeforeExpiry, double strike

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Decimals { get; set; }

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
