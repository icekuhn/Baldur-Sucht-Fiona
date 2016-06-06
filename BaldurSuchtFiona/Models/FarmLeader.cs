using System;

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
	}
}

