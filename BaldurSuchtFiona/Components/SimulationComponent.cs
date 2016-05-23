using System;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;

namespace BaldurSuchtFiona.Components
{
	internal class SimulationComponent : GameComponent
	{
		private Game1 game;
		public World World { get; private set; }
		public Baldur Baldur { get; private set; }

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
			NewGame();
		}

		public void NewGame()
		{
			World = new World();

			Area playerBase = LoadFromJson("base");
			World.Areas.Add(playerBase);

			Baldur = new Baldur() { Position = new Vector2(15, 10) };
			playerBase.Objects.Add(Baldur);

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

		private Area LoadFromJson(string name)
		{
			string rootPath = Path.Combine(Environment.CurrentDirectory, "Maps");
			using (Stream stream = File.OpenRead(rootPath + "\\" + name + ".json"))
			{
				using (StreamReader sr = new StreamReader(stream))
				{
					string json = sr.ReadToEnd();
					FileArea result = JsonConvert.DeserializeObject<FileArea>(json);

                    return new Area(result){Name = name};
				}
			}
		}
	}
}