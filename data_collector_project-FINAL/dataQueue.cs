using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    class Queue
    {
        public class dataQueue<T>
        {
            public ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
            private object lockObject = new object();

            public int Limit { get; set; }
            public void Enqueue(T obj)
            {
                queue.Enqueue(obj);
                lock (lockObject)
                {
                    T overflow;
                    while (queue.Count > Limit && queue.TryDequeue(out overflow)) ;
                }
            }
        }       
    }
}
// Data Collector Project - Roldan #2057584 - © Fall 2020

