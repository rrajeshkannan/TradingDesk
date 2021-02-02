using System;
using System.Threading;
using TradingDesk.Engine;

namespace TradingDesk.TaskModel
{
    public class PeriodicTaskRunner : IPeriodicTaskRunner
    {
        private static volatile int _tradeId = 0;

        private int _id = 0;

        private ITimeKeeper _timeKeeper;

        public PeriodicTaskRunner(ITimeKeeper timeKeeper)
        {
            _id = Interlocked.Increment(ref _tradeId);
            _timeKeeper = timeKeeper;
        }

        private IPeriodicTask _task;

        private Timer _timer;

        private readonly object _state = new object();

        public void Initialize(IPeriodicTask task, TimeSpan period)
        {
            _task = task;

            _timer = new Timer(new TimerCallback(ExecuteTask), _state, TimeSpan.Zero, period);
        }

        private void ExecuteTask(object state)
        {
            Monitor.Enter(state);

            try
            {
                Console.WriteLine($"Timer-<{_id}>-{_timeKeeper.NowTime}");
                _task.Execute();
            }
            finally
            {
                Monitor.Exit(state);
            }
        }
        
        public void Dispose()
        {
            Monitor.Enter(_state);

            Console.WriteLine($"Timer-<{_id}>-Dispose");

            if (_timer == null)
            {
                return;
            }

            _timer.Dispose();
            _timer = null;

            Monitor.Exit(_state);
        }
    }
}