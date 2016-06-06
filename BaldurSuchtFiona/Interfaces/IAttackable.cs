using System;
using BaldurSuchtFiona.Models;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Interfaces
{
	public interface IAttackable
	{
		int MaxHitpoints { get; set; }
		int CurrentHitpoints { get; set; }
		int Defense { get; set; }
		TimeSpan TotalRestoration { get; }
		TimeSpan Restoration { get; set; }

        void OnHit(Game1 game, Character attacker,List<Action> transfers);
	}
}

