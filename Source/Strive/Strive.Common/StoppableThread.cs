using System;
using System.Threading;

using Common.Logging;

namespace Strive.Common
{
    /// <summary>
    /// StoppableThread
    /// </summary>
    public class StoppableThread
    {
        Thread thisThread;
        System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal;
        AutoResetEvent iHaveStopped = new AutoResetEvent(false);
        bool isRunning = false;

        public delegate void WhileRunning();
        WhileRunning whileRunning;

        public StoppableThread(WhileRunning wr)
        {
            this.whileRunning = wr;
        }

        public void Start()
        {
            if (isRunning) return;
            thisThread = new Thread(new ThreadStart(ThreadLoop));
            thisThread.Priority = priority;
            isRunning = true;
            thisThread.Start();
        }

        public void Stop()
        {
            if (!isRunning)
            {
                return;
            }
            isRunning = false;
            WaitHandle.WaitAny(new AutoResetEvent[] { iHaveStopped });
        }

        public System.Threading.ThreadPriority Priority
        {
            get
            {
                if (thisThread != null)
                {
                    priority = thisThread.Priority;
                }
                return priority;
            }
            set
            {
                priority = value;
                if (thisThread != null)
                {
                    thisThread.Priority = priority;
                }
            }
        }

        void ThreadLoop()
        {
            ILog Log = LogManager.GetCurrentClassLogger();
            while (isRunning)
            {
                try
                {
                    whileRunning();
                }
                catch (Exception e)
                {
                    Log.Error("Thread caused exception", e);
                    isRunning = false;
                }
            }
            iHaveStopped.Set();
        }
    }
}
