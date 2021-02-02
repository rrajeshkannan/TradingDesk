using System;

namespace TradingDesk.Engine
{
    public class SystemTimeKeeper : ITimeKeeper
    {
        public TimeSpan NowTime => DateTime.Now.TimeOfDay;
    }
}
