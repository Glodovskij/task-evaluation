using System;
using System.Threading;

namespace custom_threadpool
{
    class Program
    {
        static void Main(string[] args)
        {
            Action action = () =>
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} started.");
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(10);
                }
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} finished.");
            };

            ThreadPool threadpool = new ThreadPool(10);

            threadpool.EnqueueJob(null);

            for(int i = 0; i < 1000; i++)
            {
                threadpool.EnqueueJob(action);
            }
            
            Console.ReadLine();
        }
    }
}
