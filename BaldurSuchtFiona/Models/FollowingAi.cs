using System;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
    public class FollowingAi : Ai
    {
        protected Character Target { get; private set; }
        private bool isFollowing;
        private float range;

        private Vector2? center;

        private Vector2? movingCenter;

        private float movingRadius;

        private TimeSpan delay;

        protected Random Random { get; private set; }

        public FollowingAi(Character host, float range) : base(host) 
        {
            this.range = range;
            Random = new Random();
            delay = TimeSpan.Zero;
        }

        public FollowingAi(Character host, float range,Character target,float mRadius) : base(host) 
        {
            this.range = range;
            Random = new Random();
            delay = TimeSpan.Zero;
            Target = target;
            movingRadius = mRadius;
        }        



        public override void SetCenter(Vector2? center){
        }

        public override void OnUpdate(Area area, GameTime gameTime)
        {
            
            if (Target != null)
                center = Target.Position;
            else
                center = Host.Position;
            var distance = (Host.Position - Target.Position).Length ();

            if (!isFollowing && distance <= 2f)
                isFollowing = true;


            if (!Walking && isFollowing)
            {
                if (delay > TimeSpan.Zero)
                {
                    delay -= gameTime.ElapsedGameTime;
                    return;
                }

                Vector2 variation = new Vector2(
                    (float)(Random.NextDouble() * 1), 
                    (float)(Random.NextDouble() * 1));
                var moveDestionation = center.Value + variation;

                float movingDistance;
                do
                {
                    var distanceDestionationFromCenterX = moveDestionation.X - center.Value.X;
                    if (distanceDestionationFromCenterX < 0)
                        distanceDestionationFromCenterX = distanceDestionationFromCenterX * -1;
                    var distanceDestionationFromCenterY = moveDestionation.Y - center.Value.Y;
                    if (distanceDestionationFromCenterY < 0)
                        distanceDestionationFromCenterY = distanceDestionationFromCenterY * -1;
                    var distanceDestionationFromCenter = new Vector2();
                    distanceDestionationFromCenter.X = distanceDestionationFromCenterX;
                    distanceDestionationFromCenter.Y = distanceDestionationFromCenterY;

                    movingDistance = distanceDestionationFromCenter.Length();
                } while (movingDistance > movingRadius && movingRadius != 0);

                WalkTo(moveDestionation, 0.4f);

                var delayValue = (Random.Next(200) + 1) / 100 + 0.5;
                delay = TimeSpan.FromSeconds(delayValue);
            }
        }
    }
}