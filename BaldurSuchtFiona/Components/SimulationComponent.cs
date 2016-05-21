using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona
{
	internal class SimulationComponent : GameComponent
	{
		private Game1 game;

		public Vector2 Position {
			get;
			private set;
		}
			

		public SimulationComponent (Game1 game) : base(game)
		{
			this.game = game;
		}

		public override void Update (GameTime gameTime)
		{
			Position += game.Input.Bewegung;

			base.Update (gameTime);
		}
	}
}

