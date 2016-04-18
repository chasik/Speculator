using System.ServiceModel;

namespace SpeculatorServices
{
    [ServiceContract]
    public interface ISpeculatorData
    {
        [OperationContract]
        void GetHistory(string symbol);
    }
}
