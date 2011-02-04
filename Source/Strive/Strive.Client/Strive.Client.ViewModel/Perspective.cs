using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Media3D;

using UpdateControls.XAML;

using Strive.Client.Model;
using Strive.Common;


namespace Strive.Client.ViewModel
{
    public class Perspective
    {
        public string WindowTitle
        {
            get
            {
                if (FollowEntities.Count > 0)
                    return "Following (" + FollowEntities.ToString() + ")";
                else
                    return "Fly free view";
            }
        }

        private EnumSkill currentGameCommand = EnumSkill.None;
        public EnumSkill CurrentGameCommand
        {
            get
            {
                return currentGameCommand;
            }
            set
            {
                currentGameCommand = value;
                //ITexture texture = resources.GetCursor((int)currentGameCommand);
                //CurrentWorld.RenderingScene.SetCursor(texture);
            }
        }

        public List<EntityViewModel> FollowEntities = new List<EntityViewModel>();

        private double _heading = 1.5;
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

        double _distanceRangeLow = 10.0;
        double _distanceRangeHigh = 300.0;
        double _angleRangeLow = 0.01 - Math.PI / 2.0;
        double _angleRangeHigh = Math.PI / 2.0 - 0.01;

        double _tilt = 0;
        public double Tilt
        {
            get { return _tilt; }
            set
            {
                if (value < _angleRangeLow)
                    _tilt = _angleRangeLow;
                else if (value >= _angleRangeHigh)
                    _tilt = _angleRangeHigh;
                else
                    _tilt = value;

            }
        }
         
        public Vector3D Position = new Vector3D(0, 0, 23);
        public Quaternion Rotation = Quaternion.Identity;

        private int lastTick = 0;
        private int lastFrameRate = 0;
        private int frameRate = 0;

        public int FPS { get { return lastFrameRate; } }

        private Stopwatch _movementTimer;

        public delegate bool KeyPressedCheck(Key k);

        WorldViewModel _worldViewModel;
        KeyPressedCheck _keyPressed;
        InputBindings _bindings;
        ConnectionHandler _connectionHandler;

        public Perspective(WorldViewModel worldViewModel, KeyPressedCheck keyPressed, InputBindings bindings, ConnectionHandler connectionHandler)
        {
            _worldViewModel = worldViewModel;
            _keyPressed = keyPressed;
            _bindings = bindings;
            _connectionHandler = connectionHandler;
            Home();
            _movementTimer = new Stopwatch();
            _movementTimer.Start();
        }

        double _landSpeed = 50.0;

        int _movementPerpendicular;
        int _movementForward;
        int _movementUp;
        double _speedModifier;
        double _deltaT;

        public void Check()
        {
            Vector3D initialPosition = Position;
            Quaternion initialRotation = Rotation;

            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;

            var count = FollowEntities.Count;
            if (count > 0)
            {
                Vector3D center = FollowEntities.Select(e => e.Entity.Position)
                    .Aggregate((a, b) => a + b)
                    / FollowEntities.Count;
                Vector3D diff = center - Position;
                double vectorDistance = diff.Length;

                // TODO: replace with a proper bounds and fulstrum calculation
                var maxX = FollowEntities.Max(e => e.Entity.Position.X);
                var maxY = FollowEntities.Max(e => e.Entity.Position.Y);
                var maxZ = FollowEntities.Max(e => e.Entity.Position.Z);
                var minX = FollowEntities.Min(e => e.Entity.Position.X);
                var minY = FollowEntities.Min(e => e.Entity.Position.Y);
                var minZ = FollowEntities.Min(e => e.Entity.Position.Z);
                var viewDistance = new List<double>() { 10.0, maxX - minX, maxY - minY, maxZ - minZ }.Max();

                Vector3D target = center - (diff * viewDistance / vectorDistance);
                Position += (target - Position) * _deltaT * 2;
            }

            _movementPerpendicular = 0;
            _movementForward = 0;
            _movementUp = 0;
            _speedModifier = 1f;
            _movementTimer.Stop();
            _deltaT = _movementTimer.Elapsed.TotalSeconds;
            _movementTimer.Reset();
            _movementTimer.Start();

            foreach (InputBindings.KeyBinding kb in _bindings.KeyBindings)
            {
                if (kb.KeyCombo.All(k => _keyPressed(k)))
                {
                    FollowEntities.Clear();

                    if (kb.Action == InputBindings.KeyAction.Up)
                        _movementUp++;
                    else if (kb.Action == InputBindings.KeyAction.Down)
                        _movementUp--;
                    else if (kb.Action == InputBindings.KeyAction.Left)
                        _movementPerpendicular--;
                    else if (kb.Action == InputBindings.KeyAction.Right)
                        _movementPerpendicular++;
                    else if (kb.Action == InputBindings.KeyAction.TurnLeft)
                        Heading -= _deltaT * 2.0;
                    else if (kb.Action == InputBindings.KeyAction.TurnRight)
                        Heading += _deltaT * 2.0;
                    else if (kb.Action == InputBindings.KeyAction.TiltUp)
                        Tilt += _deltaT * (_angleRangeHigh - _angleRangeLow) / 2.0;
                    else if (kb.Action == InputBindings.KeyAction.TiltDown)
                        Tilt -= _deltaT * (_angleRangeHigh - _angleRangeLow) / 2.0;
                    else if (kb.Action == InputBindings.KeyAction.Walk)
                        _speedModifier = 0.2;
                    else if (kb.Action == InputBindings.KeyAction.Forward)
                        _movementForward++;
                    else if (kb.Action == InputBindings.KeyAction.Back)
                        _movementForward--;
                    else if (kb.Action == InputBindings.KeyAction.Home)
                        Home();
                    else if (kb.Action == InputBindings.KeyAction.FollowSelected)
                        OnFollowSelected();
                    else
                        throw new Exception("Unexpected keyboard binding " + kb.Action);
                }
            }

            // Set Position
            if (_movementPerpendicular != 0 || _movementForward != 0 || _movementUp != 0)
            {
                double movementHeading = Heading + Math.Atan2(_movementForward, -_movementPerpendicular);
                Vector3D positionChange = new Vector3D(
                    Math.Sin(movementHeading) * _landSpeed,
                    Math.Cos(movementHeading) * _landSpeed,
                    _movementUp * (_distanceRangeHigh - _distanceRangeLow) / 10.0);

                Position += positionChange * _deltaT * _speedModifier;

                if (Position.Z < _distanceRangeLow)
                    Position.Z = _distanceRangeLow;
                else if (Position.Z > _distanceRangeHigh)
                    Position.Z = _distanceRangeHigh;
            }

            Rotation = new Quaternion(0, 0, 1, Heading) * new Quaternion(0, 1, 0, Tilt);

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
            Heading = 0;
            FollowEntities.Clear();
        }

        void OnFollowSelected()
        {
            if (_worldViewModel.MouseOverEntity != null)
            {
                FollowEntities = new List<EntityViewModel>() { _worldViewModel.MouseOverEntity };
                _worldViewModel.Select(_worldViewModel.MouseOverEntity.Entity.Name);
            }
            else
                FollowEntities = _worldViewModel.SelectedEntities;
        }
    }
}
