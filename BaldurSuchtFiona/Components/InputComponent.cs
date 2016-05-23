using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BaldurSuchtFiona.Components
{
	internal class InputComponent : GameComponent
	{
		private readonly Trigger upTrigger;
		private readonly Trigger downTrigger;
		private readonly Trigger leftTrigger;
		private readonly Trigger rightTrigger;
		private readonly Trigger closeTrigger;
		private readonly Trigger interactTrigger;
		private readonly Trigger healTrigger;
		private readonly Trigger attackTrigger;

		public bool Handled { get; set; }

		public bool Up { get { return upTrigger.Value; } }

		public bool Down { get { return downTrigger.Value; } }

		public bool Left { get { return leftTrigger.Value; } }

		public bool Right { get { return rightTrigger.Value; } }

		public Vector2 Movement  { get; private set; }

		public bool Close { get { return closeTrigger.Value; } }

		public bool Attack { get { return attackTrigger.Value; } }

		public bool Interact { get { return interactTrigger.Value; } }

		public bool Heal { get { return healTrigger.Value; } }

		private readonly Game1 game;

		public InputComponent (Game1 game) : base(game)
		{
			this.game = game;

			upTrigger = new Trigger ();
			downTrigger = new Trigger ();
			leftTrigger = new Trigger ();
			rightTrigger = new Trigger ();
			closeTrigger = new Trigger ();
			interactTrigger = new Trigger ();
			healTrigger = new Trigger ();
			attackTrigger = new Trigger ();
		}
			
		public override void Update (GameTime gameTime)
		{
			Vector2 bewegung = Vector2.Zero;
			bool up = false;
			bool down = false;
			bool left = false;
			bool right = false;
			bool close = false;
			bool interact = false;
			bool heal = false;
			bool attack = false;

			KeyboardState keyboard = Keyboard.GetState();
				if (keyboard.IsKeyDown (Keys.Left))
					bewegung += new Vector2 (-1f, 0f);
				if (keyboard.IsKeyDown (Keys.Right))
					bewegung += new Vector2 (1f, 0f);
				if (keyboard.IsKeyDown (Keys.Up))
					bewegung += new Vector2 (0f, -1f);
				if (keyboard.IsKeyDown (Keys.Down))
					bewegung += new Vector2 (0f, 1f);
				left |= keyboard.IsKeyDown (Keys.Left);
				right |= keyboard.IsKeyDown (Keys.Right);
				up |= keyboard.IsKeyDown (Keys.Up);
				down |= keyboard.IsKeyDown (Keys.Down);
				close |= keyboard.IsKeyDown (Keys.Escape);
				interact |= keyboard.IsKeyDown (Keys.Enter);
				heal |= keyboard.IsKeyDown (Keys.LeftControl);
				attack |= keyboard.IsKeyDown (Keys.Space);


			if (bewegung.Length () > 1f)		//????
				bewegung.Normalize ();

			Movement = bewegung;
			upTrigger.Value = up;
			downTrigger.Value = down;
			leftTrigger.Value = left;
			rightTrigger.Value = right;
			closeTrigger.Value = close;
			interactTrigger.Value = interact;
			healTrigger.Value = heal;
			attackTrigger.Value = attack;

			Handled = false;
		}

		private class Trigger {

			private bool lastValue = false;

			private bool triggered = false;

			public bool Value {
				get {
					bool result = triggered;
					triggered = false;
					return result;
				}
				set {
					if (lastValue != value)
						lastValue = triggered = value;
				}
			}
		}
	}
}

