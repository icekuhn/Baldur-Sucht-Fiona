using System;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona.Models
{
    internal class Flower : Item
    {
        public int Value { get; set; }

        public Flower(Game1 game)
        {
            Value = 1;
            InitializeData(game);
        }

        public Flower (Game1 game,Vector2 position) : base()
        {
            Position = position;
            Value = 1;
            InitializeData(game);
        }

        public Flower(Game1 game,int value)
        {
            Value = value;
            InitializeData(game);
        }

        public Flower(Game1 game,int value,Vector2 position)
        {
            Position = position;
            Value = value;
            InitializeData(game);
        }

        public void InitializeData(Game1 game){
            Name = "Iron";
            LoadTexture(game,"collectables",(Value - 1 ) * 32,32,32,32);
        }

        public override void OnCollect(World world){
            foreach (var farmer in world.Area.Objects.OfType<Farmer>())
            {
                farmer.GetAggressive();
            }
        }
    }
}

