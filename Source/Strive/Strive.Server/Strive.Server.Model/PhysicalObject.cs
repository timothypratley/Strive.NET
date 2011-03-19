using System.Windows.Media.Media3D;


namespace Strive.Server.Model
{
    public abstract class PhysicalObject
    {
        public int ObjectInstanceId;
        public int TemplateObjectId;
        public string TemplateObjectName = "";
        public int ResourceId;
        public float Height;
        public Vector3D Position = new Vector3D(0, 0, 0);
        public Quaternion Rotation = Quaternion.Identity;
        public int MaxHitPoints = 1;
        public int MaxEnergy = 1;
        public float BoundingSphereRadiusSquared;
        private float _hitPoints = 1;
        public float HitPoints
        {
            get { return _hitPoints; }
            set
            {
                if (value > MaxHitPoints)
                    _hitPoints = MaxHitPoints;
                else
                    _hitPoints = value;
            }
        }
        private float _energy = 1;
        public float Energy
        {
            get { return _energy; }
            set
            {
                if (value > MaxEnergy)
                    _energy = MaxEnergy;
                else
                    _energy = value;
            }
        }

        public PhysicalObject() { }

        public PhysicalObject(
            Schema.TemplateObjectRow template,
            Schema.ObjectInstanceRow instance
        )
        {
            ObjectInstanceId = instance.ObjectInstanceID;
            TemplateObjectId = template.TemplateObjectID;
            TemplateObjectName = template.TemplateObjectName;
            Position = new Vector3D(instance.X, instance.Y, instance.Z);
            Rotation = new Quaternion(instance.RotationX, instance.RotationY, instance.RotationZ, instance.RotationW);
            ResourceId = template.ResourceID;
            Height = template.Height;
            // can we get r^2 from ResourceID?
            // TODO: make this the proper 3d radius^2
            // store it in the database
            BoundingSphereRadiusSquared = 1;
        }
    }
}
