using System.Collections.Generic;
using TradingDesk.TradeEntity;

namespace TradingDesk.Engine
{
    public interface ILedger
    {
        bool AnyOpenTrades { get; }

        IEnumerable<Trade> OpenTrades { get; }

        void Book(Trade trade);

        void Reconcile();
    }
}