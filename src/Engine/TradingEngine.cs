using System;
using TradingDesk.Configurations;
using TradingDesk.TaskModel;
using TradingDesk.TradeEntity;

namespace TradingDesk.Engine
{
    public class TradingEngine : IDisposable
    {
        private readonly ILedger _ledger;

        private readonly ITimeKeeper _timeKeeper;

        private readonly IPeriodicTaskRunner _periodicTaskRunner;
        
        public TradingEngine (
            ILedger ledger,
            IReconciliation reconciliation,
            IPeriodicTaskRunner periodicTaskRunner,
            ITimeKeeper timeKeeper,
            ITimelineConfiguration timelineConfiguration)
        {
            _ledger = ledger;

            _periodicTaskRunner = periodicTaskRunner;
            _periodicTaskRunner.Initialize(reconciliation, timelineConfiguration.ReconciliationPeriod);

            _timeKeeper = timeKeeper;
        }

        public void BookSpot(Currency currency, Decimal quantity) 
            => BookTrade(new Spot(_timeKeeper.NowTime, currency, quantity));

        public void BookForward(Currency currency, Decimal quantity, DateTime forwardDate)
            => BookTrade(new Forward(_timeKeeper.NowTime, currency, quantity, forwardDate));

        public void BookSwap(Currency currency, Decimal quantity, Currency other, Decimal otherQuantity)
            => BookTrade(new Swap(_timeKeeper.NowTime, currency, quantity, other, otherQuantity));

        private void BookTrade(Trade trade)
            => _ledger.Book(trade);

        public void Dispose() 
            => _periodicTaskRunner.Dispose();
    }
}