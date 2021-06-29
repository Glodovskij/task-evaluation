using System;
using System.Collections.Generic;

namespace custom_threadpool
{
    public class ThreadPool
    {
        private List<JobQueueWorker> _jobQueueWorkers;

        private Queue<Action> _jobQueue;

        private Queue<JobQueueWorker> _pendingWorkers;

        private object _jobLocker = new object();
        private object _workerLocker = new object();

        public ThreadPool(int threadAmount)
        {
            _jobQueueWorkers = new List<JobQueueWorker>(threadAmount);
            _pendingWorkers = new Queue<JobQueueWorker>();

            for (int i = 0; i < threadAmount; i++)
            {
                JobQueueWorker queueWorker = new JobQueueWorker();
                queueWorker.WorkerFreed += QueueWorker_WorkerFreed;
                _jobQueueWorkers.Add(new JobQueueWorker());

                _pendingWorkers.Enqueue(queueWorker);
            }

            _jobQueue = new Queue<Action>();
        }

        private void QueueWorker_WorkerFreed(JobQueueWorker jobQueueWorker)
        {
            if (_jobQueue.Count > 0)
            {
                jobQueueWorker.SetWorkerJob(_jobQueue.Dequeue());
            }
            else
            {
                lock (_workerLocker)
                {
                    _pendingWorkers.Enqueue(jobQueueWorker);
                }
            }
        }

        public void EnqueueJob(Action job)
        {
            if (_pendingWorkers.Count > 0)
            {
                AssingJob(job);
            }
            else
            {
                lock (_jobLocker)
                {
                    _jobQueue.Enqueue(job);
                }
            }
            
        }

        public Action Dequeue()
        {
            lock(_jobLocker)
            {
                return _jobQueue.Dequeue();
            }
        }

        private void AssingJob(Action job)
        {
            lock (_workerLocker)
            {
                _pendingWorkers.Dequeue().SetWorkerJob(job);
            }
        }
    }
}
