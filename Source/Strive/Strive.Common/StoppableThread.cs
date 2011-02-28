using System;
using System.Threading;

using Common.Logging;
using System.Diagnostics.Contracts;

namespace Strive.Common
{
    /// <summary>
    /// StoppableThread
    /// </summary>
    public class StoppableThread
    {
        Thread _thisThread;
        ThreadPriority _priority = ThreadPriority.Normal;
        readonly AutoResetEvent _iHaveStopped = new AutoResetEvent(false);
        bool _isRunning;

        public delegate void WhileRunning();

        readonly WhileRunning _whileRunning;
        public StoppableThread(WhileRunning whileRunning)
        {
            Contract.Requires<ArgumentNullException>(whileRunning != null);
            _whileRunning = whileRunning;
        }

        public void Start()
        {
            if (_isRunning) return;
            _thisThread = new Thread(ThreadLoop) {Priority = _priority};
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
            WaitHandle.WaitAny(new[] { _iHaveStopped });
        }

        public ThreadPriority Priority
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
                    _whileRunning();
                }
                catch (Exception e)
                {
                    log.Error("Thread caused exception", e);
                    _isRunning = false;
                }
            }
            _iHaveStopped.Set();
        }
    }
}
