using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BaldurSuchtFiona.Screens;
using System.Collections.Generic;
using BaldurSuchtFiona.Rendering;
using System.IO;

namespace BaldurSuchtFiona.Components
{
	public class ScreenComponent : DrawableGameComponent
	{
		public new Game1 Game; 
		private SpriteBatch spriteBatch;
        public Stack<Screen> screens { get; private set; }

		public Texture2D Pixel { get; private set; }

		public SpriteFont Font { get; private set; }

        public Texture2D CollectableIcons { get; private set; }

        public Texture2D hudIcons { get; private set; }

		public NineTileRenderer Panel { get; private set; }

		public NineTileRenderer Button { get; private set; }

        public NineTileRenderer Icon { get; private set; } 

		public NineTileRenderer ButtonHovered { get; private set; }

        public NineTileRenderer IconHovered { get; private set; } 

		public NineTileRenderer Border { get; private set; }

		public Screen ActiveScreen {
			get { return screens.Count > 0 ? screens.Peek () : null; }
		}

		public ScreenComponent (Game1 game) : base(game) {
			this.Game = game;
			screens = new Stack<Screen> ();
		}

		public void ShowScreen(Screen screen) {
			screens.Push (screen);
		}

		public void CloseScreen() {
			if (screens.Count > 0)
				screens.Pop ();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch (GraphicsDevice);

			Pixel = new Texture2D (GraphicsDevice, 1, 1);
			Pixel.SetData(new [] {Color.White});

			Font = Game.Content.Load<SpriteFont>("GameFont");

            string mapPath = Path.Combine(Environment.CurrentDirectory, "Content");
            using (Stream stream = File.OpenRead(mapPath + "\\collectables.png"))
            {
                CollectableIcons = Texture2D.FromStream(GraphicsDevice, stream);
            }

            using (Stream stream = File.OpenRead (mapPath + "\\hudIcons.png")) {
                hudIcons = Texture2D.FromStream (GraphicsDevice, stream);
            }

			Texture2D texture = Game.Content.Load<Texture2D> ("MenuItems");
			Panel = new NineTileRenderer (texture, new Rectangle (0, 0, 96, 96), new Point (30, 30));
			Button = new NineTileRenderer (texture, new Rectangle (104, 8, 80, 80), new Point (20, 20));
			ButtonHovered = new NineTileRenderer (texture, new Rectangle (200, 8, 80, 80), new Point (20, 20));
			Border = new NineTileRenderer (texture, new Rectangle (296, 8, 80, 80), new Point (20, 20));
            Icon =  new NineTileRenderer (texture, new Rectangle(104, 8, 80, 80), new Point (20, 20));
            IconHovered = new NineTileRenderer (texture, new Rectangle (200, 8, 80, 80), new Point (20, 20));
		}

		public override void Update(GameTime gameTime) {
			Screen activeScreen = ActiveScreen;
            if (activeScreen != null)
            {
                foreach (var control in activeScreen.Controls)
                    control.Update(gameTime);
                activeScreen.Update(gameTime);
                Game.Input.Handled = true;
            }

			if (!Game.Input.Handled) {
				if (Game.Input.Close) {
					ShowScreen (new MainMenuScreen (this));
					Game.Input.Handled = true;
				}
			}
				
		}

		public override void Draw(GameTime gameTime) {
			spriteBatch.Begin( samplerState: SamplerState.LinearWrap);
            var list = screens.ToArray();
            Array.Reverse(list);
            foreach (var screen in list)
                if (screen is PotionScreen) { (screen as PotionScreen).Draw (gameTime, spriteBatch); }
                else if (screen is Armor2Screen) { (screen as Armor2Screen).Draw (gameTime, spriteBatch); }
                else if (screen is Armor3Screen) { (screen as Armor3Screen).Draw (gameTime, spriteBatch); }            
                else { screen.Draw (gameTime, spriteBatch); }
			spriteBatch.End ();
		}
	}
}

