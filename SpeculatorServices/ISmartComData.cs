using System.ServiceModel;

namespace SpeculatorServices
{
    [ServiceContract]
    public interface ISmartComData
    {
        [OperationContract]
        void ConnectToSmartCom();
    }
}
