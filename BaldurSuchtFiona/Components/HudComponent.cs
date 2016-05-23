using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaldurSuchtFiona.Components
{
	internal class HudComponent : DrawableGameComponent
	{
		private Game1 game;

		private SpriteBatch spriteBatch;

		private SpriteFont gameFont;

		public HudComponent(Game1 game) : base(game)
		{
			this.game = game;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
			gameFont = game.Content.Load<SpriteFont> ("GameFont");
		}

		public override void Draw(GameTime gameTime)
		{
				spriteBatch.Begin();
			spriteBatch.DrawString (gameFont, String.Format ("Testversion {0}",System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()), new Vector2 (20, 20), Color.Black); 
				spriteBatch.End();
		}
	}
}

