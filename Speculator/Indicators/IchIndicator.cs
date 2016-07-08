using System;
using System.Linq;

namespace Speculator.Indicators
{
    public class IchIndicator : IndicatorGlassBase, IIndicator
    {
        public void CalcLastValue()
        {
            //LastAddedGlassShear.Value = LastAddedGlassShear.GetAskAverage(30);
            //LastAddedGlassShear.Value2 = -LastAddedGlassShear.GetBidAverage(30);

            LastAddedGlassShear.CalcAverageForEachRow();

            if (LastAddedGlassShear.AverageValues == null)
                return;

            
            var sumPositive = LastAddedGlassShear.AverageValues.Skip((int)Parameters[0]).Take((int)Parameters[1]).Where(a => a > 0).Sum();
            var sumNegative = LastAddedGlassShear.AverageValues.Skip((int)Parameters[0]).Take((int)Parameters[1]).Where(a => a < 0).Sum();

            if (sumPositive + Math.Abs(sumNegative) == 0)
                return;


            //LastAddedGlassShear.Value = Math.Max(sumPositive, Math.Abs(sumNegative)) * 100
            //    / (sumPositive + Math.Abs(sumNegative)) * (sumPositive > Math.Abs(sumNegative) ? 1 : -1);

            LastAddedGlassShear.Value = 0;

            LastAddedGlassShear.Value2 = sumPositive + sumNegative;

            //resultOneTick.PresetHeight = 100
            //    * Math.Max(sumpositive, Math.Abs(sumnegative))
            //    / (sumpositive + Math.Abs(sumnegative)) * (sumpositive > Math.Abs(sumnegative) ? 1 : -1);

            //resultOneTick.SumPresetHeight = sumpositive + sumnegative;
        }
    }
}
