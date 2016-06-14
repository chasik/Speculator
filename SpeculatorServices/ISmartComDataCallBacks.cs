using System.ServiceModel;
using SpeculatorModel.MainData;

namespace SpeculatorServices
{
    [ServiceContract]
    public interface IDataCallBacks
    {
        [OperationContract(IsOneWay = true)]
        void Symbols(Symbol[] symbols);
    }
}
