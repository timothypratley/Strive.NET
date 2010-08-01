using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Forms;


namespace Strive.Client.ViewModel
{
    public class Perspective
    {
        double cameraHeading = 1.5;
        public double Heading { get { return cameraHeading; } }

        double cameraTilt = 0.95;
        public double Tilt { get { return cameraTilt - Math.PI / 2.0; } }

        double cameraX = 0.0;
        public double PositionX { get { return cameraX; } }

        double cameraY = 0.0;
        public double PositionY { get { return cameraY; } }

        double cameraZ = 23.0;
        public double PositionZ { get { return cameraZ; } }

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
                    {
                        Up();
                    }
                    else if (kb.Action == InputBindings.KeyAction.Down)
                    {
                        Down();
                    }
                    else if (kb.Action == InputBindings.KeyAction.Left)
                    {
                        Left();
                    }
                    else if (kb.Action == InputBindings.KeyAction.Right)
                    {
                        Right();
                    }
                    else if (kb.Action == InputBindings.KeyAction.TurnLeft)
                    {
                        TurnLeft();
                    }
                    else if (kb.Action == InputBindings.KeyAction.TurnRight)
                    {
                        TurnRight();
                    }
                    else if (kb.Action == InputBindings.KeyAction.TiltUp)
                    {
                        TiltUp();
                    }
                    else if (kb.Action == InputBindings.KeyAction.TiltDown)
                    {
                        TiltDown();
                    }
                    else if (kb.Action == InputBindings.KeyAction.Walk)
                    {
                        Walk();
                    }
                    else if (kb.Action == InputBindings.KeyAction.Forward)
                    {
                        Forward();
                    }
                    else if (kb.Action == InputBindings.KeyAction.Back)
                    {
                        Back();
                    }
                    else
                    {
                        throw new Exception("Unexpected keyboard binding " + kb.Action);
                    }
                }
            }
            SetCamera();
        }

        double distanceRangeLow = 10.0;
        double distanceRangeHigh = 300.0;
        double angleRangeLow = 0.001;
        double angleRangeHigh = Math.PI / 2.0 - 0.001;

        void Down()
        {
            cameraZ -= delta * (distanceRangeHigh - distanceRangeLow) / 10.0;
            if (cameraZ < distanceRangeLow)
                cameraZ = distanceRangeLow;
        }

        void Up()
        {
            cameraZ += delta * (distanceRangeHigh - distanceRangeLow) / 10.0;
            if (cameraZ > distanceRangeHigh)
                cameraZ = distanceRangeHigh;
        }

        void TiltUp()
        {
            cameraTilt += delta * (angleRangeHigh - angleRangeLow) / 2.0;
            if (cameraTilt >= angleRangeHigh)
            {
                cameraTilt = angleRangeHigh;
            }
        }

        void TiltDown()
        {
            cameraTilt -= delta * (angleRangeHigh - angleRangeLow) / 2.0;
            if (cameraTilt < angleRangeLow)
            {
                cameraTilt = angleRangeLow;
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
            cameraHeading -= delta * 2.0;
            if (cameraHeading >= Math.PI * 2.0)
            {
                cameraHeading -= Math.PI * 2.0;
            }
        }

        void TurnRight()
        {
            cameraHeading += delta * 2.0;
            if (cameraHeading < 0)
            {
                cameraHeading += Math.PI * 2.0;
            }
        }

        void Walk()
        {
            speedModifier = 0.2;
        }


        void SetCamera()
        {
            if (movementPerpendicular != 0 || movementForward != 0)
            {
                double movementHeading = cameraHeading
                                         + Math.Atan2(movementForward, -movementPerpendicular);
                double movementX = Math.Sin(movementHeading);
                double movementY = Math.Cos(movementHeading);

                cameraX += movementX * delta * speed * speedModifier;
                cameraY += movementY * delta * speed * speedModifier;
            }
        }
    }
}
