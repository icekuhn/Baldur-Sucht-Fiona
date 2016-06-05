using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
    public class Character: Objekt
    {
        public float MaxSpeed { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 DefaultPosition { get; set; }

        public Character(): base()
        {
            MaxSpeed = 3f;
            Radius = 0.4f;
        }
    }
}

