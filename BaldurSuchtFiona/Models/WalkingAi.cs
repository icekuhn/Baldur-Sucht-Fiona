using System;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
    public class WalkingAi : Ai
    {
        private float range;

        private Vector2? center;

        private Vector2? movingCenter;

        private float movingRadius;

        private TimeSpan delay;

        protected Random Random { get; private set; }

        public WalkingAi(Character host, float range) : base(host) 
        {
            this.range = range;
            Random = new Random();
            delay = TimeSpan.Zero;
        }

        public WalkingAi(Character host, float range,Vector2 mCenter,float mRadius) : base(host) 
        {
            this.range = range;
            Random = new Random();
            delay = TimeSpan.Zero;
            movingCenter = mCenter;
            movingRadius = mRadius;
        }

        public override void OnUpdate(Area area, GameTime gameTime)
        {
            if (!center.HasValue)
            {
                if (movingCenter.HasValue)
                    center = movingCenter.Value;
                else
                    center = Host.Position;
                
            }

            if (!Walking)
            {
                if (delay > TimeSpan.Zero)
                {
                    delay -= gameTime.ElapsedGameTime;
                    return;
                }

                Vector2 variation = new Vector2(
                    (float)(Random.NextDouble() * 2), 
                    (float)(Random.NextDouble() * 2));
                var moveDestionation = center.Value + variation * 2 * range;

                float movingDistance;
                do
                {
                    var distanceDestionationFromCenterX = moveDestionation.X - movingCenter.Value.X;
                    if (distanceDestionationFromCenterX < 0)
                        distanceDestionationFromCenterX = distanceDestionationFromCenterX * -1;
                    var distanceDestionationFromCenterY = moveDestionation.Y - movingCenter.Value.Y;
                    if (distanceDestionationFromCenterY < 0)
                        distanceDestionationFromCenterY = distanceDestionationFromCenterY * -1;
                    var distanceDestionationFromCenter = new Vector2();
                    distanceDestionationFromCenter.X = distanceDestionationFromCenterX;
                    distanceDestionationFromCenter.Y = distanceDestionationFromCenterY;

                    movingDistance = distanceDestionationFromCenter.Length();
                } while (movingDistance > movingRadius);

                variation = new Vector2(
                    (float)(Random.NextDouble() * 2), 
                    (float)(Random.NextDouble() * 2));
                moveDestionation = center.Value + variation * 2 * range;

                WalkTo(moveDestionation, 0.4f);

                var delayValue = (Random.Next(200) + 1) / 100 + 0.5;
                delay = TimeSpan.FromSeconds(delayValue);
            }
        }
    }
}