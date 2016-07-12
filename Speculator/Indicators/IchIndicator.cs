using System;
using System.Linq;
using Infragistics.Controls.Charts;

namespace Speculator.Indicators
{
    public class IchIndicator : IndicatorGlassBase, IIndicator
    {
        public void CalcLastValue()
        {
            LastAddedGlassShear.CalcAverageForEachRow();

            if (LastAddedGlassShear.AverageValues == null)
                return;
            
            var sumPositive = LastAddedGlassShear.AverageValues.Skip((int)Parameters[0]).Take((int)Parameters[1]).Where(a => a > 0).Sum();
            var sumNegative = LastAddedGlassShear.AverageValues.Skip((int)Parameters[0]).Take((int)Parameters[1]).Where(a => a < 0).Sum();

            if (sumPositive + Math.Abs(sumNegative) == 0)
                return;

            LastAddedGlassShear.Value = 0;
            LastAddedGlassShear.Value2 = sumPositive + sumNegative;
        }

        public void BindToXamDataChart(XamDataChart chart)
        {
            throw new NotImplementedException();
        }
    }
}
