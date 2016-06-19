using System;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
    public abstract class Ai
    {
        protected Character Host { get; private set; }

        public bool Walking { get { return destination.HasValue; } }

        private Vector2? startPoint;

        private Vector2? destination;

        private float speed;

        public Ai(Character host)
        {
            Host = host;
            startPoint = null;
            destination = null;
        }

        public void Update(Area area, GameTime gameTime)
        {
            OnUpdate(area, gameTime);

            if (destination.HasValue)
            {
                Vector2 expectedDistance = destination.Value - startPoint.Value;
                Vector2 currentDistance = Host.Position - startPoint.Value;

                if (currentDistance.LengthSquared() > expectedDistance.LengthSquared())
                {
                    startPoint = null;
                    destination = null;
                    Host.Velocity = Vector2.Zero;
                }
                else
                {
                    Vector2 direction = destination.Value - Host.Position;
                    direction.Normalize();
                    Host.Velocity = (direction / 10) * Host.MaxSpeed;
                }
            }
        }

        public abstract void SetCenter (Vector2? center);

        public abstract void OnUpdate(Area area, GameTime gameTime);

        protected void WalkTo(Vector2 destination, float speed)
        {
            startPoint = Host.Position;
            this.destination = destination;
            this.speed = speed;
        }

        public void StopWalking(){
            destination = null;
        }
    }
}

