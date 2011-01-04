using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;

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

        private double _tilt = -0.15;
        public double Tilt
        {
            get { return _tilt; }
            set
            {
                if (value < angleRangeLow)
                    _tilt = angleRangeLow;
                else if (value >= angleRangeHigh)
                    _tilt = angleRangeHigh;
                else
                    _tilt = value;
            }
        }

        public double X = 0;
        public double Y = 0;

        private double _z = 23;
        public double Z
        {
            get { return _z; }
            set
            {
                if (value < distanceRangeLow)
                    _z = distanceRangeLow;
                else if (value > distanceRangeHigh)
                    _z = distanceRangeHigh;
                else
                    _z = value;
            }
        }

        private int lastTick = 0;
        private int lastFrameRate = 0;
        private int frameRate = 0;

        public int FPS { get { return lastFrameRate; } }

        public Stopwatch movementTimer;

        public delegate bool KeyPressedCheck(Key k);
        public delegate bool MouseButtonCheck();

        WorldViewModel _worldViewModel;
        KeyPressedCheck _keyPressed;
        MouseButtonCheck _mouseButton;
        InputBindings _bindings;

        public Perspective(WorldViewModel worldViewModel, KeyPressedCheck keyPressed, MouseButtonCheck mouseButton, InputBindings bindings)
        {
            _worldViewModel = worldViewModel;
            _keyPressed = keyPressed;
            _mouseButton = mouseButton;
            _bindings = bindings;
            movementTimer = new Stopwatch();
            movementTimer.Start();
        }

        double speed = 50.0;

        int movementPerpendicular;
        int movementForward;
        double speedModifier;
        double delta;

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
                var centerX = FollowEntities.Select(e => e.Entity.X).Sum() / count;
                var centerY = FollowEntities.Select(e => e.Entity.Y).Sum() / count;
                var centerZ = FollowEntities.Select(e => e.Entity.Z).Sum() / count;

                var dX = (centerX - X);
                var dY = (centerY - Y);
                var dZ = (centerZ - Z);

                var vectorDistance = Math.Sqrt(dX * dX + dY * dY + dZ * dZ);

                var maxX = FollowEntities.Select(e => e.Entity.X).Max();
                var maxY = FollowEntities.Select(e => e.Entity.Y).Max();
                var maxZ = FollowEntities.Select(e => e.Entity.Z).Max();
                var minX = FollowEntities.Select(e => e.Entity.X).Min();
                var minY = FollowEntities.Select(e => e.Entity.Y).Min();
                var minZ = FollowEntities.Select(e => e.Entity.Z).Min();

                var viewDistance = new List<double>() { 10.0, maxX - minX, maxY - minY, maxZ - minZ }.Max();

                var targetX = centerX - (dX * viewDistance / vectorDistance);
                var targetY = centerY - (dY * viewDistance / vectorDistance);
                var targetZ = centerZ - (dZ * viewDistance / vectorDistance);

                X += (targetX - X) * delta * 2;
                Y += (targetY - Y) * delta * 2;
                Z += (targetZ - Z) * delta * 2;
                //Tilt = 0.001 - Math.PI / 2.0;
            }

            movementPerpendicular = 0;
            movementForward = 0;
            speedModifier = 1f;
            movementTimer.Stop();
            delta = movementTimer.Elapsed.TotalSeconds;
            movementTimer.Reset();
            movementTimer.Start();

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
            Z -= delta * (distanceRangeHigh - distanceRangeLow) / 10.0;
        }

        void Up()
        {
            Z += delta * (distanceRangeHigh - distanceRangeLow) / 10.0;
        }

        void TiltUp()
        {
            Tilt += delta * (angleRangeHigh - angleRangeLow) / 2.0;
            FollowEntities.Clear();
        }

        void TiltDown()
        {
            Tilt -= delta * (angleRangeHigh - angleRangeLow) / 2.0;
            FollowEntities.Clear();
        }

        void Forward()
        {
            movementForward++;
            FollowEntities.Clear();
        }

        void Back()
        {
            movementForward--;
            FollowEntities.Clear();
        }

        void Left()
        {
            movementPerpendicular--;
            FollowEntities.Clear();
        }

        void Right()
        {
            movementPerpendicular++;
            FollowEntities.Clear();
        }

        void TurnLeft()
        {
            Heading -= delta * 2.0;
            FollowEntities.Clear();
        }

        void TurnRight()
        {
            Heading += delta * 2.0;
            FollowEntities.Clear();
        }

        void Walk()
        {
            speedModifier = 0.2;
        }

        void Home()
        {
            X = 0;
            Y = 0;
            Z = 0;
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
            if (movementPerpendicular != 0 || movementForward != 0)
            {
                double movementHeading = Heading + Math.Atan2(movementForward, -movementPerpendicular);
                double movementX = Math.Sin(movementHeading);
                double movementY = Math.Cos(movementHeading);

                X += movementX * delta * speed * speedModifier;
                Y += movementY * delta * speed * speedModifier;
            }
        }
    }
}
