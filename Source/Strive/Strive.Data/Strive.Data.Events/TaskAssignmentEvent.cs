using Strive.Model;

namespace Strive.Data.Events
{
    public class TaskAssignmentEvent : Event
    {
        public TaskAssignmentEvent(TaskModel task, int doerId, string description)
        {
            Task = task;
            DoerId = doerId;
            Description = description;
        }

        public TaskModel Task { get; set; }
        public int DoerId { get; set; }
    }
}
