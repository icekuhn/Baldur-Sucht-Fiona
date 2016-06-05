using System;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;
using BaldurSuchtFiona.Models;
using System.Collections.Generic;
using System.Linq;
using BaldurSuchtFiona.Interfaces;

namespace BaldurSuchtFiona.Components
{
	public class SimulationComponent : GameComponent
	{
        private bool isRunning;
		private Game1 game;
		public World World { get; set; }
		public Baldur Baldur { get; set; }

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

            Area area = LoadFromJson("base");

            World.Areas.Add(area); 

            if (isRunning)
                game.Scene.ReloadContent();
            else
                isRunning = true;
		}

		public override void Update (GameTime gameTime)
        {
            List<Action> transfers = new List<Action>();
            if (game.Input.Handled)
                return;

            foreach (var area in World.Areas)
            {
                foreach (var character in area.Objects.OfType<Baldur>())
                {
                    character.move += character.Velocity;

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

                            if (item is ICollectable && character is ICollector)
                            {
                                transfers.Add(() =>
                                    {
                                        area.Objects.Remove(item);
                                        (character as ICollector).Inventory.Add(item as Item);
                                        item.Position = Vector2.Zero;
                                    });
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

            foreach (var transfer in transfers)
                transfer();

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