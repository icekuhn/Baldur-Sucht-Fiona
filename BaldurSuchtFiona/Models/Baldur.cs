using System;
using BaldurSuchtFiona;
using BaldurSuchtFiona.Models;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
	public class Baldur : Player
	{
        public int Flowers { get; set; }

        public int Ores { get; set; }

        public int Potions { get; set; }

        public int Weapons { get; set; }

        public int Keycards { get; set; }

        public Baldur () : base()
		{
            Flowers = 0;
            Ores = 0;
            Potions = 0;
            Weapons = 1;
            Keycards = 1;
		}

        public Baldur (Game1 game,Vector2 position) : base()
        {
            Position = position;
            InitializeData (game);
            Flowers = 0;
            Ores = 0;
            Potions = 0;
            Weapons = 1;
            Keycards = 1;
        }

        public void InitializeData (Game1 game){
            Name = "Baldur";
            LoadTexture(game,"Character_Armor_front");
        }
	}
}

