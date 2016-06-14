using System.ServiceModel;
using SpeculatorModel.MainData;

namespace SpeculatorServices
{
    [ServiceContract(CallbackContract = typeof(IDataCallBacks))]
    public interface IDataBase
    {

    }
}
