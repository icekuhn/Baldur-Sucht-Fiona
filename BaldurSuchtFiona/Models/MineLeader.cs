using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
    public class MineLeader : Miner,IBoss
    {
        private Keycard keycardToDrop;
        private Weapon weaponToDrop;
        private bool cardHasDropped;
        private bool wepHasDropped;

		public MineLeader ()
        {
            AttackTexture = "attack3.png";
            AttackTextureName = "attack3.png";
            Radius = 0.25f;
            Texture = "sprite_miner.png";
            TextureName = "sprite_miner.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.3f;
            AttackValue = 25;
			Defense = 20;
        }

        public MineLeader (Game1 game,Vector2 position) : this()
        {
            Position = position;
            InitializeData (game);
        }


        public void InitializeData (Game1 game){
            Ai = new WalkingAi(this, MaxSpeed);

            if (game.Baldur.WeaponCounter >= 3)
                wepHasDropped = true;
            keycardToDrop = new Keycard(game,5);
            weaponToDrop = new Weapon(game, 3);

            Name = "Miner";
        }

        public void GetAggressive(){
            Ai = new AggressiveAi(this, AttackRadius);
            IsPeaceMode = false;
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }

        public override void OnHit(Game1 game,Character attacker,List<Action> transfers){
            if (IsPeaceMode)
                GetAggressive();

            if (CurrentHitpoints <= 0)
            {
                IsAttacking = false;    
                if (!cardHasDropped)
                {
                    keycardToDrop.Position = this.Position;        
                    transfers.Add(() =>
                        {
                            game.World.Area.Objects.Add(keycardToDrop);
                        });
                    cardHasDropped = true;
                }

                if (!wepHasDropped)
                {
                    weaponToDrop.Position = this.Position;        
                    transfers.Add(() =>
                        {
                            game.World.Area.Objects.Add(weaponToDrop);
                        });
                    wepHasDropped = true;
                }
            }
        }
	}
}

