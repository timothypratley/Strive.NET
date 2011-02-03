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

        /**
        public void Limit()
        {
                if (value < angleRangeLow)
                    _tilt = angleRangeLow;
                else if (value >= angleRangeHigh)
                    _tilt = angleRangeHigh;
                else
                    _tilt = value;
                if (value < distanceRangeLow)
                    _z = distanceRangeLow;
                else if (value > distanceRangeHigh)
                    _z = distanceRangeHigh;
                else
                    _z = value;
        }
        */

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

        public Perspective(WorldViewModel worldViewModel, KeyPressedCheck keyPressed, InputBindings bindings)
        {
            _worldViewModel = worldViewModel;
            _keyPressed = keyPressed;
            _bindings = bindings;
            _movementTimer = new Stopwatch();
            _movementTimer.Start();
        }

        double _speed = 50.0;

        int _movementPerpendicular;
        int _movementForward;
        double _speedModifier;
        double _deltaT;

        public void Check()
        {
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
            _speedModifier = 1f;
            _movementTimer.Stop();
            _deltaT = _movementTimer.Elapsed.TotalSeconds;
            _movementTimer.Reset();
            _movementTimer.Start();

            foreach (InputBindings.KeyBinding kb in _bindings.KeyBindings)
            {
                if (kb.KeyCombo.All(k => _keyPressed(k)))
                {
                    if (kb.Action == InputBindings.KeyAction.Up)
                        Up();
                    else if (kb.Action == InputBindings.KeyAction.Down)
                        Down();
                    else if (kb.Action == InputBindings.KeyAction.Left)
                        Left();
                    else if (kb.Action == InputBindings.KeyAction.Right)
                        Right();
                    else if (kb.Action == InputBindings.KeyAction.TurnLeft)
                        TurnLeft();
                    else if (kb.Action == InputBindings.KeyAction.TurnRight)
                        TurnRight();
                    else if (kb.Action == InputBindings.KeyAction.TiltUp)
                        TiltUp();
                    else if (kb.Action == InputBindings.KeyAction.TiltDown)
                        TiltDown();
                    else if (kb.Action == InputBindings.KeyAction.Walk)
                        Walk();
                    else if (kb.Action == InputBindings.KeyAction.Forward)
                        Forward();
                    else if (kb.Action == InputBindings.KeyAction.Back)
                        Back();
                    else if (kb.Action == InputBindings.KeyAction.Home)
                        Home();
                    else if (kb.Action == InputBindings.KeyAction.FollowSelected)
                        OnFollowSelected();
                    else
                        throw new Exception("Unexpected keyboard binding " + kb.Action);
                }
            }
            SetCamera();
        }

        double distanceRangeLow = 10.0;
        double distanceRangeHigh = 300.0;
        double angleRangeLow = 0.01 - Math.PI / 2.0;
        double angleRangeHigh = Math.PI / 2.0 - 0.01;

        void Down()
        {
            Z -= _deltaT * (distanceRangeHigh - distanceRangeLow) / 10.0;
        }

        void Up()
        {
            Z += _deltaT * (distanceRangeHigh - distanceRangeLow) / 10.0;
        }

        void TiltUp()
        {
            Tilt += _deltaT * (angleRangeHigh - angleRangeLow) / 2.0;
            FollowEntities.Clear();
        }

        void TiltDown()
        {
            Tilt -= _deltaT * (angleRangeHigh - angleRangeLow) / 2.0;
            FollowEntities.Clear();
        }

        void Forward()
        {
            _movementForward++;
            FollowEntities.Clear();
        }

        void Back()
        {
            _movementForward--;
            FollowEntities.Clear();
        }

        void Left()
        {
            _movementPerpendicular--;
            FollowEntities.Clear();
        }

        void Right()
        {
            _movementPerpendicular++;
            FollowEntities.Clear();
        }

        void TurnLeft()
        {
            Heading -= _deltaT * 2.0;
            FollowEntities.Clear();
        }

        void TurnRight()
        {
            Heading += _deltaT * 2.0;
            FollowEntities.Clear();
        }

        void Walk()
        {
            _speedModifier = 0.2;
        }

        void Home()
        {
            Position = new Vector3D(0, 0, 0);
            Tilt = 0.001 - Math.PI / 2.0;
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

        void SetCamera()
        {
            if (_movementPerpendicular != 0 || _movementForward != 0)
            {
                double movementHeading = Heading + Math.Atan2(_movementForward, -_movementPerpendicular);
                double movementX = Math.Sin(movementHeading);
                double movementY = Math.Cos(movementHeading);

                X += movementX * _deltaT * _speed * _speedModifier;
                Y += movementY * _deltaT * _speed * _speedModifier;
            }
        }
    }
}
