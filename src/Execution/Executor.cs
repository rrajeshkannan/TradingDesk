using System;
using System.Collections.Generic;
using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    public class Executor : IExecutor
    {
        private readonly IExecutionStrategy[] _executionStrategies;

        public Executor(IExecutionStrategy[] executionStrategies)
            => _executionStrategies = executionStrategies;

        public bool TryExecute(
            Trade trade, 
            IDictionary<(Type, Currency, Decimal), Trade> executableTrades)
        {
            foreach (var executionStrategy in _executionStrategies)
            {
                if (executionStrategy.CanHandle(trade))
                {
                    return executionStrategy.TryExecute(trade, executableTrades);
                }
            }

            return false;
            //return strategy?.TryExecute(trade, executableTrades) ?? false;
        }
    }
}