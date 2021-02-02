using System;
using TradingDesk.Configurations;
using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    public static class ExecutionStrategyExtensions
    {
        public static bool DeadlineCrossed(this Trade trade, TimeSpan nowTime, TimeSpan fulfillmentDeadline) 
            => nowTime > trade.Time.Add(fulfillmentDeadline);
    }
}