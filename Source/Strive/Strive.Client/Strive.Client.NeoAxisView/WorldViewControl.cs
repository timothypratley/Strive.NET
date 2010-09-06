using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using Engine.MathEx;
using Engine.Renderer;
using Engine.MapSystem;
using Engine.EntitySystem;
using Engine.PhysicsSystem;
using Engine.Utils;
using Engine;
using GameCommon;
using GameEntities;
using WindowsAppFramework;

using UpdateControls.XAML;
using Strive.Client.Model;
using Strive.Client.ViewModel;


namespace Strive.Client.NeoAxisView
{
    public partial class WorldViewControl : RenderTargetUserControl
    {
        Perspective _perspective;
        public WorldViewControl()
        {
            _perspective = new Perspective(
                World.ViewModel,
                new Perspective.KeyPressedCheck(IsKeyPressed),
                new Perspective.MouseButtonCheck(GetMouseButtons),
                new InputBindings());
            AutomaticUpdateFPS = 60;
            Render += renderTargetUserControl1_Render;
            RenderUI += new RenderUIDelegate(WorldViewControl_RenderUI);
            MouseClick += new MouseEventHandler(renderTargetUserControl1_MouseClick);
            MouseEnter += new EventHandler(WorldViewControl_MouseEnter);
            MouseDown += new MouseEventHandler(WorldViewControl_MouseDown);
            MouseUp += new MouseEventHandler(WorldViewControl_MouseUp);
            MouseMove += new MouseEventHandler(WorldViewControl_MouseMove);
        }

        // TODO: use dependencies instead
        void EntitiesView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (EntityModel em in e.NewItems)
                {
                    // load it
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (EntityModel em in e.OldItems)
                {
                    // remove them
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
            }
            else
            {
                throw new NotImplementedException(e.Action.ToString());
            }
        }

        public MouseButtons GetMouseButtons()
        {
            return MouseButtons;
        }

        void WorldViewControl_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        int mouseX;
        int mouseY;
        void WorldViewControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                MouseRelativeMode = false;
            }
        }

        void WorldViewControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                MouseRelativeMode = true;
                mouseX = MousePosition.X;
                mouseY = MousePosition.Y;
            }
        }

        void WorldViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseRelativeMode)
            {
                var o = GetMouseRelativeModeOffset();
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
                        + "    loc: " + CameraPosition.ToString(0)
                        + "    dir: " + CameraDirection.ToString(0)
                        + "    mouse: " + MouseIntersection.ToString(2);

            renderer.AddText(text, new Vec2(.01f, .01f), HorizontalAlign.Left,
                VerticalAlign.Top, new ColorValue(1, 1, 1));
        }

        void renderTargetUserControl1_Render(Camera camera)
        {
            this.camera = camera;
            RenderEntityOverCursor(camera);
            _perspective.Check();
            CameraPosition = new Vec3((float)_perspective.X, (float)_perspective.Y, (float)_perspective.Z);
            CameraDirection = new Angles(0f, 0f, MathFunctions.RadToDeg((float)_perspective.Heading)).ToQuat()
                * new Angles(0f, MathFunctions.RadToDeg((float)_perspective.Tilt), 0f).ToQuat()
                * Vec3.XAxis;
        }

        ToolTip tt = new ToolTip();
        MapObject mapObject = null;
        void RenderEntityOverCursor(Camera camera)
        {
            Vec2 mouse = GetFloatMousePosition();
            mapObject = null;

            if (mouse.X < 0 || mouse.X > 1 || mouse.Y < 0 || mouse.Y > 1)
            {
                tt.ShowAlways = false;
                tt.RemoveAll();
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
                    tt.SetToolTip(this, mapObject.Name);
                    tt.ShowAlways = true;
                    World.ViewModel.SetMouseOverEntity(mapObject.Name);
                }
                else
                {
                    tt.ShowAlways = false;
                    tt.RemoveAll();
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

        Random r = new Random();
        void renderTargetUserControl1_MouseClick(object sender, MouseEventArgs e)
        {
            var em = World.ViewModel.SelectedEntities.FirstOrDefault();
            if (em != null)
            {
                // TODO: set the target
            }
            if (mapObject != null)
            {
                if (IsKeyPressed(Keys.ShiftKey) || IsKeyPressed(Keys.ControlKey))
                    World.ViewModel.SelectAdd(mapObject.Name);
                else
                    World.ViewModel.Select(mapObject.Name);
                var b = mapObject as GameEntities.RTSBuilding;
                if (b != null)
                {
                    RTSUnit unit = (RTSUnit)Entities.Instance.Create(EntityTypes.Instance.GetByName("RTSRobot"), Map.Instance);

                    RTSCharacter character = unit as RTSCharacter;
                    if (character == null)
                        Log.Fatal("RTSBuilding: CreateProductedUnit: character == null");

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
                    b.AddForce(ForceType.Global, 0f, new Vec3(r.Next(500) - 250, r.Next(500) - 250, r.Next(1000) + 250), new Vec3(0, 0, 0));
                }
            }
        }
    }
}
