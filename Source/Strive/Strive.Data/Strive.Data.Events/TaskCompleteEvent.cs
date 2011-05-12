using Strive.Model;

namespace Strive.Data.Events
{
    public class TaskCompleteEvent : Event
    {
        public TaskCompleteEvent(EntityModel doer, TaskModel task, string description)
        {
            Doer = doer;
            Task = task;
            Description = description;
        }

        public TaskModel Task { get; set; }
        public EntityModel Doer { get; set; }
    }
}
