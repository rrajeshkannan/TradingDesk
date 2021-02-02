using System;
using System.Collections.Generic;
using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    public class BaseExecutionStrategy : IExecutionStrategy
    {
        private readonly ITradeCancellationValidator _cancellationValidator;

        public BaseExecutionStrategy(ITradeCancellationValidator cancellationValidator)
            => _cancellationValidator = cancellationValidator;

        public virtual bool CanHandle(Trade trade)
        {
            return (trade is Spot) || 
                (trade is Forward);
        }

        public virtual bool TryExecute(
            Trade trade,
            IDictionary<(Type, Currency, Decimal), Trade> executableTrades)
        {
            if (trade.Status == Status.Open)
            {
                if (_cancellationValidator.CanCancel(trade))
                {
                    trade.Cancel();

                    var key = (trade.GetType(), trade.Currency, trade.Quantity);
                    executableTrades.Remove(key);

                    // trade just got cancelled -> no chance for further execution
                    return true;
                }

                // not cancelled -> there is chance for further execution
                return false;
            }

            // Status.Fulfilled - trade already fulfilled -> no chance for further execution
            // Status.Cancelled - trade already cancelled -> no chance for further execution
            // Status.Unknown - invalid trade -> no chance for further execution
            return true;
        }
    }
}