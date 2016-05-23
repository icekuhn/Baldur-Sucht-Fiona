using System;

namespace BaldurSuchtFiona
{
	public class Healpot : Item
	{
		public int HealthRestoration { get;set;}

		public Healpot ()
		{
			HealthRestoration = 10;
		}
	}
}

