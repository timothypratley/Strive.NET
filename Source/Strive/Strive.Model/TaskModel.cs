using System.Windows.Media.Media3D;


namespace Strive.Model
{
    public class TaskModel
    {
        public TaskModel(int id, Vector3D start, Vector3D finish)
        {
            Id = id;
            Start = start;
            Finish = finish;
        }

        public int Id { get; private set; }
        public Vector3D Start { get; private set; }
        public Vector3D Finish { get; private set; }
    }
}
