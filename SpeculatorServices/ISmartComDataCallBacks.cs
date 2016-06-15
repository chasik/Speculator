using System.ServiceModel;
using SpeculatorModel.MainData;

namespace SpeculatorServices
{
    [ServiceContract]
    public interface IDataCallBacks
    {
        [OperationContract(IsOneWay = true)]
        void UpdateAskEvent();

        [OperationContract(IsOneWay = true)]
        void UpdateBidEvent();
    }
}
