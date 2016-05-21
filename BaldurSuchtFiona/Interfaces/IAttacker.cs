using System;
using System.Collections.Generic;

namespace BaldurSuchtFiona
{
	public interface IAttacker
	{
		List<IAttackable> AttackableItems { get; }
		float AttackRange { get; }
		int AttackValue { get; }
		TimeSpan TotalRecovery { get; }
		TimeSpan Recovery { get; set; }
		bool IsAttacking { get; set; }
	}
}

