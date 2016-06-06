using System;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
	public class Healpot : Item
	{
		public int HealthRestoration { get;set;}

		public Healpot ()
		{
			HealthRestoration = 10;
        }

        public override void OnCollect(World world){
            
        }
	}
}

