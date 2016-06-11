using System;
using BaldurSuchtFiona.Interfaces;
using BaldurSuchtFiona.Models;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona
{
    public class AggressiveAi : Ai
    {
        private Vector2? startPoint;

        private IAttacker attacker;

        private Objekt target;

        private float range;

        private Vector2? center;

        private TimeSpan delay;

        protected Random Random { get; private set; }

        public AggressiveAi(Character host, float range) : base(host) 
        {
            this.range = range;
            attacker = (IAttacker)host;
            Random = new Random();
            delay = TimeSpan.Zero;
        }

        public override void OnUpdate(Area area, GameTime gameTime)
        {
            if (!startPoint.HasValue)
                startPoint = Host.Position;

            if (target == null)
            {
                var potentialTargets = area.Objects.
                    Where(i => (i.Position - Host.Position).LengthSquared() < range * range).
                    Where(i => i.GetType() != Host.GetType()).                                
                    OrderBy(i => (i.Position - Host.Position).LengthSquared()).               
                    OfType<IAttackable>().                                                   
                    Where(a => a.CurrentHitpoints > 0);  

                target = potentialTargets.FirstOrDefault() as Objekt;
            }

            if (target != null)
            {
                attacker.IsAttacking = true;

                if ((target.Position - Host.Position).LengthSquared() > range * range ||
                    (target as IAttackable).CurrentHitpoints <= 0)
                {
                    target = null;
                    WalkTo(startPoint.Value, 0.4f);
                }
                else
                {
                    WalkTo(target.Position, 0.6f);
                }
            }
            else
            {
                attacker.IsAttacking = false;
                if (!center.HasValue)
                    center = Host.Position;

                if (!Walking)
                {
                    if (delay > TimeSpan.Zero)
                    {
                        delay -= gameTime.ElapsedGameTime;
                        return;
                    }

                    Vector2 variation = new Vector2(
                        (float)(Random.NextDouble() * 2 - 1.0), 
                        (float)(Random.NextDouble() * 2 - 1.0));
                    WalkTo(center.Value + variation * 2 * range, 0.4f);
                    delay = TimeSpan.FromSeconds(2);
                } 
            }
        }
    }
}

