using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using AvalonDock;
using Engine;
using Engine.EntitySystem;
using Engine.MapSystem;
using Engine.Renderer;
using Engine.MathEx;
using Engine.SoundSystem;
using Engine.UISystem;
using Engine.EntitySystem;
using Engine.PhysicsSystem;
using Engine.Utils;
using GameCommon;
using GameEntities;
using WPFAppFramework;

using UpdateControls.XAML;
using Strive.Client.Model;
using Strive.Client.ViewModel;


namespace Strive.Client.NeoAxisView
{
    /// <summary>
    /// Interaction logic for WorldView.xaml
    /// </summary>
    public partial class WorldView : DockableContent
    {
        Perspective _perspective;
        public WorldView()
        {
            InitializeComponent();
            _perspective = new Perspective(
                World.ViewModel,
                new Perspective.KeyPressedCheck(r.IsKeyPressed),
                new InputBindings());
            r.AutomaticUpdateFPS = 60;
            r.Render += renderTargetUserControl1_Render;
            r.RenderUI += new RenderTargetUserControl.RenderUIDelegate(WorldViewControl_RenderUI);
            r.MouseEnter += new MouseEventHandler(WorldViewControl_MouseEnter);
            r.MouseDown += new MouseButtonEventHandler(WorldViewControl_MouseDown);
            r.MouseUp += new MouseButtonEventHandler(WorldViewControl_MouseUp);
            r.MouseMove += new MouseEventHandler(WorldViewControl_MouseMove);
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

        Camera camera = null;
        Vec3 MouseIntersection = Vec3.Zero;
        void WorldViewControl_RenderUI(GuiRenderer renderer)
        {
            if (camera != null)
            {
                Nameplates.RenderObjectsTips(renderer, camera);
            }
            string text = "FPS: " + _perspective.FPS
                        + "    loc: " + r.CameraPosition.ToString(0)
                        + "    dir: " + r.CameraDirection.ToString(0)
                        + "    mouse: " + MouseIntersection.ToString(2);

            renderer.AddText(text, new Vec2(.01f, .01f), HorizontalAlign.Left,
                VerticalAlign.Top, new ColorValue(1, 1, 1));
        }

        void renderTargetUserControl1_Render(Camera camera)
        {
            this.camera = camera;
            _perspective.Check();
            r.CameraPosition = new Vec3((float)_perspective.X, (float)_perspective.Y, (float)_perspective.Z);
            r.CameraDirection = new Angles(0f, 0f, MathFunctions.RadToDeg((float)_perspective.Heading)).ToQuat()
                * new Angles(0f, MathFunctions.RadToDeg((float)_perspective.Tilt), 0f).ToQuat()
                * Vec3.XAxis;
            if (SoundWorld.Instance != null)
                SoundWorld.Instance.SetListener(camera.Position, Vec3.Zero, camera.Direction, camera.Up);
            RenderEntityOverCursor(camera);
        }

        // TODO: tooltip does not behave as desired
        System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
        MapObject mapObject = null;
        void RenderEntityOverCursor(Camera camera)
        {
            Vec2 mouse = r.GetFloatMousePosition();
            mapObject = null;

            if (mouse.X < 0 || mouse.X > 1 || mouse.Y < 0 || mouse.Y > 1)
            {
                // tt.ShowAlways = false;
                // tt.RemoveAll();
                tt.StaysOpen = false;
                tt.Content = null;
            }
            else
            {
                // Find entity under cursor of mouse
                Ray ray = camera.GetCameraToViewportRay(mouse);
                Map.Instance.GetObjects(ray, delegate(MapObject obj, float scale)
                {
                    if (obj is StaticMesh)
                        return true;
                    mapObject = obj;
                    return false;
                });

                if (mapObject != null)
                {
                    // Put a yellow box around it and a tooltip
                    camera.DebugGeometry.Color = new ColorValue(1, 1, 0);
                    camera.DebugGeometry.AddBounds(mapObject.MapBounds);
                    //tt.ShowAlways = true;
                    tt.StaysOpen = true;
                    tt.Content = mapObject.Name;
                    World.ViewModel.SetMouseOverEntity(mapObject.Name);
                }
                else
                {
                    //tt.ShowAlways = false;
                    //tt.RemoveAll();
                    tt.StaysOpen = false;
                    tt.Content = null;
                    World.ViewModel.ClearMouseOverEntity();
                }

                RayCastResult result = PhysicsWorld.Instance.RayCast(ray, (int)ContactGroup.CastOnlyCollision);
                MouseIntersection = result.Position;
                if (result.Shape != null)
                {
                    camera.DebugGeometry.Color = new ColorValue(1, 0, 0);
                    camera.DebugGeometry.AddSphere(new Sphere(result.Position, 0.2f));
                }
            }

            // Show all selected entities with a blue box around them
            camera.DebugGeometry.Color = new ColorValue(0.5f, 0.5f, 1);
            foreach (EntityViewModel evm in World.ViewModel.SelectedEntities)
            {
                var mo = Entities.Instance.GetByName(evm.Entity.Name) as MapObject;
                if (mo != null && mo != mapObject)
                {
                    camera.DebugGeometry.AddBounds(mo.MapBounds);
                }
            }
        }

        Random rand = new Random();
        void WorldViewControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                r.MouseRelativeMode = true;
            }

            var em = World.ViewModel.SelectedEntities.FirstOrDefault();
            if (em != null)
            {
                // TODO: set the target
            }
            if (mapObject != null)
            {
                if (r.IsKeyPressed(Key.LeftShift)
                    || r.IsKeyPressed(Key.LeftCtrl)
                    || r.IsKeyPressed(Key.RightShift)
                    || r.IsKeyPressed(Key.RightCtrl))
                    World.ViewModel.SelectAdd(mapObject.Name);
                else
                    World.ViewModel.Select(mapObject.Name);

                var b = mapObject as GameEntities.RTSBuilding;
                if (b != null)
                {
                    RTSUnit unit = (RTSUnit)Entities.Instance.Create(EntityTypes.Instance.GetByName("RTSRobot"), Map.Instance);

                    RTSCharacter character = unit as RTSCharacter;
                    if (character == null)
                        Log.Fatal("RTSBuilding: Create character == null");

                    Vec2 p = GridPathFindSystem.Instance.GetNearestFreePosition(b.Position.ToVec2(),
                        character.Type.Radius * 2);
                    unit.Position = new Vec3(p.X, p.Y, GridPathFindSystem.Instance.GetMotionMapHeight(p) +
                        character.Type.Height * .5f);

                    if (b.Intellect != null)
                        unit.InitialFaction = b.Intellect.Faction;

                    unit.Move(Vec3.Zero);

                    unit.PostCreate();
                }
            }

            if (mapObject != null && mapObject.PhysicsModel != null)
            {
                foreach (Body b in mapObject.PhysicsModel.Bodies)
                {
                    b.AddForce(ForceType.Global, 0f, new Vec3(rand.Next(500) - 250, rand.Next(500) - 250, rand.Next(1000) + 250), new Vec3(0, 0, 0));
                }
            }
        }
    }
}
