using System;
using System.Diagnostics.Contracts;
using Strive.Model;


namespace Strive.Client.ViewModel
{
    public class TaskViewModel
    {
        private readonly TaskModel _task;
        private readonly WorldNavigation _entityNavigation;

        public TaskViewModel(TaskModel task, WorldNavigation navigation)
        {
            Contract.Requires<ArgumentNullException>(task != null);
            Contract.Requires<ArgumentNullException>(navigation != null);

            _task = task;
            _entityNavigation = navigation;
        }

        public TaskModel Task { get { return _task; } }

        public bool IsSelected
        {
            get { return _entityNavigation.IsSelected(_task); }
            set
            {
                if (value)
                    _entityNavigation.AddSelected(_task);
                else
                    _entityNavigation.RemoveSelected(_task);
            }
        }

        public override string ToString()
        {
            return _task.Start + " to " + _task.Finish;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            var that = obj as TaskViewModel;
            if (that == null)
                return false;
            return Equals(Task, that.Task);
        }

        public override int GetHashCode()
        {
            return Task.GetHashCode();
        }
    }
}
