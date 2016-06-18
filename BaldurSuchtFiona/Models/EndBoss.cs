using System;
using BaldurSuchtFiona.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BaldurSuchtFiona
{
    public class EndBoss : Enemy
    {
        public EndBoss ()
        {
            AttackTexture = "attack1.png";
            AttackTextureName = "attack1.png";
            Radius = 0.25f;
            Texture = "sprite_boss.png";
            TextureName = "sprite_boss.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.3f;
            AttackValue = 45;
            Defense = 10;
        }

        public EndBoss (Game1 game,Vector2 position) : this()
        {
            Position = position;
            InitializeData (game);
        }

        public void InitializeData (Game1 game){
            Ai = new AggressiveAi(this, AttackRadius);

            Name = "EndBoss";
        }

        public void GetAggressive(){
            Ai = new AggressiveAi(this, AttackRadius);
            IsPeaceMode = false;
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }

        public override void OnHit(Game1 game,Character attacker,List<Action> transfers){
            if (CurrentHitpoints <= 0)
            {
                IsAttacking = false;   
            }
        }
    }
}

