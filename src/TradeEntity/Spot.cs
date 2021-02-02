using System;

namespace TradingDesk.TradeEntity
{
    public class Spot : Trade
    {
        public Spot(TimeSpan time, Currency currency, Decimal quantity) : base(time, currency, quantity) { }
    }
}