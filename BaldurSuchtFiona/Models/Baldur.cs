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

                    flowerValue += 1;
                };
                return flowerValue;
            }}

        public int Ores { get {
                var oreValue = 0;
                foreach (var item in Inventory)
                {
                    if (!(item is Iron))
                        continue;
                    
                    oreValue += 1;
                };
                return oreValue;
            }}

        public int Potions { get; set; }

        public int ArmorCounter { get {
                var armValue = 0;
                foreach (var item in Inventory)
                {
                    if (!(item is Armor))
                        continue;

                    armValue += 1;

                };
                return armValue;
            }}

        public int WeaponCounter { get {
                var wepValue = 0;
                foreach (var item in Inventory)
                {
                    if (!(item is Weapon))
                        continue;

                    wepValue += 1;

                };
                return wepValue;
            }}

        public int KeycardCounter { get {
                var cardValue = 0;
                foreach (var item in Inventory)
                {
                    if (!(item is Keycard))
                        continue;

                    cardValue += 1;

                }
                return cardValue;
            }}

        public Baldur () : base()
		{
            Radius = 0.25f;
            Potions = 0;
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

