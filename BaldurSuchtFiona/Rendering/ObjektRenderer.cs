using System;
using BaldurSuchtFiona.Rendering;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
    internal abstract class ObjektRenderer
    {
        // Referenz auf Object
        protected Objekt Objekt { get; private set; }

        // Referenz auf Camera
        protected Camera Camera { get; private set; }

        // Referenz auf Textur
        protected Texture2D Texture { get; private set; }

        // Referenz auf Textur
        protected Texture2D AttackTexture { get; private set; }

        // Größenangabe eines Frames in Pixel
        protected Point FrameSize { get; private set; }

        // Anzahl der Millisekunden pro Frame
        protected int FrameTime { get; private set; }

        // Item-Mittelpunkt in Pixel
        protected Point ObjektOffset { get; private set; }

        // Skalierungsfactor
        protected float FrameScale { get; private set; }

        // Vergangene Aminationszeit in Millisekunden
        protected int AnimationTime { get; set; }

        // Initialisierung
        public ObjektRenderer(Objekt objekt, Camera camera, Texture2D texture, Point frameSize, int frameTime, Point objektOffset, float frameScale){
            this.Objekt = objekt;
            this.Camera = camera;
            this.Texture = texture;
            this.FrameSize = frameSize;
            this.FrameTime = frameTime;
            this.ObjektOffset = objektOffset;
            this.FrameScale = frameScale;
        }

        public ObjektRenderer(Objekt objekt, Camera camera, Texture2D texture, Texture2D attackTexture, Point frameSize, int frameTime, Point objektOffset, float frameScale){
            this.Objekt = objekt;
            this.Camera = camera;
            this.Texture = texture;
            this.AttackTexture = attackTexture;
            this.FrameSize = frameSize;
            this.FrameTime = frameTime;
            this.ObjektOffset = objektOffset;
            this.FrameScale = frameScale;
        }

        public abstract void Draw(SpriteBatch spriteBatch, Point offset, GameTime gameTime);
    }
}

