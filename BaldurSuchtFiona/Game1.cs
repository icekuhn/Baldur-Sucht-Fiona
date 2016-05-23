using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using BaldurSuchtFiona.Components;


namespace BaldurSuchtFiona
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	internal class Game1 : Game
	{
		private GraphicsDeviceManager graphics;

		public InputComponent Input {
			get;
			private set;
		}

		public ScreenComponent Screen {
			get;
			private set;
		}

		public SimulationComponent Simulation {
			get;
			private set;
		}

		public SceneComponent Scene {
			get;
			private set;
		}

		public HudComponent Hud {
			get;
			private set;
		}

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = false;

			Input = new InputComponent (this);
			Input.UpdateOrder = 0;
			Components.Add (Input);

			Screen = new ScreenComponent (this);
			Screen.UpdateOrder = 1;
			Screen.DrawOrder = 2;
			Components.Add (Screen);

			Simulation = new SimulationComponent (this);
			Simulation.UpdateOrder = 2;
			Components.Add (Simulation);

			Scene = new SceneComponent (this);
			Scene.UpdateOrder = 3;
			Scene.DrawOrder = 0;
			Components.Add (Scene);

			Hud = new HudComponent (this);
			Hud.UpdateOrder = 4;
			Hud.DrawOrder = 1;
			Components.Add (Hud);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
            
			base.Initialize ();
		}

	}
}

