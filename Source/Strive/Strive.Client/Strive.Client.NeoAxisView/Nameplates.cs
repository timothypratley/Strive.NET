using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

namespace Strive.Client.NeoAxisView
{
    class Nameplates
    {
        static List<MapObject> _tempObjects = new List<MapObject>(128);
        static bool showObjectsTips = true;
        static float visibleDistance = 200;

        public static void RenderObjectsTips(GuiRenderer renderer, Camera camera)
        {
            if (!showObjectsTips)
                return;

            Vec3 cameraPosition = camera.Position;

            float aspectRatioInv = 1.0f / camera.AspectRatio;

            //generate visibled objects list 
            //var forty = EntityTypes.Instance.GetByName("Forty");
            Map.Instance.GetObjectsByScreenRectangle(camera, new Rect(0, 0, 1, 1), delegate(MapObject obj)
            {
                if (!obj.Visible)
                    return;
                if (!obj.EditorSelectable)
                    return;
                //if (obj.Type == forty)
                    //return;

                float lengthSqr = (obj.Position - cameraPosition).LengthSqr();
                if (lengthSqr > visibleDistance * visibleDistance)
                    return;

                _tempObjects.Add(obj);
            });

            //sort objects by distance 
            //_tempObjects.Sort( delegate( MapObject obj1, MapObject obj2 ) 
            ListUtils.SelectionSort(_tempObjects, delegate(MapObject obj1, MapObject obj2)
            {
                float distanceSqr1 = (obj1.Position - cameraPosition).LengthSqr();
                float distanceSqr2 = (obj2.Position - cameraPosition).LengthSqr();

                if (distanceSqr1 < distanceSqr2)
                    return 1;
                if (distanceSqr1 > distanceSqr2)
                    return -1;
                return 0;
            });

            //render objects 
            foreach (MapObject obj in _tempObjects)
            {
                Vec2 screenPos;
                camera.ProjectToScreenCoordinates(obj.Position + new Vec3(0, 0, obj.MapBounds.GetSize().Z + 0.2f), out screenPos);
                if (!new Rect(.001f, .001f, .999f, .999f).IsContainsPoint(screenPos))
                    continue;

                float length = (obj.Position - cameraPosition).LengthFast();

                float alpha = 1;
                if (length > visibleDistance * .9f)
                    alpha = (visibleDistance - length) / (visibleDistance * .1f);

                float textLength = 0;

                    ColorValue textColor = new ColorValue(0.8f, 0.8f, 1, 1);
                    textColor.Alpha *= alpha;

                    Engine.Renderer.Font font = FontManager.Instance.LoadFont("Default", 0.02f);
                    if (font == null)
                        font = renderer.DefaultFont;

                    string text = obj.Name.ToString();
                    if (text.Length != 0)
                    {
                        textLength = font.GetTextLength(renderer, text) / renderer.AspectRatio;

                        if (screenPos.X < textLength * .5f)
                        {
                            float visibleArea = 1.0f - (textLength * .5f - screenPos.X) /
                               (textLength * .5f);
                            MathFunctions.Clamp(ref visibleArea, 0, 1);
                            textColor.Alpha *= visibleArea;
                            alpha *= visibleArea;
                        }
                        if (screenPos.X > 1.0f - textLength * .5f)
                        {
                            float visibleArea = 1.0f - (screenPos.X - (1.0f - textLength * .5f)) /
                               (textLength * .5f);
                            MathFunctions.Clamp(ref visibleArea, 0, 1);
                            textColor.Alpha *= visibleArea;
                            alpha *= visibleArea;
                        }

                        renderer.AddText(font, text, screenPos, HorizontalAlign.Center,
                           VerticalAlign.Center, textColor);
                    }

                /*** icon rendering
                if (!string.IsNullOrEmpty(tipItem.Icon))
                {
                    Vec2 size = new Vec2(tipItem.IconSize * aspectRatioInv, tipItem.IconSize);
                    Rect rectangle = new Rect(screenPos - size * .5f, screenPos + size * .5f);

                    if (textLength == 0)
                    {
                        if (rectangle.Right < size.X)
                        {
                            float v = (rectangle.Right - size.X * .5f) / (size.X * .5f);
                            MathFunctions.Clamp(ref v, 0, 1);
                            alpha *= v;
                        }
                        if (rectangle.Left > 1.0 - size.X)
                        {
                            float v = ((1.0f - rectangle.Left) - size.X * .5f) / (size.X * .5f);
                            MathFunctions.Clamp(ref v, 0, 1);
                            alpha *= v;
                        }
                        if (rectangle.Top < size.Y)
                        {
                            float v = (rectangle.Top - size.Y * .5f) / (size.Y * .5f);
                            MathFunctions.Clamp(ref v, 0, 1);
                            alpha *= v;
                        }
                        if (rectangle.Bottom > 1.0 - size.Y)
                        {
                            float v = ((1.0f - rectangle.Bottom) - size.Y * .5f) / (size.Y * .5f);
                            MathFunctions.Clamp(ref v, 0, 1);
                            alpha *= v;
                        }
                    }

                    if (textLength != 0)
                        rectangle -= new Vec2(textLength * .5f + size.X * .5f, 0);

                    Texture texture = TextureManager.Instance.Load(tipItem.Icon);

                    ColorValue color = tipItem.IconColor;
                    color *= alpha;

                    if (texture != null)
                        renderer.AddQuad(rectangle, new Rect(0, 0, 1, 1), texture, color, true);
                }
                 */

            }

            _tempObjects.Clear();
        } 
    }
}
