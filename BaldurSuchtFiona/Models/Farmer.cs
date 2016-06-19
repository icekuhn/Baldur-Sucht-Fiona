using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BaldurSuchtFiona.Models
{
	public class Farmer : Enemy
    {
        public List<Flower> Flowers { get; set; }

        public Farmer () : base()
        {
            AttackTexture = "attack2.png";
            AttackTextureName = "attack2.png";
            Radius = 0.25f;
            Texture = "sprite_farmer.png";
            TextureName = "sprite_farmer.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.25f;
            AttackValue = 10;
            Flowers = new List<Flower>();
            Defense = 0;
        }

        public Farmer (Game1 game,Vector2 position) : this()
        {
            Position = position;
            InitializeData (game);
        }

        public Farmer (Game1 game,Vector2 position,List<Flower> flowers) : this()
        {
            Position = position;
            Flowers = flowers;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            if(Flowers.Any())
                Ai = new WalkingAi(this, MaxSpeed,Flowers.First().Position,AttackRadius-1);
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

