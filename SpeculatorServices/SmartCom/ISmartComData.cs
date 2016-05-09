using System.ServiceModel;

namespace SpeculatorServices.SmartCom
{
    [ServiceContract]
    public interface ISmartComData
    {
        [OperationContract]
        void ConnectToSmartCom();
    }
}
