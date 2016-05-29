using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BaldurSuchtFiona.Components
{
	public class HudComponent : DrawableGameComponent
	{
		private Game1 game;

		private SpriteBatch spriteBatch;

		private SpriteFont gameFont;

        private Texture2D HudIcons;

		public HudComponent(Game1 game) : base(game)
		{
			this.game = game;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
			gameFont = game.Content.Load<SpriteFont> ("GameFont");

            string mapPath = Path.Combine(Environment.CurrentDirectory, "Content");
            using (Stream stream = File.OpenRead(mapPath + "\\hudIcons.png"))
            {
                HudIcons = Texture2D.FromStream(GraphicsDevice, stream);
            }
		}

		public override void Draw(GameTime gameTime)
		{
            int maxHitPoints = game.Simulation.Baldur.MaxHitpoints;
            int currentHitPoints = game.Simulation.Baldur.CurrentHitpoints;
            int flowers = game.Simulation.Baldur.Flowers;
            int ores = game.Simulation.Baldur.Ores;
            int potions = game.Simulation.Baldur.Potions;
            int weapons = game.Simulation.Baldur.Weapons;
            int keycards = game.Simulation.Baldur.Keycards;
            int amors = game.Simulation.Baldur.Defense;

            spriteBatch.Begin();
            spriteBatch.DrawString (gameFont, String.Format ("Spielzeit: {0}", gameTime.TotalGameTime.ToString()), new Vector2 (12, 12), Color.White);
            spriteBatch.Draw(HudIcons, new Vector2(GraphicsDevice.Viewport.Width - 92f, 4), Color.White);
            spriteBatch.DrawString (gameFont, String.Format ("{0}/{1}", currentHitPoints.ToString(), maxHitPoints.ToString()), new Vector2(GraphicsDevice.Viewport.Width - 58f, 12), Color.White);
            spriteBatch.DrawString (gameFont, ores.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 58f, 44), Color.White);
            spriteBatch.DrawString (gameFont, flowers.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 58f, 76), Color.White);
            spriteBatch.DrawString (gameFont, potions.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 58f, 108), Color.White);

            spriteBatch.End();


		}
	}
}

