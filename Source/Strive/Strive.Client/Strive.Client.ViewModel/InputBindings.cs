using System.Collections.Generic;
using System.Windows.Input;

namespace Strive.Client.ViewModel
{
    public class InputBindings
    {
        public enum ActionState
        {
            KeyAction,
            CreateAction
        }

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
            FollowSelected,
            Possess,
            Create
        };

        public enum CreationAction
        {
            Item,
            Mobile,
            Factory,
            Plan
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

        public class CreationBinding
        {
            public List<Key> KeyCombo { get; set; }
            public CreationAction Action { get; set; }
            public CreationBinding(List<Key> keys, CreationAction action)
            {
                KeyCombo = keys;
                Action = action;
            }
        };

        public List<KeyBinding> KeyBindings = new List<KeyBinding>();
        public List<CreationBinding> CreationBindings = new List<CreationBinding>();

        public InputBindings()
        {
            Default();
        }

        public void AddKeyBinding(List<Key> keys, KeyAction action)
        {
            KeyBindings.Add(new KeyBinding(keys, action));
        }

        public void AddCreationBinding(List<Key> keys, CreationAction action)
        {
            CreationBindings.Add(new CreationBinding(keys, action));
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
            AddKeyBinding(new List<Key> { Key.LeftShift }, KeyAction.Walk);
            AddKeyBinding(new List<Key> { Key.RightShift }, KeyAction.Walk);
            AddKeyBinding(new List<Key> { Key.Home }, KeyAction.Home);
            AddKeyBinding(new List<Key> { Key.G }, KeyAction.FollowSelected);
            AddKeyBinding(new List<Key> { Key.P }, KeyAction.Possess);
            AddKeyBinding(new List<Key> { Key.C }, KeyAction.Create);

            CreationBindings = new List<CreationBinding>();

            AddCreationBinding(new List<Key> { Key.M }, CreationAction.Mobile);
            AddCreationBinding(new List<Key> { Key.I }, CreationAction.Item);
            AddCreationBinding(new List<Key> { Key.F }, CreationAction.Factory);
            AddCreationBinding(new List<Key> { Key.P }, CreationAction.Plan);
        }
    }
}
