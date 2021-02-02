using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    public interface ITradeCancellationValidator
    {
        bool CanCancel(Trade trade);
    }
}