using System;

namespace TradingDesk.TradeEntity
{
    public class Forward : Trade
    {
        public DateTime ForwardDate { get; }

        public Forward(TimeSpan time, Currency currency, Decimal quantity, DateTime forwardDate) : base(time, currency, quantity)
        {
            ForwardDate = forwardDate;
        }
    }
}