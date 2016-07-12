using System.Runtime.Serialization;
using SmartCOM3Lib;

namespace SpeculatorServices.Trading
{
    [DataContract]
    public class TradingOrder
    {
        [DataMember]
        public string Symbol { get; set; }

        [DataMember]
        public StOrder_State State { get; set; }

        [DataMember]
        public StOrder_Action Action { get; set; }

        [DataMember]
        public StOrder_Type Type { get; set; }

        [DataMember]
        public StOrder_Validity Validity { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public double Stop { get; set; }

        [DataMember]
        public int Cookie { get; set; }
    }
}
