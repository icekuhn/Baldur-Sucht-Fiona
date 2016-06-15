using System;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona
{
    public class Weapon : Item,ICollectable
    {
        public int Value { get; set; }

        public Weapon(Game1 game)
        {
            Value = 1;
            InitializeData(game);
        }

        public Weapon (Game1 game,Vector2 position) : base()
        {
            Position = position;
            Value = 1;
            InitializeData(game);
        }

        public Weapon(Game1 game,int value)
        {
            Value = value;
            InitializeData(game);
        }

        public Weapon(Game1 game,int value,Vector2 position)
        {
            Position = position;
            Value = value;
            InitializeData(game);
        }

        public void InitializeData(Game1 game){
            Name = "Weapon";
            LoadTexture(game,"collectables",(Value - 1 ) * 32,96,32,32);
        }

        public override void OnCollect(World world){

        }
    }
}