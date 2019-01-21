using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoDicomTest
{

    class StoreScuQueue
    {
        private ConcurrentDictionary<int, TransmissionInfo> _actionCollection = new ConcurrentDictionary<int, TransmissionInfo>();
        private AutoResetEvent _event = new AutoResetEvent(true);

        public event EventHandler<TransmissionInfo> TransmissionCompleted;
        private Func<TransmissionInfo, Task> _transmission;

        public StoreScuQueue(Func<TransmissionInfo, Task> tranmission)
        {
            _transmission = tranmission;
        }

        public void WaitCanAdd()
        {
            if (_actionCollection.Count >= 4)
            {
                _event.WaitOne();
            }
        }

        public void Add(TransmissionInfo taskA)
        {
            _actionCollection.AddOrUpdate(taskA.TaskId, taskA, (key, oldValue) => taskA);
            var task = Action(taskA);
            if (_actionCollection.Count >= 4)
            {
                _event.Reset();
            }
        }

        public async Task Action(TransmissionInfo info)
        {
            Console.WriteLine($"-- Start transmission {info.TaskId}");
            await _transmission(info);
            OneTaskCompleted(info.TaskId);
        }

        public void OneTaskCompleted(int taskId)
        {
            _actionCollection.TryRemove(taskId, out TransmissionInfo info);
            _event.Set();
            TransmissionCompleted?.Invoke(this, info);
        }
    }

}
