using System.ServiceModel;

namespace SpeculatorServices.Transaq
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
