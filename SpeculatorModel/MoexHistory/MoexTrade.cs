using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexTrades")]
    public class MoexTrade : BaseInfo
    {
    }
}
