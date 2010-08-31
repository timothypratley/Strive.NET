using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Forms;

using Strive.Client.Model;
using Strive.Common;


namespace Strive.Client.ViewModel
{
    public class Perspective
    {
        public string WindowTitle
        {
            get {
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

        public double Heading = 1.5;
        public double Tilt = -0.15;
        public double X = 0.0;
        public double Y = 0.0;
        public double Z = 23.0;

        private int lastTick = 0;
        private int lastFrameRate = 0;
        private int frameRate = 0;

        public int FPS { get { return lastFrameRate; } }

        public Stopwatch movementTimer;

        public delegate bool KeyPressedCheck(Keys k);
        public delegate MouseButtons MouseButtonCheck();

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
                Func<double,double,double> plus = (a, b) => a + b;
                X = FollowEntities.Select(e => e.Entity.X).Aggregate(plus)/count;
                Y = FollowEntities.Select(e => e.Entity.Y).Aggregate(plus)/count;
                Z = FollowEntities.Select(e => e.Entity.Z).Aggregate(plus)/count + 20.0;
                Tilt = 0.001 - Math.PI / 2.0;
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
        double angleRangeLow = 0.001 - Math.PI/2.0;
        double angleRangeHigh = - 0.001;

        void Down()
        {
            Z -= delta * (distanceRangeHigh - distanceRangeLow) / 10.0;
            if (Z < distanceRangeLow)
                Z = distanceRangeLow;
        }

        void Up()
        {
            Z += delta * (distanceRangeHigh - distanceRangeLow) / 10.0;
            if (Z > distanceRangeHigh)
                Z = distanceRangeHigh;
        }

        void TiltUp()
        {
            Tilt += delta * (angleRangeHigh - angleRangeLow) / 2.0;
            if (Tilt >= angleRangeHigh)
            {
                Tilt = angleRangeHigh;
            }
            FollowEntities.Clear();
        }

        void TiltDown()
        {
            Tilt -= delta * (angleRangeHigh - angleRangeLow) / 2.0;
            if (Tilt < angleRangeLow)
            {
                Tilt = angleRangeLow;
            }
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
            if (Heading >= Math.PI * 2.0)
            {
                Heading -= Math.PI * 2.0;
            }
            FollowEntities.Clear();
        }

        void TurnRight()
        {
            Heading += delta * 2.0;
            if (Heading < 0)
            {
                Heading += Math.PI * 2.0;
            }
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
            FollowEntities = _worldViewModel.SelectedEntities;
        }

        void SetCamera()
        {
            if (movementPerpendicular != 0 || movementForward != 0)
            {
                double movementHeading = Heading
                                         + Math.Atan2(movementForward, -movementPerpendicular);
                double movementX = Math.Sin(movementHeading);
                double movementY = Math.Cos(movementHeading);

                X += movementX * delta * speed * speedModifier;
                Y += movementY * delta * speed * speedModifier;
            }
        }
    }
}
