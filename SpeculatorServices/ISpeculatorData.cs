using System.ServiceModel;
using SpeculatorModel.MainData;

namespace SpeculatorServices
{
    [ServiceContract]
    public interface ISpeculatorData
    {
        [OperationContract]
        void GetHistory(string symbol);

        [OperationContract]
        DataSource[] DataSources();
    }
}
