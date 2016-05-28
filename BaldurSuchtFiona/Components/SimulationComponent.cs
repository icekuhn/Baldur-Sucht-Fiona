using System;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace BaldurSuchtFiona.Components
{
	internal class SimulationComponent : GameComponent
	{
		private Game1 game;
		public World World { get; private set; }
        public Baldur Baldur { get; private set; }

        private float gap = 0.00001f;

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


			Baldur = new Baldur() { 
                Position = new Vector2(15, 10)
            };
			playerBase.Objects.Add(Baldur);

		}

		public override void Update (GameTime gameTime)
		{
            if (game.Input.Handled)
                return;
            foreach (var area in World.Areas)
            {
                foreach (var character in area.Objects.OfType<Character>())
                {
                    character.move += character.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    foreach (var item in area.Objects)
                    {
                        if (item == character)
                            continue;

                        Vector2 distance = (item.Position + item.move) - (character.Position + character.move);

                        float overlap = item.Radius + character.Radius - distance.Length();
                        if (overlap > 0f)
                        {
                            Vector2 resolution = distance * (overlap / distance.Length());
                            if (item.IsFixed && !character.IsFixed)
                            {
                                character.move -= resolution;
                            }
                            else if (!item.IsFixed && character.IsFixed)
                            {
                                item.move += resolution;
                            }
                            else if (!item.IsFixed && !character.IsFixed)
                            {
                                float totalMass = item.Mass + character.Mass;
                                character.move -= resolution * (item.Mass / totalMass);
                                item.move += resolution * (character.Mass / totalMass);
                            }
                        }
                    }
                }

                foreach (var item in area.Objects)
                {
                    bool collision = false;
                    int loops = 0;

                    do
                    {
                        Vector2 position = item.Position + item.move;
                        int minCellX = (int)(position.X - item.Radius);
                        int maxCellX = (int)(position.X + item.Radius);
                        int minCellY = (int)(position.Y - item.Radius);
                        int maxCellY = (int)(position.Y + item.Radius);

                        collision = false;
                        float minImpact = 2f;
                        int minAxis = 0;

                        for (int x = minCellX; x <= maxCellX; x++)
                        {
                            for (int y = minCellY; y <= maxCellY; y++)
                            {
                                if (!area.IsCellBlocked(x, y))
                                    continue;

                                if (position.X - item.Radius > x + 1 ||
                                    position.X + item.Radius < x ||
                                    position.Y - item.Radius > y + 1 ||
                                    position.Y + item.Radius < y)
                                    continue;

                                collision = true;

                                float diffX = float.MaxValue;
                                if (item.move.X > 0)
                                    diffX = position.X + item.Radius - x + gap;
                                if (item.move.X < 0)
                                    diffX = position.X - item.Radius - (x + 1) - gap;
                                float impactX = 1f - (diffX / item.move.X);

                                float diffY = float.MaxValue;
                                if (item.move.Y > 0)
                                    diffY = position.Y + item.Radius - y + gap;
                                if (item.move.Y < 0)
                                    diffY = position.Y - item.Radius - (y + 1) - gap;
                                float impactY = 1f - (diffY / item.move.Y);

                                int axis = 0;
                                float impact = 0;
                                if (impactX > impactY)
                                {
                                    axis = 1;
                                    impact = impactX;
                                }
                                else if(impactX < impactY)
                                {
                                    axis = 2;
                                    impact = impactY;
                                }

                                if (impact < minImpact)
                                {
                                    minImpact = impact;
                                    minAxis = axis;
                                }
                            }
                        }
                        if (collision)
                        {
                            if (minAxis == 1)
                                item.move *= new Vector2(minImpact, 1f);

                            if (minAxis == 2)
                                item.move *= new Vector2(1f, minImpact);
                        }
                        loops++;
                    }
                    while(collision && loops < 2);

                    item.Position += item.move;
                    item.move = Vector2.Zero;

                }
            }

            
            //velocity = game.Input.Movement;
			//Baldur.Position += velocity;
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