using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using SpeculatorModel.MainData;

namespace SpeculatorModel.MoexHistory
{
    [DataContract, Table("MoexSymbols")]
    public class MoexSymbol : Symbol
    {
    }
}
