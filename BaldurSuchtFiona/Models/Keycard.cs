using System;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona
{
    public class Keycard : Item,ICollectable
    {
        public int Value { get; set; }

        public Keycard(Game1 game)
        {
            Value = 1;
            InitializeData(game);
        }

        public Keycard (Game1 game,Vector2 position) : base()
        {
            Position = position;
            Value = 1;
            InitializeData(game);
        }

        public Keycard(Game1 game,int value)
        {
            Value = value;
            InitializeData(game);
        }

        public Keycard(Game1 game,int value,Vector2 position)
        {
            Position = position;
            Value = value;
            InitializeData(game);
        }

        public void InitializeData(Game1 game){
            Name = "Keycard";
            LoadTexture(game,"collectables",(Value - 1 ) * 32,64,32,32);
        }

        public override void OnCollect(World world){

        }
    }
}

