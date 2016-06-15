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

        private Texture2D CollectableIcons;

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
            using (Stream stream = File.OpenRead(mapPath + "\\collectables.png"))
            {
                CollectableIcons = Texture2D.FromStream(GraphicsDevice, stream);
            }
		}

		public override void Draw(GameTime gameTime)
		{
            int maxHitPoints = game.Baldur.MaxHitpoints;
            int currentHitPoints = game.Baldur.CurrentHitpoints;
            int flowers = game.Baldur.Flowers;
            int ores = game.Baldur.Ores;
            int potions = game.Baldur.Potions;
            //int weapons = game.Baldur.WeaponCounter;
            //int keycards = game.Baldur.KeycardCounter;
            int keycards = game.Baldur.KeycardCounter;
            int amors = game.Baldur.Defense;

            spriteBatch.Begin();
            spriteBatch.DrawString (gameFont, String.Format ("Spielzeit: {0}", game.GameTime.ToString()), new Vector2 (12, 12), Color.White);

            Rectangle sourceRectangle = new Rectangle(0, 64, game.Baldur.KeycardCounter * 32, 32);
            Rectangle destinationRectangle = new Rectangle(12, 25, game.Baldur.KeycardCounter * 32, 32);
            spriteBatch.Draw(CollectableIcons, destinationRectangle, sourceRectangle, Color.White);

            sourceRectangle = new Rectangle(0, 96, game.Baldur.WeaponCounter * 32, 32);
            destinationRectangle = new Rectangle(12, 57, game.Baldur.WeaponCounter * 32, 32);
            spriteBatch.Draw(CollectableIcons, destinationRectangle, sourceRectangle, Color.White);

            sourceRectangle = new Rectangle(0, 128, game.Baldur.ArmorCounter * 32, 32);
            destinationRectangle = new Rectangle(12, 89, game.Baldur.ArmorCounter * 32, 32);
            spriteBatch.Draw(CollectableIcons, destinationRectangle, sourceRectangle, Color.White);

            spriteBatch.Draw(HudIcons, new Vector2(GraphicsDevice.Viewport.Width - 92f, 4), Color.White);
            spriteBatch.DrawString (gameFont, String.Format ("{0}/{1}", currentHitPoints.ToString(), maxHitPoints.ToString()), new Vector2(GraphicsDevice.Viewport.Width - 58f, 12), Color.White);
            spriteBatch.DrawString (gameFont, ores.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 58f, 44), Color.White);
            spriteBatch.DrawString (gameFont, flowers.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 58f, 76), Color.White);
            spriteBatch.DrawString (gameFont, potions.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 58f, 108), Color.White);

            spriteBatch.End();


		}
	}
}

