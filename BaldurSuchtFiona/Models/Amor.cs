using System;
using Microsoft.Xna.Framework;
using BaldurSuchtFiona.Models;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona
{
    public class Armor : Item,ICollectable
    {
        public int Value { get; set; }

        public Armor(int value)
        {
            Value = value;
            Name = "Armor";
        }

        public Armor(Game1 game)
        {
            Value = 1;
            InitializeData(game);
        }

        public Armor (Game1 game,Vector2 position) : base()
        {
            Position = position;
            Value = 1;
            InitializeData(game);
        }

        public Armor(Game1 game,int value)
        {
            Value = value;
            InitializeData(game);
        }

        public Armor(Game1 game,int value,Vector2 position)
        {
            Position = position;
            Value = value;
            InitializeData(game);
        }

        public void InitializeData(Game1 game){
            Name = "Armor";
            LoadTexture(game,"collectables",(Value - 1 ) * 32,128,32,32);
        }

        public override void OnCollect(World world){

        }
    }
}