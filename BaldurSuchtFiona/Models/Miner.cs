using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona.Models
{
	public class Miner : Enemy
    {
        private Game1 _game;
        public Miner () : base()
        {
            AttackTexture = "attack3.png";
            AttackTextureName = "attack3.png";
            Radius = 0.25f;
            Texture = "sprite_miner.png";
            TextureName = "sprite_miner.png";
            IsPeaceMode = true;
            MaxSpeed = 0.4f;
            AttackRange = 0.3f;
            AttackValue = 15;
            Defense = 5;
        }

        public Miner (Game1 game,Vector2 position) : this()
        {
            _game = game;
            Position = position;
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
                if (_game.World.Area.Objects.OfType<Miner> ().Where (f=>f.CurrentHitpoints >= 0).Count () == 1 && _game.World.Area.Name == "level3") {
                    if (_game.Baldur.KeycardCounter < 5)
                    {
                        transfers.Add(() =>
                            {
                                var mineLeader = new MineLeader(_game, new Vector2(45.5f, 26.8f));
                                _game.World.Area.Objects.Add(mineLeader);
                            });
                    }
                }
                IsAttacking = false;
            }
        }
	}
}

