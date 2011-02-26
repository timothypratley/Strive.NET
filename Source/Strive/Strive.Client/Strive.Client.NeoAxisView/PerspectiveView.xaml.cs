using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Engine;
using Engine.EntitySystem;
using Engine.MapSystem;
using Engine.Renderer;
using Engine.MathEx;
using Engine.SoundSystem;
using Engine.PhysicsSystem;
using GameEntities;
using Strive.Client.ViewModel;


namespace Strive.Client.NeoAxisView
{
    /// <summary>
    /// Interaction logic for PerspectiveView.xaml
    /// </summary>
    public partial class PerspectiveView
    {
        public PerspectiveViewModel Perspective { get; private set; }
        readonly WorldViewModel _worldViewModel;
        public PerspectiveView(WorldViewModel worldViewModel)
        {
            InitializeComponent();
            _worldViewModel = worldViewModel;
            Perspective = new PerspectiveViewModel(worldViewModel, renderTarget.IsKeyPressed);
            renderTarget.AutomaticUpdateFPS = 60;
            renderTarget.Render += renderTargetUserControl_Render;
            renderTarget.RenderUI += PerspectiveViewControl_RenderUI;
            renderTarget.MouseEnter += PerspectiveViewControl_MouseEnter;
            renderTarget.MouseDown += PerspectiveViewControl_MouseDown;
            renderTarget.MouseUp += PerspectiveViewControl_MouseUp;
            renderTarget.MouseMove += PerspectiveViewControl_MouseMove;
        }

        Camera _camera;
        Vec3 _mouseIntersection = Vec3.Zero;
        void PerspectiveViewControl_RenderUI(GuiRenderer renderer)
        {
            if (_camera != null)
                Nameplates.RenderObjectsTips(renderer, _camera);
            string text = "FPS: " + Perspective.Fps
                        + "    loc: " + renderTarget.CameraPosition.ToString(0)
                        + "    dir: " + renderTarget.CameraDirection.ToString(0)
                        + "    mouse: " + _mouseIntersection.ToString(2)
                        + "    over: " + _worldViewModel.MouseOverEntity;

            renderer.AddText(text, new Vec2(.01f, .01f), HorizontalAlign.Left,
                VerticalAlign.Top, new ColorValue(1, 1, 1));
        }

        void renderTargetUserControl_Render(Camera camera)
        {
            _camera = camera;
            Perspective.Check();
            renderTarget.CameraPosition = Perspective.Position.ToVec3();
            renderTarget.CameraDirection = Perspective.Rotation.ToQuat() * Vec3.XAxis;
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

        private void SetToolTipString()
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

        MapObject _mouseOver;
        void RenderEntityOverCursor(Camera camera)
        {
            Vec2 mouse = renderTarget.GetFloatMousePosition();
            _mouseOver = null;

            if (mouse.X < 0 || mouse.X > 1 || mouse.Y < 0 || mouse.Y > 1)
                _worldViewModel.ClearMouseOverEntity();
            else
            {
                // Find entity under cursor of mouse
                Ray ray = camera.GetCameraToViewportRay(mouse);
                Map.Instance.GetObjects(ray, delegate(MapObject obj, float scale)
                {
                    if (obj is StaticMesh)
                        return true;
                    _mouseOver = obj;
                    return false;
                });

                if (_mouseOver != null)
                {
                    // Put a yellow box around it and a tooltip
                    camera.DebugGeometry.Color = new ColorValue(1, 1, 0);
                    camera.DebugGeometry.AddBounds(_mouseOver.MapBounds);
                    _worldViewModel.SetMouseOverEntity(_mouseOver.Name);
                }
                else
                    _worldViewModel.ClearMouseOverEntity();

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
            // except MouseOver as it will be yellow
            camera.DebugGeometry.Color = new ColorValue(0.5f, 0.5f, 1);
            foreach (MapObject mo in _worldViewModel.SelectedEntities
                .Select(evm => Entities.Instance.GetByName(evm.Entity.Name))
                .OfType<MapObject>()
                .Where(mo => mo != _mouseOver))
            {
                camera.DebugGeometry.AddBounds(mo.MapBounds);
            }
        }

        void PerspectiveViewControl_MouseEnter(object sender, EventArgs e)
        {
            // TODO: remove this event?
            //this.Focus();
        }

        void PerspectiveViewControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Released)
                renderTarget.MouseRelativeMode = false;
        }

        bool _ignoreFirst;   // first relative is screwy, workaround
        void PerspectiveViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (renderTarget.MouseRelativeMode)
            {
                var o = renderTarget.GetMouseRelativeModeOffset();
                if (_ignoreFirst)
                    _ignoreFirst = false;
                else if (o.X != 0 || o.Y != 0)
                {
                    Perspective.UnFollow();
                    Perspective.Heading -= o.X / 2.0;
                    Perspective.Tilt -= o.Y / 2.0;
                }
            }
        }

        readonly Random _rand = new Random();
        void PerspectiveViewControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                renderTarget.MouseRelativeMode = true;
                _ignoreFirst = true;
            }

            EntityViewModel evm = _worldViewModel.SelectedEntities.FirstOrDefault();
            if (evm != null)
            {
                // TODO: set the target
            }
            if (_mouseOver != null)
            {
                if (renderTarget.IsKeyPressed(Key.LeftShift)
                    || renderTarget.IsKeyPressed(Key.LeftCtrl)
                    || renderTarget.IsKeyPressed(Key.RightShift)
                    || renderTarget.IsKeyPressed(Key.RightCtrl))
                    _worldViewModel.SelectAdd(_mouseOver.Name);
                else
                    _worldViewModel.Select(_mouseOver.Name);

                var b = _mouseOver as RTSBuilding;
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

            if (_mouseOver != null && _mouseOver.PhysicsModel != null)
            {
                foreach (Body b in _mouseOver.PhysicsModel.Bodies)
                {
                    b.AddForce(ForceType.Global, 0f, new Vec3(_rand.Next(500) - 250, _rand.Next(500) - 250, _rand.Next(1000) + 250), Vec3.Zero);
                }
            }
        }
    }
}
