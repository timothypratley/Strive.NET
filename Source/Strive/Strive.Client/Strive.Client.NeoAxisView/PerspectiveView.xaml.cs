using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.EntitySystem;
using Engine.MapSystem;
using Engine.MathEx;
using Engine.PhysicsSystem;
using Engine.Renderer;
using Engine.SoundSystem;
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
            renderTarget.MouseDoubleClick += (s, e) => _worldViewModel.FollowSelected.Execute(null);
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
            if (Perspective.IsPrimary)
                WorldView.UpdateFromWorldModel();
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
        private void SetToolTipString()
        {
            var e = _worldViewModel.MouseOverEntity;
            var tt = ToolTip as ToolTip;
            if (tt == null)
                ToolTip = tt = new ToolTip();
            if (_worldViewModel.IsMouseOverEntity)
            {
                tt.Content = _worldViewModel.MouseOverEntity.Entity.Name;
                tt.IsOpen = true;
            }
            else
            {
                tt.IsOpen = false;
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
                    if (!(obj.UserData is int))
                        return true;
                    _mouseOver = obj;
                    return false;
                });

                if (_mouseOver != null)
                {
                    // Put a yellow box around it and a tooltip
                    camera.DebugGeometry.Color = new ColorValue(1, 1, 0);
                    camera.DebugGeometry.AddBounds(_mouseOver.MapBounds);
                    _worldViewModel.WorldNavigation.MouseOverEntity = (int)_mouseOver.UserData;
                }
                else
                    _worldViewModel.ClearMouseOverEntity();

                // TODO: after removing the physical bodies, this ray-cast never hits anything of course...
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
            foreach (MapObject mo in _worldViewModel.WorldNavigation.SelectedEntities
                .Select(x => Entities.Instance.GetByName(x.ToString()))
                .OfType<MapObject>()
                .Where(mo => mo != _mouseOver))
                camera.DebugGeometry.AddBounds(mo.MapBounds);
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

            if (_mouseOver != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var id = (int)_mouseOver.UserData;
                if (renderTarget.IsKeyPressed(Key.LeftShift)
                    || renderTarget.IsKeyPressed(Key.LeftCtrl)
                    || renderTarget.IsKeyPressed(Key.RightShift)
                    || renderTarget.IsKeyPressed(Key.RightCtrl))
                    _worldViewModel.WorldNavigation.AddSelectedEntity(id);
                else
                    _worldViewModel.WorldNavigation.SetSelectedEntity(id);

                // TODO: this is a spaghetti way to fire the command, fix it
                if (_mouseOver is RTSBuilding)
                    _worldViewModel.ProduceEntity.Execute(null);
            }

            // TODO: This is just for fun, will remove it later
            if (_mouseOver != null && _mouseOver.PhysicsModel != null)
                foreach (Body b in _mouseOver.PhysicsModel.Bodies)
                    b.AddForce(ForceType.Global, 0f,
                        new Vec3(_rand.Next(500) - 250, _rand.Next(500) - 250, _rand.Next(1000) + 250),
                        Vec3.Zero);
        }
    }
}
