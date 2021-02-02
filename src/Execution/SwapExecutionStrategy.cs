using System;
using System.Collections.Generic;
using TradingDesk.TradeEntity;

namespace TradingDesk.Execution
{
    public class SwapExecutionStrategy : BaseExecutionStrategy
    {
        public SwapExecutionStrategy(ITradeCancellationValidator cancellationValidator) 
            : base(cancellationValidator) { }

        public override bool CanHandle(Trade trade)
        {
            return trade is Swap;
        }

        public override bool TryExecute(
            Trade trade,
            IDictionary<(Type, Currency, Decimal), Trade> executableTrades)
        {
            if (base.TryExecute(trade, executableTrades))
            {
                // Already executed -> no need for further execution
                return true;
            }

            if (trade.Status != Status.Open)
            {
                // trade is not open -> no need for further execution
                return true;
            }

            var swap = trade as Swap;

            if (swap == null)
            {
                // this strategy can't handle this "trade"
                return false;
            }

            var spot1Key = (typeof(Spot), swap.Currency, swap.Quantity);
            var spot2Key = (typeof(Spot), swap.OtherCurrency, swap.OtherQuantity);
            var forwardKey = (typeof(Forward), swap.Currency, swap.Quantity);

            if (executableTrades.TryGetValue(spot1Key, out var spot1) && 
                executableTrades.TryGetValue(spot2Key, out var spot2) && 
                executableTrades.TryGetValue(forwardKey, out var forward))
            {
                executableTrades.Remove(spot1Key);
                executableTrades.Remove(spot2Key);
                executableTrades.Remove(forwardKey);

                var swapKey = (typeof(Swap), swap.Currency, swap.Quantity);
                executableTrades.Remove(swapKey);

                spot1.Fulfill();
                spot2.Fulfill();
                forward.Fulfill();
                swap.Fulfill();

                return true;
            }
            
            // no execution happened here
            return false;
        }
    }
}