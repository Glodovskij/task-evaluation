using System;
using System.Threading;

namespace custom_threadpool
{
    public class JobQueueWorker
    {
        private Thread _executionThread;
        private Action _job;

        private AutoResetEvent _waitHandle;

        public event Action<JobQueueWorker> WorkerFreed;

        public JobQueueWorker()
        {
            _waitHandle = new AutoResetEvent(false);

            _executionThread = new Thread(Execute);
            _executionThread.IsBackground = true;
            _executionThread.Start();
        }

        public void SetWorkerJob(Action job)
        {
            _job = job;

            _waitHandle.Set();
        }

        private void Execute()
        {
            while (true)
            {
                _waitHandle.WaitOne();

                try
                {
                    _job.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Job interrupted incorrectly on thread {Thread.CurrentThread.ManagedThreadId}. {ex.Message}");
                }
                WorkerFreed.Invoke(this);
            }
        }
    }
}
