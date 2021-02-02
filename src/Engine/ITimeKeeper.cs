using System;

namespace TradingDesk.Engine
{
    public interface ITimeKeeper
    {
        TimeSpan NowTime { get; }
    }
}