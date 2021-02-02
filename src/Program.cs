using System;
using System.Threading;
using TradingDesk.Configurations;
using TradingDesk.Engine;
using TradingDesk.Execution;
using TradingDesk.TaskModel;
using TradingDesk.TradeEntity;

namespace TradingDesk
{
    public class Program : IDisposable
    {
        private readonly ILedger _ledger;
        private readonly TradingEngine _engine;

        public Program(TimeSpan fulfillmentDeadline)
        {
            var timelineConfiguration = new TimelineConfiguration(new TimeSpan(0, 0, 1), fulfillmentDeadline);
            var timeKeeper = new SystemTimeKeeper();

            var tradeCancellationValidator = new TradeCancellationValidator(timeKeeper, timelineConfiguration);
            var executionStrategies = new IExecutionStrategy[]
            {
                new BaseExecutionStrategy(tradeCancellationValidator),
                new SwapExecutionStrategy(tradeCancellationValidator)
            };

            _ledger = new Ledger();
            _engine = new TradingEngine(
                _ledger,
                new Reconciliation(_ledger, new Executor(executionStrategies)),
                new PeriodicTaskRunner(timeKeeper),
                timeKeeper,
                timelineConfiguration);
        }

        public void Dispose()
        {
            _engine.Dispose();
        }

        static void Main()
        {
            SpotsAndForwardThenSwapWithinDeadline();
            SpotsAndForwardThenSwapOutsideDeadline();
            SwapThenSpotsAndForwardWithinDeadline();
            SwapThenSpotsAndForwardOutsideDeadline();
            SpotsAndForwardThenSwapWithinDeadlineButDifferentQuantities();
            SpotsAndForwardThenSwapWithinDeadlineButDifferentCurrencies();

            Console.ReadKey();
        }

        static void SpotsAndForwardThenSwapWithinDeadline()
        {
            var fulfillmentDeadline = new TimeSpan(0, 5, 0);

            using (var program = new Program(fulfillmentDeadline))
            {
                Console.WriteLine(nameof(SpotsAndForwardThenSwapWithinDeadline));

                program._engine.BookSpot(Currency.EUR, 100);
                program._engine.BookSpot(Currency.USD, 120);
                program._engine.BookForward(Currency.EUR, 100, DateTime.Now.AddDays(7));

                Thread.Sleep(new TimeSpan(0, 0, 2));
                program._engine.BookSwap(Currency.EUR, 100, Currency.USD, 120);

                Thread.Sleep(new TimeSpan(0, 0, 2));
            }

            Console.WriteLine();
        }

        static void SpotsAndForwardThenSwapOutsideDeadline()
        {
            var fulfillmentDeadline = new TimeSpan(0, 0, 3);

            using (var program = new Program(fulfillmentDeadline))
            {
                Console.WriteLine(nameof(SpotsAndForwardThenSwapOutsideDeadline));

                program._engine.BookSpot(Currency.EUR, 100);
                program._engine.BookSpot(Currency.USD, 120);
                program._engine.BookForward(Currency.EUR, 100, DateTime.Now.AddDays(7));

                Thread.Sleep(fulfillmentDeadline);
                program._engine.BookSwap(Currency.EUR, 100, Currency.USD, 120);

                Thread.Sleep(fulfillmentDeadline);
                Thread.Sleep(fulfillmentDeadline);
            }

            Console.WriteLine();
        }

        static void SwapThenSpotsAndForwardWithinDeadline()
        {
            var fulfillmentDeadline = new TimeSpan(0, 5, 0);

            using (var program = new Program(fulfillmentDeadline))
            {
                Console.WriteLine(nameof(SwapThenSpotsAndForwardWithinDeadline));

                program._engine.BookSwap(Currency.EUR, 100, Currency.USD, 120);

                Thread.Sleep(new TimeSpan(0, 0, 2));
                program._engine.BookSpot(Currency.EUR, 100);
                program._engine.BookSpot(Currency.USD, 120);
                program._engine.BookForward(Currency.EUR, 100, DateTime.Now.AddDays(7));

                Thread.Sleep(new TimeSpan(0, 0, 2));
            }

            Console.WriteLine();
        }

        static void SwapThenSpotsAndForwardOutsideDeadline()
        {
            var fulfillmentDeadline = new TimeSpan(0, 0, 3);

            using (var program = new Program(fulfillmentDeadline))
            {
                Console.WriteLine(nameof(SwapThenSpotsAndForwardOutsideDeadline));
                program._engine.BookSwap(Currency.EUR, 100, Currency.USD, 120);

                Thread.Sleep(fulfillmentDeadline);
                program._engine.BookSpot(Currency.EUR, 100);
                program._engine.BookSpot(Currency.USD, 120);
                program._engine.BookForward(Currency.EUR, 100, DateTime.Now.AddDays(7));

                Thread.Sleep(fulfillmentDeadline);
                Thread.Sleep(fulfillmentDeadline);
            }

            Console.WriteLine();
        }

        static void SpotsAndForwardThenSwapWithinDeadlineButDifferentQuantities()
        {
            var fulfillmentDeadline = new TimeSpan(0, 0, 3);

            using (var program = new Program(fulfillmentDeadline))
            {
                Console.WriteLine(nameof(SpotsAndForwardThenSwapWithinDeadlineButDifferentQuantities));

                program._engine.BookSpot(Currency.EUR, 200);
                program._engine.BookSpot(Currency.USD, 240);
                program._engine.BookForward(Currency.EUR, 200, DateTime.Now.AddDays(7));

                Thread.Sleep(new TimeSpan(0, 0, 2));
                program._engine.BookSwap(Currency.EUR, 100, Currency.USD, 120);

                Thread.Sleep(fulfillmentDeadline);
                Thread.Sleep(fulfillmentDeadline);
            }

            Console.WriteLine();
        }

        static void SpotsAndForwardThenSwapWithinDeadlineButDifferentCurrencies()
        {
            var fulfillmentDeadline = new TimeSpan(0, 0, 3);

            using (var program = new Program(fulfillmentDeadline))
            {
                Console.WriteLine(nameof(SpotsAndForwardThenSwapWithinDeadlineButDifferentCurrencies));

                program._engine.BookSpot(Currency.EUR, 100);
                program._engine.BookSpot(Currency.USD, 120);
                program._engine.BookForward(Currency.EUR, 100, DateTime.Now.AddDays(7));

                Thread.Sleep(new TimeSpan(0, 0, 2));
                program._engine.BookSwap(Currency.EUR, 100, Currency.GBP, 120);

                Thread.Sleep(fulfillmentDeadline);
                Thread.Sleep(fulfillmentDeadline);
            }

            Console.WriteLine();
        }
    }
}