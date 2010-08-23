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
                if (FollowSelected)
                    return "Follow Selected (" + FollowEntity == null ? "None" : FollowEntity.Name + ")";
                else if (FollowEntity != null)
                    return FollowEntity.Name;
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

        public EntityModel FollowEntity = null;
        public bool FollowSelected = false;

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

        KeyPressedCheck _keyPressed;
        MouseButtonCheck _mouseButton;
        InputBindings _bindings;

        public Perspective(KeyPressedCheck keyPressed, MouseButtonCheck mouseButton, InputBindings bindings)
        {
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

            if (FollowEntity != null)
            {
                X = FollowEntity.X;
                Y = FollowEntity.Y;
                Z = FollowEntity.Z + 20.0;
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
        }

        void TiltDown()
        {
            Tilt -= delta * (angleRangeHigh - angleRangeLow) / 2.0;
            if (Tilt < angleRangeLow)
            {
                Tilt = angleRangeLow;
            }
        }

        void Forward()
        {
            movementForward++;
        }

        void Back()
        {
            movementForward--;
        }

        void Left()
        {
            movementPerpendicular--;
        }

        void Right()
        {
            movementPerpendicular++;
        }

        void TurnLeft()
        {
            Heading -= delta * 2.0;
            if (Heading >= Math.PI * 2.0)
            {
                Heading -= Math.PI * 2.0;
            }
        }

        void TurnRight()
        {
            Heading += delta * 2.0;
            if (Heading < 0)
            {
                Heading += Math.PI * 2.0;
            }
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
        }

        void OnFollowSelected()
        {
            FollowSelected = !FollowSelected;
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
