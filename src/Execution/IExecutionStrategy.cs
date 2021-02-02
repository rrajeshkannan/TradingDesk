using System;
using System.Collections.Generic;
using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    public interface IExecutionStrategy
    {
        bool CanHandle(Trade trade);

        bool TryExecute(Trade trade, IDictionary<(Type, Currency, Decimal), Trade> executableTrades);
    }
}