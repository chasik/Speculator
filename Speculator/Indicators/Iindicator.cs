using Infragistics.Controls.Charts;

namespace Speculator.Indicators
{
    public interface IIndicator
    {
        void CalcLastValue();

        void BindToXamDataChart(XamDataChart chart);
    }
}
