using System;

namespace BaldurSuchtFiona.Interfaces
{
	public interface IAttackable
	{
		int MaxHitpoints { get; set; }
		int CurrentHitpoints { get; set; }
		int Defense { get; set; }
		TimeSpan TotalRestoration { get; }
		TimeSpan Restoration { get; set; }
	}
}

