using System;
using System.Collections.Generic;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
	public interface ICollector
	{
		List<Item> Inventory { get; set; }
	}
}

