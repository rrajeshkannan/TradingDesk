using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingDesk.TradeEntity;

namespace TradingDesk.Engine
{
    public class Ledger : ILedger
    {
        private readonly List<Trade> _open = new List<Trade>();

        private readonly List<Trade> _fulfilled = new List<Trade>();

        private readonly List<Trade> _cancelled = new List<Trade>();

        public bool AnyOpenTrades => _open.Any();

        public IEnumerable<Trade> OpenTrades => _open.AsReadOnly();

        public void Book(Trade trade)
        {
            _open.Add(trade);

            trade.Status = Status.Open;
        }

        public void Reconcile()
        {
            var openTrades = new List<Trade>(_open);

            foreach (var trade in openTrades)
            {
                switch (trade.Status)
                {
                    case Status.Fulfilled:
                        _open.Remove(trade);
                        _fulfilled.Add(trade);
                        break;

                    case Status.Cancelled:
                        _open.Remove(trade);
                        _cancelled.Add(trade);
                        break;

                    case Status.Open:
                    case Status.Unknown:
                    default:
                        break;
                }
            }

            Console.WriteLine("New Ledger Status:");
            Console.WriteLine("Open Trades:");
            foreach (var trade in _open)
            {
                Console.WriteLine(trade.ToString());
            }

            Console.WriteLine("Fulfilled Trades:");
            foreach (var trade in _fulfilled)
            {
                Console.WriteLine(trade.ToString());
            }

            Console.WriteLine("Cancelled Trades:");
            foreach (var trade in _cancelled)
            {
                Console.WriteLine(trade.ToString());
            }
        }
    }
}