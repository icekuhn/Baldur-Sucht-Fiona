using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaldurSuchtFiona
{
	internal class SceneComponent : DrawableGameComponent
	{
		private Game1 game; 
		private SpriteBatch spriteBatch;
		private Texture2D Held;

		public SceneComponent (Game1 game) : base(game)
		{
			this.game = game;
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			Held = game.Content.Load<Texture2D>("Character_Armor_front");

			base.LoadContent ();
		}

		public override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();
			spriteBatch.Draw (Held, game.Simulation.Position, Color.White);
			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}

