﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;


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

		public SimulationComponent Simulation {
			get;
			private set;
		}

		public SceneComponent Scene {
			get;
			private set;
		}


		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = true;

			Input = new InputComponent (this);
			Input.UpdateOrder = 0;
			Components.Add (Input);

			Simulation = new SimulationComponent (this);
			Simulation.UpdateOrder = 1;
			Components.Add (Simulation);

			Scene = new SceneComponent (this);
			Scene.UpdateOrder = 2;
			Scene.DrawOrder = 0;
			Components.Add (Scene);
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
