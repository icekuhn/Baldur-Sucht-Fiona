using System;
using System.Collections.Generic;

namespace BaldurSuchtFiona.Interfaces
{
	public interface IAttacker
	{
        List<IAttackable> AttackableItems { get; }
        float AttackRange { get; }
        float AttackRadius { get; }
		int AttackValue { get; }
		TimeSpan TotalRecovery { get; }
		TimeSpan Recovery { get; set; }
		bool IsAttacking { get; set; }
        bool IsPeaceMode{ get; set; }
	}
}

