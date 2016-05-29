using System;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona.Components
{
	public class SimulationComponent : GameComponent
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

            Area area = LoadFromJson("base");

            Baldur = new Baldur() { Position = new Vector2(15.5f, 11.5f), Radius = 1f };
			//playerBase.Objects.Add(Baldur);
            Flower flower = new Flower() { Position = new Vector2(5, 5), Radius = 0.25f };

            area.Items.Add(flower);
            area.Players.Add(Baldur);

            World.Areas.Add(area); 
		}

		public override void Update (GameTime gameTime)
		{
			if (!game.Input.Handled) 
			{
				Baldur.Velocity = game.Input.Movement /10;
			}
			else 
			{
				Baldur.Velocity = Vector2.Zero;
			}

            Baldur.Position += Baldur.Velocity; // * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
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
        
                    Area area = new Area(result.layers.Length, result.width, result.height);
                    area.Name = name;

                    area.Background = new Color(128,128,128);
                    if (!string.IsNullOrEmpty(result.backgroundcolor))
                    {
                        area.Background = new Color(
                            Convert.ToInt32(result.backgroundcolor.Substring(1, 2), 16),
                            Convert.ToInt32(result.backgroundcolor.Substring(3, 2), 16),
                            Convert.ToInt32(result.backgroundcolor.Substring(1, 2), 16));
                    }

                    for (int i = 0; i < result.tilesets.Length; i++)
                    {
                        FileTileset tileset = result.tilesets[i];

                        int start = tileset.firstgid;
                        int perRow = tileset.imagewidth / tileset.tilewidth;
                        int width = tileset.tilewidth;

                        for (int j = 0; j < tileset.tilecount; j++)
                        {
                            int x = j % perRow;
                            int y = j / perRow;
                         
                            bool block = false;
                            if (tileset.tileproperties != null)
                            {
                                FileTileProperty property;
                                if (tileset.tileproperties.TryGetValue(j, out property))
                                    block = property.blocked;
                            }

                            Tile tile = new Tile()
                            {
                                Texture = tileset.image,
                                SourceRectangle = new Rectangle(x * width, y * width, width, width),
                                Blocked = block
                            };

                            area.Tiles.Add(start + j, tile);
                        }
                    }

                    for (int l = 0; l < result.layers.Length; l++)
                    {
                        FileLayer layer = result.layers[l];
                        for (int i = 0; i < layer.data.Length; i++)
                        {
                            int x = i % area.Width;
                            int y = i / area.Width;
                            area.Layers[l].Tiles[x, y] = layer.data[i];
                        }
                    }
                    return area;
                }
			}
		}
	}
}