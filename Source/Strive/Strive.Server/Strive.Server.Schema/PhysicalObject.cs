using System.Windows.Media.Media3D;


namespace Strive.Server.DB
{
    public abstract class PhysicalObject
    {
        public int ObjectInstanceId;
        public int TemplateObjectId;
        public string TemplateObjectName = "";
        public int ResourceId;
        public float Height;
        public Vector3D Position = new Vector3D();
        public Quaternion Rotation = Quaternion.Identity;
        public int MaxHealth = 1;
        public int MaxEnergy = 1;
        public float BoundingSphereRadiusSquared;
        private float _Health = 1;
        public float Health
        {
            get { return _Health; }
            set
            {
                if (value > MaxHealth)
                    _Health = MaxHealth;
                else
                    _Health = value;
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
