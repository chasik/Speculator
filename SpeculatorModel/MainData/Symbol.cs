using System.Runtime.Serialization;

namespace SpeculatorModel.MainData
{
    [DataContract]
    public class Symbol
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string LongName { get; set; }

        [DataMember]
        public double? Step { get; set; }

        [DataMember]
        public int? LotSize { get; set; }

        [DataMember]
        public double? Punkt { get; set; }
    }
}
