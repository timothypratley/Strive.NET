using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Strive.Client.ViewModel
{
    public class InputBindings
    {
        public enum KeyAction
        {
            Up,
            Down,
            TiltUp,
            TiltDown,
            Forward,
            Back,
            Left,
            Right,
            TurnLeft,
            TurnRight,
            Walk,
            Home,
            FollowSelected
        };

        public class KeyBinding
        {
            public List<Keys> KeyCombo { get; set; }
            public KeyAction Action { get; set; }
            public KeyBinding(List<Keys> keys, KeyAction action)
            {
                KeyCombo = keys;
                Action = action;
            }
        };

        public List<KeyBinding> KeyBindings = new List<KeyBinding>();

        public InputBindings()
        {
            Default();
        }

        public void AddKeyBinding(List<Keys> keys, KeyAction action)
        {
            KeyBindings.Add(new KeyBinding(keys, action));
        }

        public void Default()
        {
            KeyBindings = new List<KeyBinding>();

            AddKeyBinding(new List<Keys> { Keys.PageUp }, KeyAction.Up);
            AddKeyBinding(new List<Keys> { Keys.PageDown }, KeyAction.Down);
            AddKeyBinding(new List<Keys> { Keys.R }, KeyAction.TiltUp);
            AddKeyBinding(new List<Keys> { Keys.F }, KeyAction.TiltDown);
            AddKeyBinding(new List<Keys> { Keys.W }, KeyAction.Forward);
            AddKeyBinding(new List<Keys> { Keys.S }, KeyAction.Back);
            AddKeyBinding(new List<Keys> { Keys.A }, KeyAction.Left);
            AddKeyBinding(new List<Keys> { Keys.D }, KeyAction.Right);
            AddKeyBinding(new List<Keys> { Keys.Q }, KeyAction.TurnLeft);
            AddKeyBinding(new List<Keys> { Keys.E }, KeyAction.TurnRight);
            AddKeyBinding(new List<Keys> { Keys.Shift }, KeyAction.Walk);
            AddKeyBinding(new List<Keys> { Keys.Home }, KeyAction.Home);
            AddKeyBinding(new List<Keys> { Keys.F8 }, KeyAction.FollowSelected);
        }
    }
}
