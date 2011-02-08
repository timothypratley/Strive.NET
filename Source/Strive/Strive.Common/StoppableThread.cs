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
        Thread _thisThread;
        System.Threading.ThreadPriority _priority = System.Threading.ThreadPriority.Normal;
        AutoResetEvent iHaveStopped = new AutoResetEvent(false);
        bool _isRunning = false;

        public delegate void WhileRunning();
        WhileRunning whileRunning;

        public StoppableThread(WhileRunning wr)
        {
            this.whileRunning = wr;
        }

        public void Start()
        {
            if (_isRunning) return;
            _thisThread = new Thread(new ThreadStart(ThreadLoop)) {Priority = _priority};
            _isRunning = true;
            _thisThread.Start();
        }

        public void Stop()
        {
            if (!_isRunning)
            {
                return;
            }
            _isRunning = false;
            WaitHandle.WaitAny(new AutoResetEvent[] { iHaveStopped });
        }

        public System.Threading.ThreadPriority Priority
        {
            get
            {
                if (_thisThread != null)
                {
                    _priority = _thisThread.Priority;
                }
                return _priority;
            }
            set
            {
                _priority = value;
                if (_thisThread != null)
                {
                    _thisThread.Priority = _priority;
                }
            }
        }

        void ThreadLoop()
        {
            ILog log = LogManager.GetCurrentClassLogger();
            while (_isRunning)
            {
                try
                {
                    whileRunning();
                }
                catch (Exception e)
                {
                    log.Error("Thread caused exception", e);
                    _isRunning = false;
                }
            }
            iHaveStopped.Set();
        }
    }
}
