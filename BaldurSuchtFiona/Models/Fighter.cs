using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BaldurSuchtFiona.Models
{
	public class Fighter : Enemy
    {
        private Game1 _game;

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
            Defense = 10;
        }
        public Fighter (Game1 game, Vector2 position) : this()
        {
            _game = game;
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
            if (_game.World.Area.Objects.OfType<Fighter> ().Where (f=>f.CurrentHitpoints >= 0).Count () == 1) {
                if (_game.Baldur.KeycardCounter < 6) //todo: keycard zeigen, wenn alle tot sind
                {
                    transfers.Add(() =>
                        {
                            var keycard = new Keycard (_game, 5, this.Position);
                            _game.World.Area.Objects.Add (keycard);
                        });
                }
            }
        }
	}
}

