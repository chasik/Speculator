using System.Runtime.Serialization;

namespace SpeculatorModel.MainData
{
    [DataContract]
    public class Portfolio
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
