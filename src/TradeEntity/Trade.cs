using System;
using System.Threading;

namespace TradingDesk.TradeEntity
{
    public abstract class Trade
    {
        private static volatile int _tradeId = 0;

        public int Id { get; }

        public TimeSpan Time { get; }

        public Status Status { get; set; }

        public Currency Currency { get; }

        public Decimal Quantity { get; }

        public Trade(TimeSpan time, Currency currency, Decimal quantity)
        {
            Id = Interlocked.Increment(ref _tradeId);
            Time = time;
            Status = Status.Unknown;

            Currency = currency;
            Quantity = quantity;
        }

        public void Fulfill()
        {
            Status = Status.Fulfilled;
        }

        public void Cancel()
        {
            Status = Status.Cancelled;
        }

        public override string ToString()
            => $"{Id}::Kind:<{GetType().Name}>,Time:<{Time}>,Status:<{Status}>,Currency:<{Currency}>,Quantity:{Quantity}";
    }
}