using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BaldurSuchtFiona.Models;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona.Models
{
	public abstract class Player : Character,ICollector,IAttackable,IAttacker
	{
		//IAttacker
        public List<IAttackable> AttackableItems { get; set; }
        public float AttackRange { get; set; }
        public float AttackRadius { get; set; }
		public int AttackValue { get; set; }
		public TimeSpan TotalRecovery { get; set; }
		public TimeSpan Recovery { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsPeaceMode{ get; set; }
		//IAttackable
		public int MaxHitpoints{ get; set; }
		public int CurrentHitpoints{ get; set; }
		public int Defense{ get; set; }
		public TimeSpan TotalRestoration { get; set; }
		public TimeSpan Restoration { get; set; }
		//ICollector
		public List<Item> Inventory { get; set; }


		public Player () : base()
		{
			AttackRange = 1f;
			AttackValue = 15;
			TotalRecovery = new TimeSpan (0, 0, 0, 0, 800);
			Recovery = new TimeSpan (0, 0, 0, 0, 0);
			MaxHitpoints = 100;
			CurrentHitpoints = 100;
			Defense = 0;
			TotalRecovery = new TimeSpan (0, 0, 0, 5);
			Recovery = new TimeSpan (0, 0, 0, 0);
            Inventory = new List<Item>();
            AttackableItems = new List<IAttackable>();
		}

        public void OnHit(Game1 game,Character attacker,List<Action> transfers){

        }
	}
}

