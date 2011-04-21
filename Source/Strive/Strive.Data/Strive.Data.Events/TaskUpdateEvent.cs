using Strive.Model;

namespace Strive.Data.Events
{
    public class TaskUpdateEvent : Event
    {
        public TaskUpdateEvent(TaskModel task, string description)
        {
            Task = task;
            Description = description;
        }

        public TaskModel Task { get; set; }
    }
}
