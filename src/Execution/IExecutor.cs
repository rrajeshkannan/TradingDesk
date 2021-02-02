using System;
using System.Collections.Generic;
using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    public interface IExecutor
    {
        bool TryExecute(Trade trade, IDictionary<(Type, Currency, Decimal), Trade> executableTrades);
    }
}