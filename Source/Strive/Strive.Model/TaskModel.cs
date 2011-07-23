using System.Windows.Media.Media3D;


namespace Strive.Model
{
    public class TaskModel
    {
        public TaskModel(int id, int missionId, Vector3D finish, int? doerId)
        {
            Id = id;
            MissionId = missionId;
            Finish = finish;
            DoerId = doerId;
        }

        public int Id { get; private set; }
        public int MissionId { get; private set; }
        public Vector3D Finish { get; private set; }
        public int? DoerId { get; private set; }

        public bool Matches(TaskModel task)
        {
            return task.MissionId == MissionId
                && task.Finish == Finish;
        }

        internal TaskModel WithDoer(int entityId)
        {
            throw new System.NotImplementedException();
        }
    }
}
