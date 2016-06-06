using System;
using BaldurSuchtFiona.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
    internal class SimpleObjektRenderer: ObjektRenderer
    {
        // Anzahl der Frames
        private int frameCount;
        private Objekt _objekt;

        public SimpleObjektRenderer(Objekt objekt, Camera camera, Texture2D texture)
            : base (objekt, camera, texture, new Point(32, 32), 70, new Point(16,26), 1f)
        {
            frameCount = 1;
            _objekt = objekt;
        }

        public override void Draw(SpriteBatch spriteBatch, Point offset, GameTime gameTime)
        {
            // Animationszeit berechnen
            AnimationTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Ermitteln aktuelles Frames
            int frame = (AnimationTime / FrameTime) % frameCount;

            // Bestimmung Spieler-Mittelpunkt (View-Koordinaten)
            int posX = (int)((Objekt.Position.X) * Camera.Scale) - offset.X;
            int posY = (int)((Objekt.Position.Y) * Camera.Scale) - offset.Y;
            int radius = (int)(Objekt.Radius * Camera.Scale);

            Vector2 scale = new Vector2(Camera.Scale / FrameSize.X, Camera.Scale / FrameSize.Y) * FrameScale;

            //Rectangle sourceRectangle = new Rectangle( frame * FrameSize.X, 0, FrameSize.X, FrameSize.Y);
            Rectangle sourceRectangle = new Rectangle( _objekt.DrawX,_objekt.DrawY, FrameSize.X, FrameSize.Y);

            Rectangle destinationRectangle = new Rectangle(
                posX - (int)(ObjektOffset.X * scale.X), posY - (int)(ObjektOffset.Y * scale.Y),
                (int)(FrameSize.X * scale.X), (int)(FrameSize.Y * scale.Y));

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}

