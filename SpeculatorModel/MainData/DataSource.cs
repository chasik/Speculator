using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MainData
{
    [DataContract, Table("DataSources")]
    public class DataSource
    {
        [DataMember, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte Id { get; set; }

        [DataMember, MaxLength(100, ErrorMessage = "Превышена длина наименования источника данных!")]
        public string Name { get; set; }
    }

    public enum DataSourceEnum
    {
        SmartCom = 1,
        Transaq = 2,
        Quik = 3,
        Plaza = 4,
        MoexHistory = 5
    }
}
