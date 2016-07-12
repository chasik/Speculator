using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeculatorModel.MainData
{
    public enum TradingState
    {
        Free = 1,
        TryDoMainOrder = 2,
        MainOrderSucceeded = 3,
        MainOrderFailed = 4
    }
}
