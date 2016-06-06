using System;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Models
{
	public class MineLeader : Enemy
	{
		public MineLeader ()
		{
			Defense = 10;
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }

        public override void OnHit(Game1 game,Character attacker,List<Action> transfers){

        }
	}
}

