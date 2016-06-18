using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Models
{
	public class Fighter : Enemy
    {
        public Fighter () : base()
        {
            AttackTexture = "attack1.png";
            AttackTextureName = "attack1.png";
            Radius = 0.25f;
            Texture = "sprite_guard.png";
            TextureName = "sprite_guard.png";
            IsPeaceMode = true;
            MaxSpeed = 0.5f;
            AttackRange = 0.25f;
            AttackValue = 20;
        }
        public Fighter (Game1 game, Vector2 position) : this()
        {
            DefaultPosition = Position = position;
            InitializeData(game);
        }

        //todo attack4.png als Schlaganimation zuweisen

        public void InitializeData(Game1 game){
            Ai = new AggressiveAi(this, AttackRadius);
            Name = "Fighter";
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }

        public override void OnHit(Game1 game,Character attacker,List<Action> transfers){

        }
	}
}

