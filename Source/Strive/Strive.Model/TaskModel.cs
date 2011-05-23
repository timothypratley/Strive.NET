using System.Windows.Media.Media3D;


namespace Strive.Model
{
    public class TaskModel
    {
        public TaskModel(int id, int planId, Vector3D start, Vector3D finish)
        {
            Id = id;
            PlanId = planId;
            Start = start;
            Finish = finish;
        }

        public int Id { get; private set; }
        public int PlanId { get; private set; }
        public Vector3D Start { get; private set; }
        public Vector3D Finish { get; private set; }

        public bool Matches(TaskModel t)
        {
            return t.Start == Start
                && t.Finish == Finish;
        }
    }
}
