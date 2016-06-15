using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
    public class FarmLeader : Boss
	{
        private Keycard keycardToDrop;
        private Weapon weaponToDrop;
        private bool cardHasDropped;
        private bool wepHasDropped;
        public FarmLeader () : base()
        {
            Radius = 0.25f;
            Texture = "sprite_farmer.png";
            TextureName = "sprite_farmer.png";
            IsPeaceMode = true;
            MaxSpeed = 0.8f;
            AttackRange = 0.5f;
            AttackValue = 20;
        }

        public FarmLeader (Game1 game,Vector2 position) : base()
        {
            Radius = 0.25f;
            Position = position;
            InitializeData (game);
            Texture = "sprite_farmer.png";
            TextureName = "sprite_farmer.png";
            IsPeaceMode = true;
            MaxSpeed = 0.8f;
            AttackRange = 0.5f;
            AttackValue = 20;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            Ai = new WalkingAi(this, MaxSpeed);

            keycardToDrop = new Keycard(game,4);
            weaponToDrop = new Weapon(game, 2);
            Name = "Farm Leader";
        }

        public void GetAggressive(){
            Ai = new AggressiveAi(this, AttackRadius);
            IsPeaceMode = false;
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }

        public override void OnHit(Game1 game,Character attacker,List<Action> transfers){
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

