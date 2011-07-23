using System;
using System.Diagnostics.Contracts;
using Strive.Model;


namespace Strive.Client.ViewModel
{
    public class TaskViewModel
    {
        private readonly TaskModel _task;
        private readonly EntityModel _doer;
        private readonly WorldNavigation _entityNavigation;

        public TaskViewModel(TaskModel task, WorldNavigation navigation, EntityModel doer)
        {
            Contract.Requires<ArgumentNullException>(task != null);
            Contract.Requires<ArgumentNullException>(navigation != null);

            _task = task;
            _doer = doer;
            _entityNavigation = navigation;
        }

        public TaskModel Task { get { return _task; } }
        public EntityModel Doer { get { return _doer; } }

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
            return "Go to " + _task.Finish;
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
