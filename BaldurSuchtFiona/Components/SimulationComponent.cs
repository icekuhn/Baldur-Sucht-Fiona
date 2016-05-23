using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Components
{
	internal class SimulationComponent : GameComponent
	{
		private Game1 game;

		public Vector2 Position {
			get;
			private set;
		}

		public Vector2 velocity {
			get;
			private set;
		}
			

		public SimulationComponent (Game1 game) : base(game)
		{
			this.game = game;
		}

		public void NewGame() {
			Position = new Vector2 (0, 0);
		}

		public override void Update (GameTime gameTime)
		{
			if (!game.Input.Handled) 
			{
				velocity = game.Input.Movement;
			}
			else 
			{
				velocity = Vector2.Zero;
			}

			Position += velocity;
		}
	}
}

