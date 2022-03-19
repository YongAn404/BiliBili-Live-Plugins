using Live;
using System;
using System.Collections.Generic;

namespace Live
{
    public class TimerSvc
    {

        class TaskPack
        {
            public int tid;
            public Action<int> cb;
            public TaskPack(int tid, Action<int> cb)
            {
                this.tid = tid;
                this.cb = cb;
            }
        }

        private static TimerSvc instance = null;
        public static TimerSvc Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimerSvc();
                }
                return instance;
            }
        }
        PETimer pt = null;
        Queue<TaskPack> tpQue = new Queue<TaskPack>();
        private static readonly string tpQueLock = "tpQueLock";

        public void Init()
        {
            pt = new PETimer(100);
            tpQue.Clear();

            //pt.SetLog((string info) =>
            //{
            //    Form1.Log(info);
            //});

            pt.SetHandle((Action<int> cb, int tid) =>
            {
                if (cb != null)
                {
                    lock (tpQueLock)
                    {
                        tpQue.Enqueue(new TaskPack(tid, cb));
                    }
                }
            });

            CMDWnd.Log("PETimer已启用");

        }

        public void Update()
        {
            while (tpQue.Count > 0)
            {
                TaskPack tp = null;
                lock (tpQueLock)
                {
                    tp = tpQue.Dequeue();
                }
                if (tp != null)
                {
                    tp.cb(tp.tid);
                }
            }
        }

        public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
        {
            return pt.AddTimeTask(callback, delay, timeUnit, count);
        }
    }
}

