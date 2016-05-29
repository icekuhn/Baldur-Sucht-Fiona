using System;
using System.Collections.Generic;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona.Interfaces
{
	public interface ICollector
	{
		List<Item> Inventory { get; set; }
	}
}

