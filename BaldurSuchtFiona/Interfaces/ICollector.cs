using System;
using System.Collections.Generic;

namespace BaldurSuchtFiona
{
	public interface ICollector
	{
		List<Item> Inventory { get; set; }
	}
}

