using System.Windows.Media.Media3D;


namespace Strive.Model
{
    public class TaskModel
    {
        public TaskModel(int id, int missionId, Vector3D finish)
        {
            Id = id;
            MissionId = missionId;
            Finish = finish;
        }

        public int Id { get; private set; }
        public int MissionId { get; private set; }
        public Vector3D Finish { get; private set; }

        public bool Matches(TaskModel t)
        {
            return t.MissionId == MissionId
                && t.Finish == Finish;
        }
    }
}
