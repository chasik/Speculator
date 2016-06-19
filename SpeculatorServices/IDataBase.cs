using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using SpeculatorModel.MainData;

namespace SpeculatorServices
{
    [ServiceContract(CallbackContract = typeof(IDataCallBacks))]
    public interface IDataBase
    {
        [OperationContract]
        void ListenSymbol(Symbol symbol);
    }
}
