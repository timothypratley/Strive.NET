﻿using System;
using System.Collections.Generic;
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

using Strive.Client.ViewModel;


namespace Strive.Client.NeoAxisView
{
    public partial class WorldViewControl : RenderTargetUserControl
    {
        Perspective _perspective;
        public WorldViewControl()
        {
            _perspective = new Perspective(
                new Perspective.KeyPressedCheck(IsKeyPressed),
                new Perspective.MouseButtonCheck(GetMouseButtons),
                World.ViewModel.bindings);
            CameraFixedUp = Vec3.ZAxis;
            CameraPosition = new Vec3(0, 10, 1);
            CameraDirection = new Vec3(0, 1, 0);
            AutomaticUpdateFPS = 60;
            Render += renderTargetUserControl1_Render;
            RenderUI += new RenderUIDelegate(WorldViewControl_RenderUI);
            MouseClick += new MouseEventHandler(renderTargetUserControl1_MouseClick);
            MouseEnter += new EventHandler(WorldViewControl_MouseEnter);
        }

        public MouseButtons GetMouseButtons()
        {
            return MouseButtons;
        }

        void WorldViewControl_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        Camera camera = null;
        void WorldViewControl_RenderUI(GuiRenderer renderer)
        {
            if (camera != null)
            {
                Nameplates.RenderObjectsTips(renderer, camera);
            }
            string text = "FPS: " + _perspective.FPS
                        + "    loc: " + CameraPosition.ToString(0)
                        + "    dir: " + CameraDirection.ToString(0);

            renderer.AddText(text, new Vec2(.01f, .01f), HorizontalAlign.Left,
                VerticalAlign.Top, new ColorValue(1, 1, 1));
        }

        void renderTargetUserControl1_Render(Camera camera)
        {
            this.camera = camera;
            RenderEntityOverCursor(camera);
            _perspective.Check();
            CameraPosition = new Vec3((float)_perspective.PositionX, (float)_perspective.PositionY, (float)_perspective.PositionZ);
            CameraDirection = new Angles(0f, 0f, MathFunctions.RadToDeg((float)_perspective.Heading)).ToQuat()
                * new Angles(0f, MathFunctions.RadToDeg((float)_perspective.Tilt), 0f).ToQuat()
                * Vec3.XAxis;
        }

        ToolTip tt = new ToolTip();
        MapObject mapObject = null;
        void RenderEntityOverCursor(Camera camera)
        {
            Vec2 mouse = GetFloatMousePosition();

            if (mouse.X < 0 || mouse.X > 1 || mouse.Y < 0 || mouse.Y > 1)
            {
                tt.ShowAlways = false;
                tt.RemoveAll();
                return;
            }

            Ray ray = camera.GetCameraToViewportRay(mouse);

            mapObject = null;
            Map.Instance.GetObjects(ray, delegate(MapObject obj, float scale)
            {
                if (obj is StaticMesh)
                    return true;
                mapObject = obj;
                return false;
            });

            if (mapObject != null)
            {
                camera.DebugGeometry.Color = new ColorValue(1, 1, 0);
                camera.DebugGeometry.AddBounds(mapObject.MapBounds);
                tt.SetToolTip(this, mapObject.Name);
                tt.ShowAlways = true;
            }
            else
            {
                tt.ShowAlways = false;
                tt.RemoveAll();
            }
        }

        public void SetCamera(double x, double y, double z, double dirX, double dirY, double dirZ)
        {
            CameraPosition = new Vec3((float)x, (float)y, (float)z);
            CameraDirection = new Vec3((float)dirX, (float)dirY, (float)dirZ);
        }

        Random r = new Random();
        void renderTargetUserControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (mapObject != null)
            {
                var b = mapObject as GameEntities.RTSBuilding;
                if (b != null)
                {
                    RTSUnit unit = (RTSUnit)Entities.Instance.Create(EntityTypes.Instance.GetByName( "RTSRobot" ), Map.Instance);

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