using System;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Models
{
	public class World
	{
		public List<Area> Areas { get; private set; }

		public World()
		{
			Areas = new List<Area>();
		}
	}
}

