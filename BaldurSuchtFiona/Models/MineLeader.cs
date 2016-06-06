using System;

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
	}
}

