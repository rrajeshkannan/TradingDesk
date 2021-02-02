using System;

namespace TradingDesk.Configurations
{
    public class TimelineConfiguration : ITimelineConfiguration
    {
        public TimeSpan ReconciliationPeriod { get; }
        public TimeSpan FulfillmentDeadline { get; }

        public TimelineConfiguration(TimeSpan reconciliationPeriod, TimeSpan fulfillmentDeadline)
        {
            ReconciliationPeriod = reconciliationPeriod;
            FulfillmentDeadline = fulfillmentDeadline;
        }
    }
}