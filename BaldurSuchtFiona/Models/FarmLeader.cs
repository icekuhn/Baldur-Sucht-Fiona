using System;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Models
{
	public class FarmLeader : Enemy
	{
		public FarmLeader () : base()
		{
			Defense = 8;
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }

        public override void OnHit(Game1 game,Character attacker,List<Action> transfers){

        }
	}
}

