using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona
{
    public class Iron : Item,ICollectable
    {
        public int Value { get; set; }

        public Iron()
        {
            Value = 1;
            Name = "Iron";
        }

        public Iron (Vector2 position) : base()
        {
            Position = position;
            Value = 1;
            Name = "Iron";
        }

        public Iron(int value)
        {
            Value = value;
            Name = "Iron";
        }

        public Iron(int value,Vector2 position)
        {
            Position = position;
            Value = value;
            Name = "Iron";
        }
    }
}

