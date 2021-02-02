using System;

namespace TradingDesk.Configurations
{
    public interface ITimelineConfiguration
    {
        TimeSpan ReconciliationPeriod { get; }
        TimeSpan FulfillmentDeadline { get; }
    }
}