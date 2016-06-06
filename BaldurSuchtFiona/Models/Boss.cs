using System;

namespace BaldurSuchtFiona.Models
{
	public class Boss : Enemy
	{
		public Boss ()
		{
			Defense = 20;
        }

        public override void CheckCollectableInteraction(Objekt Item){

        }
	}
}

