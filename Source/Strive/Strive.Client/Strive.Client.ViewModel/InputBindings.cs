using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

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
            public List<Key> KeyCombo { get; set; }
            public KeyAction Action { get; set; }
            public KeyBinding(List<Key> keys, KeyAction action)
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

        public void AddKeyBinding(List<Key> keys, KeyAction action)
        {
            KeyBindings.Add(new KeyBinding(keys, action));
        }

        public void Default()
        {
            KeyBindings = new List<KeyBinding>();

            AddKeyBinding(new List<Key> { Key.PageUp }, KeyAction.Up);
            AddKeyBinding(new List<Key> { Key.PageDown }, KeyAction.Down);
            AddKeyBinding(new List<Key> { Key.R }, KeyAction.TiltUp);
            AddKeyBinding(new List<Key> { Key.F }, KeyAction.TiltDown);
            AddKeyBinding(new List<Key> { Key.W }, KeyAction.Forward);
            AddKeyBinding(new List<Key> { Key.S }, KeyAction.Back);
            AddKeyBinding(new List<Key> { Key.A }, KeyAction.Left);
            AddKeyBinding(new List<Key> { Key.D }, KeyAction.Right);
            AddKeyBinding(new List<Key> { Key.Q }, KeyAction.TurnLeft);
            AddKeyBinding(new List<Key> { Key.E }, KeyAction.TurnRight);
            AddKeyBinding(new List<Key> { Key.LeftShift, Key.RightShift }, KeyAction.Walk);
            AddKeyBinding(new List<Key> { Key.Home }, KeyAction.Home);
            AddKeyBinding(new List<Key> { Key.G }, KeyAction.FollowSelected);
        }
    }
}
