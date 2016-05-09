using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexClaims")]
    public class MoexClaim : BaseInfo
    {

    }
}
