using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Rendering
{
    internal class Camera
    {
        public int Border { get; set; }

        private Vector2 viewSizeHalf;

        public float Scale { get; private set; }

        public Vector2 Position { get; private set; }

        public Vector2 Offset { get { return Position - ViewSizeHalf; } }

        public Vector2 ViewSizeHalf { get { return viewSizeHalf / Scale;} }
            
        public Camera(Point viewSize)
        {
            viewSizeHalf = new Vector2(viewSize.X / 2f, viewSize.Y / 2f);        
            Scale = 64f;
            Border = 200;
        }

        public void SetFocus(Vector2 position, Vector2 areaSize)
        {
            Vector2 viewSize = ViewSizeHalf * 2f;
            float worldBorder = Border / Scale;

            if (areaSize.X > viewSize.X)
            {
                float left = position.X - Offset.X - worldBorder;
                if (left < 0f)
                    Position = new Vector2(Position.X + left, Position.Y);

                float right = viewSize.X - position.X + Offset.X - worldBorder;
                if (right < 0f)
                    Position = new Vector2(Position.X - right, Position.Y);

                left = Offset.X;
                if (left < 0f)
                    Position = new Vector2(Position.X - left, Position.Y);

                right = areaSize.X - Offset.X - viewSize.X;
                if (right < 0f)
                    Position = new Vector2(Position.X + right, Position.Y);
            }
            else
            {
                Position = new Vector2(areaSize.X / 2f, Position.Y);
            }


            if (areaSize.Y > viewSize.Y)
            {
                float top = position.Y - Offset.Y - worldBorder;
                if (top < 0f)
                    Position = new Vector2(Position.X, Position.Y + top);

                float bottom = viewSize.Y - position.Y + Offset.Y - worldBorder;
                if (bottom < 0f)
                    Position = new Vector2(Position.X, Position.Y - bottom);

                top = Offset.Y;
                if (top < 0f)
                    Position = new Vector2(Position.X, Position.Y - top);

                bottom = areaSize.Y - Offset.Y - viewSize.Y;
                if (bottom < 0f)
                    Position = new Vector2(Position.X, Position.Y + bottom);
            }
            else
            {
                Position = new Vector2(areaSize.X, Position.Y / 2f);
            }

        }
    }
}

