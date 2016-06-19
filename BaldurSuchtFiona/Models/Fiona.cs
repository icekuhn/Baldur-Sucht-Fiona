using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
	public class Fiona : Character
    {
        public Ai Ai { get; set; } 
        public float MaxSpeed { get; set; }

        public Fiona () : base()
        {
            MaxSpeed = 0.6f;
            Radius = 0.4f;
            Texture = "sprite_fiona.png";
            TextureName = "sprite_fiona.png";
        }

        public Fiona (Game1 game,Vector2 position) : this()
        {
            Position = position;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            Ai = new FollowingAi(this, MaxSpeed,game.Baldur,3f); 

            Name = "Fiona";
            //  LoadTexture(game,"Character_Armor_front");
        }
	}
}

