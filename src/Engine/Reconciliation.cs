using System.Linq;
using TradingDesk.Execution;

namespace TradingDesk.Engine
{
    public class Reconciliation : IReconciliation
    {
        private readonly ILedger _ledger;

        private readonly IExecutor _executor;

        public Reconciliation(ILedger ledger, IExecutor executor)
        {
            _ledger = ledger;
            _executor = executor;
        }

        public void Execute()
            => Reconcile();

        private void Reconcile()
        {
            if (!_ledger.AnyOpenTrades)
            {
                // no open trades to reconcile
                return;
            }

            var executableTrades = _ledger.OpenTrades
                .OrderBy(trade => trade.Time);

            // trading "space" for: (1) efficiency (performance), (2) simplified logic
            var executableTradesMap = executableTrades
                .ToDictionary(trade => (trade.GetType(), trade.Currency, trade.Quantity));

            bool tradeStatusChange = false;

            foreach (var trade in executableTrades)
            {
                tradeStatusChange |= _executor.TryExecute(trade, executableTradesMap);
            }

            if (tradeStatusChange)
            {
                _ledger.Reconcile();
            }
        }
    }
}