using System.ServiceModel;

namespace SpeculatorServices
{
    [ServiceContract]
    public interface ITransaqData
    {
        [OperationContract]
        void ConnectToTransaq();
        [OperationContract]
        void DisconnectFromTransaq();
    }
}
