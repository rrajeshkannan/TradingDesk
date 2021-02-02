using System;

namespace TradingDesk.TaskModel
{
    public interface IPeriodicTaskRunner : IDisposable
    {
        void Initialize(IPeriodicTask task, TimeSpan period);
    }
}