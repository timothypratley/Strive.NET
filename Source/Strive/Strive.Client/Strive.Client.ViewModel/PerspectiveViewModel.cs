using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Media3D;

using UpdateControls.XAML;

using Strive.Client.Model;
using Strive.Common;


namespace Strive.Client.ViewModel
{
    public class PerspectiveViewModel
    {
        public string WindowTitle
        {
            get
            {
                if (_followEntities.Count > 0)
                    return "Following (" + _followEntities + ")";
                return "Fly free view";
            }
        }

        private EnumSkill _currentGameCommand = EnumSkill.None;
        public EnumSkill CurrentGameCommand
        {
            get { return _currentGameCommand; }
            set
            {
                _currentGameCommand = value;
                //ITexture texture = resources.GetCursor((int)currentGameCommand);
                //CurrentWorld.RenderingScene.SetCursor(texture);
            }
        }

        DictionaryModel<string, EntityModel> _followEntities = new DictionaryModel<string, EntityModel>();
        public IEnumerable<EntityViewModel> FollowEntities
        {
            get
            {
                return _followEntities.Entities
                    .Select(em => new EntityViewModel(em, WorldViewModel.Navigation));
            }
        }

        public ICommand CreateEntity
        {
            get
            {
                return MakeCommand
                    .Do(() => WorldViewModel.AddOrReplace("foo", "bar", Position, Rotation));
            }
        }

        private double _heading;
        public double Heading
        {
            get { return _heading; }
            set
            {
                _heading = value;
                while (_heading < 0)
                    _heading += Math.PI * 2.0;
                while (Heading >= Math.PI * 2.0)
                    _heading -= Math.PI * 2.0;
            }
        }

        const double DistanceRangeLow = 10.0;
        const double DistanceRangeHigh = 300.0;
        const double AngleRangeLow = 0.01 - Math.PI / 2.0;
        const double AngleRangeHigh = Math.PI / 2.0 - 0.01;

        double _tilt;
        public double Tilt
        {
            get { return _tilt; }
            set
            {
                if (value < AngleRangeLow)
                    _tilt = AngleRangeLow;
                else if (value >= AngleRangeHigh)
                    _tilt = AngleRangeHigh;
                else
                    _tilt = value;

            }
        }
         
        public Vector3D Position = new Vector3D(0, 0, 23);
        public Quaternion Rotation = Quaternion.Identity;

        private int _lastTick;
        private int _frameRate;

        public int Fps { get; private set; }

        private readonly Stopwatch _movementTimer;

        public delegate bool KeyPressedCheck(Key k);

        public WorldViewModel WorldViewModel { get; private set; }

        readonly KeyPressedCheck _keyPressed;
        readonly InputBindings _bindings;
        readonly ConnectionHandler _connectionHandler;

        public PerspectiveViewModel(WorldViewModel worldViewModel, KeyPressedCheck keyPressed, InputBindings bindings, ConnectionHandler connectionHandler)
        {
            WorldViewModel = worldViewModel;
            _keyPressed = keyPressed;
            _bindings = bindings;
            _connectionHandler = connectionHandler;
            Home();
            _movementTimer = new Stopwatch();
            _movementTimer.Start();
        }

        double _landSpeed = 50.0;

        public void Check()
        {
            Vector3D initialPosition = Position;
            Quaternion initialRotation = Rotation;

            if (Environment.TickCount - _lastTick >= 1000)
            {
                Fps = _frameRate;
                _frameRate = 0;
                _lastTick = Environment.TickCount;
            }
            _frameRate++;

            _movementTimer.Stop();
            double deltaT = _movementTimer.Elapsed.TotalSeconds;
            _movementTimer.Reset();
            _movementTimer.Start();

            // Move toward or follow one or more entities
            var count = _followEntities.Count;
            if (count > 0)
            {
                Vector3D center = _followEntities.Entities
                    .Where(e => WorldViewModel.World.ContainsKey(e.Name))
                    .Average(e => e.Position);
                Vector3D diff = center - Position;
                double vectorDistance = diff.Length;

                // TODO: replace with a proper bounds and fulstrum calculation
                var maxX = _followEntities.Entities.Max(e => e.Position.X);
                var maxY = _followEntities.Entities.Max(e => e.Position.Y);
                var maxZ = _followEntities.Entities.Max(e => e.Position.Z);
                var minX = _followEntities.Entities.Min(e => e.Position.X);
                var minY = _followEntities.Entities.Min(e => e.Position.Y);
                var minZ = _followEntities.Entities.Min(e => e.Position.Z);
                var viewDistance = new List<double> { 10.0, maxX - minX, maxY - minY, maxZ - minZ }.Max();

                Vector3D target = center - (diff * viewDistance / vectorDistance);
                Position += (target - Position) * deltaT * 2;
            }

            // Apply movement based upon inputs
            int movementPerpendicular = 0;
            int movementForward = 0;
            int movementUp = 0;
            double speedModifier = 1;
            foreach (InputBindings.KeyBinding kb in _bindings.KeyBindings
                .Where(kb => kb.KeyCombo.All(k => _keyPressed(k))))
            {
                _followEntities.Clear();

                if (kb.Action == InputBindings.KeyAction.Up)
                    movementUp++;
                else if (kb.Action == InputBindings.KeyAction.Down)
                    movementUp--;
                else if (kb.Action == InputBindings.KeyAction.Left)
                    movementPerpendicular--;
                else if (kb.Action == InputBindings.KeyAction.Right)
                    movementPerpendicular++;
                else if (kb.Action == InputBindings.KeyAction.TurnLeft)
                    Heading -= deltaT * 2.0;
                else if (kb.Action == InputBindings.KeyAction.TurnRight)
                    Heading += deltaT * 2.0;
                else if (kb.Action == InputBindings.KeyAction.TiltUp)
                    Tilt += deltaT * (AngleRangeHigh - AngleRangeLow) / 2.0;
                else if (kb.Action == InputBindings.KeyAction.TiltDown)
                    Tilt -= deltaT * (AngleRangeHigh - AngleRangeLow) / 2.0;
                else if (kb.Action == InputBindings.KeyAction.Walk)
                    speedModifier = 0.2;
                else if (kb.Action == InputBindings.KeyAction.Forward)
                    movementForward++;
                else if (kb.Action == InputBindings.KeyAction.Back)
                    movementForward--;
                else if (kb.Action == InputBindings.KeyAction.Home)
                    Home();
                else if (kb.Action == InputBindings.KeyAction.FollowSelected)
                    OnFollowSelected();
                else
                    throw new Exception("Unexpected keyboard binding " + kb.Action);
            }

            // Set Position and Rotation
            Rotation = new Quaternion(0, 0, 1, Heading) * new Quaternion(0, 1, 0, Tilt);
            if (movementPerpendicular != 0 || movementForward != 0 || movementUp != 0)
            {
                double movementHeading = Heading + Math.Atan2(movementForward, -movementPerpendicular);
                var positionChange = new Vector3D(
                    Math.Sin(movementHeading) * _landSpeed,
                    Math.Cos(movementHeading) * _landSpeed,
                    movementUp * (DistanceRangeHigh - DistanceRangeLow) / 10.0);

                Position += positionChange * deltaT * speedModifier;

                if (Position.Z < DistanceRangeLow)
                    Position.Z = DistanceRangeLow;
                else if (Position.Z > DistanceRangeHigh)
                    Position.Z = DistanceRangeHigh;
            }

            // Send update if required
            if (Position != initialPosition || Rotation != initialRotation)
            {
                _connectionHandler.SendPosition(Position, Rotation);
            }
        }

        void Home()
        {
            Position = new Vector3D(0, 0, 0);
            Tilt = 0.001 - Math.PI / 2.0;
            Heading = 1.5;
            _followEntities.Clear();
        }

        void OnFollowSelected()
        {
            var target = WorldViewModel.Navigation.MouseOverEntity;
            if ( target != null)
            {
                _followEntities.Clear();
                _followEntities.AddEntity(target.Name, target);
                WorldViewModel.Select(target.Name);
            }
            else
                _followEntities = new DictionaryModel<string, EntityModel>(
                    WorldViewModel.Navigation.SelectedEntities
                    .Select(e => new KeyValuePair<string, EntityModel>(e.Name, e)));
        }
    }
}
