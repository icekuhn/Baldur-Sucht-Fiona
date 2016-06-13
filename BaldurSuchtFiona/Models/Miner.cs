using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
	public class Miner : Enemy
	{
        public Miner () : base()
        {
            Radius = 0.25f;
            Texture = "sprite_miner.png";
            TextureName = "sprite_miner.png";
            IsPeaceMode = true;
            MaxSpeed = 0.4f;
            AttackRange = 0.3f;
            AttackValue = 15;
        }

        public Miner (Game1 game,Vector2 position) : base()
        {
            Radius = 0.25f;
            Position = position;
            InitializeData (game);
            Texture = "sprite_miner.png";
            TextureName = "sprite_miner.png";
            IsPeaceMode = true;
            MaxSpeed = 0.4f;
            AttackRange = 0.3f;
            AttackValue = 15;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            Ai = new WalkingAi(this, MaxSpeed);

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
            }
        }
	}
}

