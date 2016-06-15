using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona.Models
{
	public abstract class Enemy : Character,IAttackable,IAttacker
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
        public string AttackTexture{ get; set; }
        public string AttackTextureName{ get; set; }
		//IAttackable
		public int MaxHitpoints{ get; set; }
		public int CurrentHitpoints{ get; set; }
		public int Defense{ get; set; }
		public TimeSpan TotalRestoration { get; set; }
		public TimeSpan Restoration { get; set; }

        public abstract void CheckCollectableInteraction(Objekt Item);

		public Enemy () : base()
        {
            AttackRange = 1f;
            AttackRadius = 5f;
			AttackValue = 5;
			TotalRecovery = new TimeSpan (0, 0, 0, 0, 800);
			Recovery = new TimeSpan (0, 0, 0, 0, 0);
			MaxHitpoints = 100;
			CurrentHitpoints = 100;
			Defense = 0;
            AttackableItems = new List<IAttackable>();
        }

        public abstract void OnHit(Game1 game, Character attacker,List<Action> transfers);
	}
}