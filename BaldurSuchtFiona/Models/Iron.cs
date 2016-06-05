using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona.Models
{
    internal class Iron : Item,ICollectable
    {
        public int Value { get; set; }

        public Iron(Game1 game)
        {
            Value = 1;
            InitializeData(game);
        }

        public Iron (Game1 game,Vector2 position) : base()
        {
            Position = position;
            Value = 1;
            InitializeData(game);
        }

        public Iron(Game1 game,int value)
        {
            Value = value;
            InitializeData(game);
        }

        public Iron(Game1 game,int value,Vector2 position)
        {
            Position = position;
            Value = value;
            InitializeData(game);
        }

        public void InitializeData(Game1 game){
            Name = "Iron";
            LoadTexture(game,"collectables.png",(Value - 1 ) * 32,0,32,32);
        }

        public override void OnCollect(World world){

        }
    }
}

