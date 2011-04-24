using Strive.Model;

namespace Strive.Network.Messages.ToServer
{
    public class CreateTask
    {
        public CreateTask(TaskModel task)
        {
            Task = task;
        }

        public TaskModel Task;
    }
}
