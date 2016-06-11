using System;
using BaldurSuchtFiona;
using BaldurSuchtFiona.Models;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
	public class Baldur : Player
    {
        public bool IsDead { get; set;}
        public bool ContinueAttack { get; set; }

        public int Flowers { get {
                var flowerValue = 0;
                foreach (var item in Inventory)
                {
                    if (!(item is Flower))
                        continue;

                    flowerValue += (item as Flower).Value;
                };
                return flowerValue;
            }}

        public int Ores { get {
                var oreValue = 0;
                foreach (var item in Inventory)
                {
                    if (!(item is Iron))
                        continue;
                    
                    oreValue += (item as Iron).Value;
                };
                return oreValue;
            }}

        public int Potions { get; set; }

        public int Weapons { get; set; }

        public int Keycards { get; set; }

        public int KeycardCounter { get {
                var highValue = 0;
                foreach (var item in Inventory)
                {
                    if (!(item is Keycard))
                        continue;

                    var cardValue = (item as Keycard).Value;

                    if (highValue < cardValue)
                        highValue = cardValue;
                }
                return highValue;
            }}

        public Baldur () : base()
		{
            Radius = 0.25f;
            Potions = 0;
            Weapons = 1;
            Keycards = 1;
            Texture = "sprite_player_3.png";
            TextureName = "sprite_player_3.png";
            AttackTexture = "attack1.png";
            AttackTextureName = "attack1.png";
            MaxSpeed = 1f;
            AttackValue = 25;
		}

        public Baldur (Game1 game,Vector2 position) : base()
        {
            Radius = 0.25f;
            Position = position;
            InitializeData (game);
            Potions = 0;
            Weapons = 1;
            Keycards = 1;
            Texture = "sprite_player_3.png";
            TextureName = "sprite_player_3.png";
            AttackTexture = "attack1.png";
            AttackTextureName = "attack1.png";
            MaxSpeed = 1f;
            AttackValue = 25;
        }

        public void InitializeData (Game1 game){
            Name = "Baldur";
          //  LoadTexture(game,"Character_Armor_front");
        }
	}
}

