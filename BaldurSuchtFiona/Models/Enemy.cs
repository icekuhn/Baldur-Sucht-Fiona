using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BaldurSuchtFiona
{
    public abstract class Enemy : Character,IAttackable,IAttacker
	{
		//IAttacker
		public List<IAttackable> AttackableItems { get; set; }
		public float AttackRange { get; set; }
		public int AttackValue { get; set; }
		public TimeSpan TotalRecovery { get; set; }
		public TimeSpan Recovery { get; set; }
		public bool IsAttacking { get; set; }
		//IAttackable
		public int MaxHitpoints{ get; set; }
		public int CurrentHitpoints{ get; set; }
		public int Defense{ get; set; }
		public TimeSpan TotalRestoration { get; set; }
		public TimeSpan Restoration { get; set; }

		public Enemy () : base()
		{
			AttackRange = 1f;
			AttackValue = 15;
			TotalRecovery = new TimeSpan (0, 0, 0, 0, 800);
			Recovery = new TimeSpan (0, 0, 0, 0, 0);
			MaxHitpoints = 100;
			CurrentHitpoints = 100;
			Defense = 0;
			TotalRecovery = new TimeSpan (0, 0, 0, 0);
			Recovery = new TimeSpan (0, 0, 0, 0);
		}
	}
}