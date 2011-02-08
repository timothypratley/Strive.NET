using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using AvalonDock;
using Engine;
using Engine.EntitySystem;
using Engine.MapSystem;
using Engine.Renderer;
using Engine.MathEx;
using Engine.SoundSystem;
using Engine.PhysicsSystem;
using GameEntities;
using Strive.Client.Model;
using Strive.Client.ViewModel;


namespace Strive.Client.NeoAxisView
{
    /// <summary>
    /// Interaction logic for WorldView.xaml
    /// </summary>
    public partial class WorldView : DockableContent
    {
        readonly PerspectiveViewModel _perspective;
        readonly WorldViewModel _worldViewModel;
        public WorldView(WorldViewModel worldViewModel, ConnectionHandler connectionHandler)
        {
            InitializeComponent();
            _worldViewModel = worldViewModel;
            _perspective = new PerspectiveViewModel(
                worldViewModel,
                r.IsKeyPressed,
                new InputBindings(),
                connectionHandler);
            r.AutomaticUpdateFPS = 60;
            r.Render += renderTargetUserControl1_Render;
            r.RenderUI += WorldViewControl_RenderUI;
            r.MouseEnter += WorldViewControl_MouseEnter;
            r.MouseDown += WorldViewControl_MouseDown;
            r.MouseUp += WorldViewControl_MouseUp;
            r.MouseMove += WorldViewControl_MouseMove;
        }

        Camera _camera;
        Vec3 _mouseIntersection = Vec3.Zero;
        void WorldViewControl_RenderUI(GuiRenderer renderer)
        {
            if (_camera != null)
            {
                Nameplates.RenderObjectsTips(renderer, _camera);
            }
            string text = "FPS: " + _perspective.FPS
                        + "    loc: " + r.CameraPosition.ToString(0)
                        + "    dir: " + r.CameraDirection.ToString(0)
                        + "    mouse: " + _mouseIntersection.ToString(2)
                        + "    over: " + _worldViewModel.MouseOverEntity;

            renderer.AddText(text, new Vec2(.01f, .01f), HorizontalAlign.Left,
                VerticalAlign.Top, new ColorValue(1, 1, 1));
        }

        void renderTargetUserControl1_Render(Camera camera)
        {
            _camera = camera;
            _perspective.Check();
            r.CameraPosition = _perspective.Position.ToVec3();
            r.CameraDirection = _perspective.Rotation.ToQuat() * Vec3.XAxis;
                // TODO: what does it all mean?
                //new Angles(0f, 0f, MathFunctions.RadToDeg((float)_perspective.Heading)).ToQuat()
                //* new Angles(0f, MathFunctions.RadToDeg((float)_perspective.Tilt), 0f).ToQuat()
                //* Vec3.XAxis;
            if (SoundWorld.Instance != null)
                SoundWorld.Instance.SetListener(camera.Position, Vec3.Zero, camera.Direction, camera.Up);
            RenderEntityOverCursor(camera);
        }

        // Would like to do this in XAML - it must be possible but not sure how 
        string _toolTipString;
        public void SetToolTipString()
        {
            var e = _worldViewModel.MouseOverEntity;
            string newTip =  e == null ? null : e.Entity.Name;
            if (newTip != _toolTipString)
            {
                _toolTipString = newTip;
                if (newTip == null)
                {
                    if (ToolTip != null)
                    {
                        ((ToolTip)ToolTip).IsOpen = false;
                    }
                    ToolTip = null;
                }
                else
                {
                    ToolTip = new ToolTip { Content = _toolTipString, IsOpen = true, StaysOpen = true };
                }
            }
        }

        MapObject _mapObject;
        void RenderEntityOverCursor(Camera camera)
        {
            Vec2 mouse = r.GetFloatMousePosition();
            _mapObject = null;

            if (mouse.X < 0 || mouse.X > 1 || mouse.Y < 0 || mouse.Y > 1)
            {
                _worldViewModel.ClearMouseOverEntity();
            }
            else
            {
                // Find entity under cursor of mouse
                Ray ray = camera.GetCameraToViewportRay(mouse);
                Map.Instance.GetObjects(ray, delegate(MapObject obj, float scale)
                {
                    if (obj is StaticMesh)
                        return true;
                    _mapObject = obj;
                    return false;
                });

                if (_mapObject != null)
                {
                    // Put a yellow box around it and a tooltip
                    camera.DebugGeometry.Color = new ColorValue(1, 1, 0);
                    camera.DebugGeometry.AddBounds(_mapObject.MapBounds);
                    _worldViewModel.SetMouseOverEntity(_mapObject.Name);
                }
                else
                {
                    _worldViewModel.ClearMouseOverEntity();
                }

                RayCastResult result = PhysicsWorld.Instance.RayCast(ray, (int)ContactGroup.CastOnlyCollision);
                _mouseIntersection = result.Position;
                if (result.Shape != null)
                {
                    camera.DebugGeometry.Color = new ColorValue(1, 0, 0);
                    camera.DebugGeometry.AddSphere(new Sphere(result.Position, 0.2f));
                }
            }
            SetToolTipString();

            // Show all selected entities with a blue box around them
            camera.DebugGeometry.Color = new ColorValue(0.5f, 0.5f, 1);
            foreach (EntityViewModel evm in _worldViewModel.SelectedEntities)
            {
                var mo = Entities.Instance.GetByName(evm.Entity.Name) as MapObject;
                if (mo != null && mo != _mapObject)
                {
                    camera.DebugGeometry.AddBounds(mo.MapBounds);
                }
            }
        }

        void WorldViewControl_MouseEnter(object sender, EventArgs e)
        {
            //this.Focus();
        }

        void WorldViewControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Released)
            {
                r.MouseRelativeMode = false;
            }
        }

        void WorldViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (r.MouseRelativeMode)
            {
                var o = r.GetMouseRelativeModeOffset();
                _perspective.Heading += o.X / 200f;
                _perspective.Tilt -= o.Y / 200f;
            }
        }

        readonly Random _rand = new Random();
        void WorldViewControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                r.MouseRelativeMode = true;
            }

            var em = _worldViewModel.SelectedEntities.FirstOrDefault();
            if (em != null)
            {
                // TODO: set the target
            }
            if (_mapObject != null)
            {
                if (r.IsKeyPressed(Key.LeftShift)
                    || r.IsKeyPressed(Key.LeftCtrl)
                    || r.IsKeyPressed(Key.RightShift)
                    || r.IsKeyPressed(Key.RightCtrl))
                    _worldViewModel.SelectAdd(_mapObject.Name);
                else
                    _worldViewModel.Select(_mapObject.Name);

                var b = _mapObject as RTSBuilding;
                if (b != null)
                {
                    var unit = (RTSUnit)Entities.Instance.Create(EntityTypes.Instance.GetByName("RTSRobot"), Map.Instance);

                    var character = unit as RTSCharacter;
                    if (character == null)
                        Log.Fatal("RTSBuilding: Create character == null");
                    else
                    {
                        Vec2 p = GridPathFindSystem.Instance.GetNearestFreePosition(
                            b.Position.ToVec2(), character.Type.Radius*2);
                        unit.Position = new Vec3(p.X, p.Y,
                                                 GridPathFindSystem.Instance.GetMotionMapHeight(p) +
                                                 character.Type.Height*.5f);
                    }

                    if (b.Intellect != null)
                        unit.InitialFaction = b.Intellect.Faction;

                    unit.Move(Vec3.Zero);

                    unit.PostCreate();
                }
            }

            if (_mapObject != null && _mapObject.PhysicsModel != null)
            {
                foreach (Body b in _mapObject.PhysicsModel.Bodies)
                {
                    b.AddForce(ForceType.Global, 0f, new Vec3(_rand.Next(500) - 250, _rand.Next(500) - 250, _rand.Next(1000) + 250), new Vec3(0, 0, 0));
                }
            }
        }
    }
}
