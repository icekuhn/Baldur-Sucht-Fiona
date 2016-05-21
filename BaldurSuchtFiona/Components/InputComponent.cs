using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BaldurSuchtFiona
{
	internal class InputComponent : GameComponent
	{
		private Game1 game;

		public Vector2 Bewegung {
			get;
			private set;
		}

		public InputComponent (Game1 game) : base(game)
		{
			this.game = game;
		}
			
		public override void Update (GameTime gameTime)
		{
			Vector2 bewegung = Vector2.Zero;

			KeyboardState keyboard = Keyboard.GetState();
			if (keyboard.IsKeyDown(Keys.Escape))
				game.Exit();
			if (keyboard.IsKeyDown(Keys.Left))
				bewegung += new Vector2(-1f, 0f);
			if (keyboard.IsKeyDown(Keys.Right))
				bewegung += new Vector2(1f, 0f);
			if (keyboard.IsKeyDown(Keys.Up))
				bewegung += new Vector2(0f, -1f);
			if (keyboard.IsKeyDown(Keys.Down))
				bewegung += new Vector2(0f, 1f);

			Bewegung = bewegung;

			base.Update (gameTime);
		}	
	}
}

