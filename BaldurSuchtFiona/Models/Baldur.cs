using System;
using BaldurSuchtFiona;
using BaldurSuchtFiona.Models;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona.Models
{
	public class Baldur : Player
    {
        private Game1 _game;
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

        public int Ores {
            get {
                var oreValue = 0;
                foreach (var item in Inventory) {
                    if (!(item is Iron))
                        continue;

                    oreValue += 1;
                };
                return oreValue;
            }}

        public int Potions {
            get {
                var potValue = 0;
                foreach (var item in Inventory) {
                    if (!(item is Healpot))
                        continue;

                    potValue += 1;
                };
                return potValue;
            }
        }

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
            Texture = "sprite_player_3.png";
            TextureName = "sprite_player_3.png";
            AttackTexture = "attack1.png";
            AttackTextureName = "attack1.png";
            MaxSpeed = 1f;
            AttackValue = 25;
            Inventory.Add(new Healpot ());
            Inventory.Add(new Healpot ());
            Inventory.Add(new Healpot ());
		}

        public Baldur (Game1 game,Vector2 position) : this()
        {
            Position = position;
            _game = game;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            Name = "Baldur";
          //  LoadTexture(game,"Character_Armor_front");
        }

        public void UseHealPotion(){
            var healpotion = Inventory.OfType<Healpot> ().FirstOrDefault ();
            if (healpotion == null)
                return;
            CurrentHitpoints += healpotion.HealthRestoration;
            if (CurrentHitpoints > MaxHitpoints)
                CurrentHitpoints = MaxHitpoints;
        }

        public void ChangeAttackTexture(){
            switch (WeaponCounter)
            {
                case 1:
                    AttackTexture = "attack1.png";
                    AttackTextureName = "attack1.png";
                    break;
                case 2:
                    AttackTexture = "attack2.png";
                    AttackTextureName = "attack2.png";
                    break;
                case 3:
                    AttackTexture = "attack3.png";
                    AttackTextureName = "attack3.png";
                    break;
            }
            _game.Scene.RemoveBaldurFromRenderer();
        }
	}
}

