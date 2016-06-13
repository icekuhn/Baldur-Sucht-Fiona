using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Models
{
	public class Farmer : Enemy
    {
        public Flower Flower { get; set; }

        public Farmer () : base()
        {
            Radius = 0.25f;
            Texture = "sprite_farmer.png";
            TextureName = "sprite_farmer.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.25f;
            AttackValue = 10;
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
            AttackValue = 10;
            InitializeData (game);
        }

        public Farmer (Game1 game,Vector2 position,Flower flower) : base()
        {
            Radius = 0.25f;
            Position = position;
            Texture = "sprite_farmer.png";
            TextureName = "sprite_farmer.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.25f;
            AttackValue = 10;
            Flower = flower;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            if(Flower != null)
                Ai = new WalkingAi(this, MaxSpeed,Flower.Position,AttackRadius-1);
            else
                Ai = new WalkingAi(this, MaxSpeed);
                
            Name = "Farmer";
            //  LoadTexture(game,"Character_Armor_front");
        }

        public void GetAggressive(){
            Ai = new AggressiveAi(this, AttackRadius);
            IsPeaceMode = false;
        }

        public override void CheckCollectableInteraction(Objekt Item){
            if (Ai != null)
                Ai.StopWalking();
        }

        public override void OnHit(Game1 game,Character attacker,List<Action> transfers){
            if (IsPeaceMode)
                GetAggressive();

            if (CurrentHitpoints <= 0)
            {
                IsAttacking = false;
            }
//            var dropIron = new Iron(game,1,Position);
//
//            transfers.Add(() =>
//                {
//                    game.World.Area.Objects.Remove(this);
//                    game.World.Area.Objects.Add(dropIron);
//                });
        }
	}
}

