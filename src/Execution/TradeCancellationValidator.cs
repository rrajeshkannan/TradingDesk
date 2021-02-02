using TradingDesk.Configurations;
using TradingDesk.Engine;
using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    internal class TradeCancellationValidator : ITradeCancellationValidator
    {
        private readonly ITimeKeeper _timeKeeper;

        private readonly ITimelineConfiguration _timeline;

        public TradeCancellationValidator(ITimeKeeper timeKeeper, ITimelineConfiguration timeline)
        {
            _timeline = timeline;
            _timeKeeper = timeKeeper;
        }

        public bool CanCancel(Trade trade)
        {
            // if outside deadline -> cancel the trade
            return trade.DeadlineCrossed(_timeKeeper.NowTime, _timeline.FulfillmentDeadline);
        }
    }
}