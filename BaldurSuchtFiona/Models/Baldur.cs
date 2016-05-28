using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona
{
	internal class Baldur : Player
    {
        public Baldur () : base()
        {
        }

        public Baldur (Game1 game,Vector2 position) : base()
        {
            Position = position;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            Name = "Baldur";
            LoadTexture(game,"Character_Armor_front");
		}
	}
}

