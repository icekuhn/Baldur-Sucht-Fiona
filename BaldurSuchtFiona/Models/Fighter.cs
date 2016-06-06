using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
	public class Fighter : Enemy
    {
        public Fighter () : base()
        {
            AttackRange = 5f;
            AttackValue = 10;
            Defense = 4;
        }
        public Fighter (Game1 game, Vector2 position) : base()
        {
            AttackRange = 5f;
            AttackValue = 10;
            Defense = 4;
            DefaultPosition = Position = position;
            InitializeData(game);
        }

        public void InitializeData(Game1 game){
            Name = "Fighter";
            Texture = "sprite_player_3.png";
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }
	}
}

