using System;

namespace TradingDesk.TradeEntity
{
    public class Swap : Trade
    {
        public Currency OtherCurrency { get; }

        public Decimal OtherQuantity { get; }

        public Swap(TimeSpan time, Currency currency, Decimal quantity, Currency otherCurrency, Decimal otherQuantity) 
            : base(time, currency, quantity)
        {
            OtherCurrency = otherCurrency;
            OtherQuantity = otherQuantity;
        }

        public override string ToString()
            => $"{base.ToString()},OtherCurrency:<{OtherCurrency}>,OtherQuantity:<{OtherQuantity}>";
    }
}