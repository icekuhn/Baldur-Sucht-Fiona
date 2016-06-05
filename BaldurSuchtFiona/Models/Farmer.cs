using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
	public class Farmer : Enemy
    {

        public Farmer () : base()
        {
            Radius = 0.25f;
            Texture = "sprite_farmer.png";
            TextureName = "sprite_farmer.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.25f;
        }

        public Farmer (Game1 game,Vector2 position) : base()
        {
            Radius = 0.25f;
            Position = position;
            InitializeData (game);
            Texture = "sprite_farmer.png";
            TextureName = "sprite_farmer.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.25f;
        }

        public void InitializeData (Game1 game){
            Ai = new WalkingAi(this, MaxSpeed);
            Name = "Miner";
            //  LoadTexture(game,"Character_Armor_front");
        }

        public void GetAggressive(){
            Ai = new AggressiveAi(this, AttackRadius);
            IsPeaceMode = false;
        }
	}
}

