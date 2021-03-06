﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using BaldurSuchtFiona.Components;
using BaldurSuchtFiona.Models;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using BaldurSuchtFiona.Rendering;


namespace BaldurSuchtFiona
{
	/// <summary>
	/// 	This is the main type for your game.
	/// </summary>
	public class Game1 : Game
    {
        private bool isStarted;
        public World World { get; set; }
        public Baldur Baldur { get; set; }
        public bool AllowTeleport { get; set; }

		private GraphicsDeviceManager graphics;

		public InputComponent Input {
			get;
			set;
		}

		public ScreenComponent Screen {
			get;
			set;
		}

		public SimulationComponent Simulation {
			get;
			set;
		}

		public SceneComponent Scene {
			get;
			set;
		}

		public HudComponent Hud {
			get;
			set;
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

		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
            
			base.Initialize ();
        }

        public void StartGame()
        {
            var startPosition = new Vector2(15, 12);
            this.Baldur = new Baldur(this,startPosition);
            World = new World();
            isStarted = true;
            Area area = LoadFromJson("base");
            World.Area = area; 
            AllowTeleport = true;
        }

        public void NewGame()
        {
            isStarted = true;
            World = new World();
            LoadLevel(0);
            AllowTeleport = true;
        }

        public void LoadLevel(int levelNumber){
            AllowTeleport = false;
            Area area;
            switch (levelNumber)
            {
                case 0:
                    area = LoadFromJson("base");
                    World.Area = area; 
                    LoadBaseObjekts();
                    break;
                case 1:
                    area = LoadFromJson("level1");
                    World.Area = area; 
                    LoadLevel1Objekts();
                    break;
                default:
                    throw new NotImplementedException();
                    
            }

        }

        public void LoadBaseObjekts(){

            var gd = Scene.GetGraphicsDevice();
            List<string> requiredTilesetTextures = new List<string>();
            List<string> requiredObjektTextures = new List<string>();

            // Tile Texturen
            foreach (var tile in this.World.Area.Tiles.Values)
                if (!requiredTilesetTextures.Contains(tile.Texture))
                    requiredTilesetTextures.Add(tile.Texture);

            // Objekt Texturen
            requiredObjektTextures.Add("sprite_player_3.png");
            requiredObjektTextures.Add("sprite_farmer.png");
            requiredObjektTextures.Add("collectables.png");
            foreach (var objekt in this.World.Area.Objects)
                if (!string.IsNullOrEmpty(objekt.Texture) && !requiredObjektTextures.Contains(objekt.Texture))
                    requiredObjektTextures.Add(objekt.Texture);




            // Tileset Texturen laden
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            Dictionary<string, Texture2D> tilesetTextures = new Dictionary<string, Texture2D>();
            foreach (var textureName in requiredTilesetTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(gd, stream);
                    tilesetTextures.Add(textureName, texture);
                }
            }            
            var map = this.World.Area;



            // Objekt Texturen laden
            mapPath = Path.Combine(Environment.CurrentDirectory, "Content");
            Dictionary<string, Texture2D>  objektTextures = new Dictionary<string, Texture2D>();
            foreach (var textureName in requiredObjektTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(gd, stream);
                    objektTextures.Add(textureName, texture);
                }
            }     

            var startPosition = new Vector2(15, 12);

            if (!isStarted)
            {
                startPosition = this.World.Area.GetTeleportPosition();
            }
            else
                isStarted = false;
            
            this.Baldur.Position = startPosition;
            map.Objects.Add(this.Baldur);


            Scene.SetContent(tilesetTextures,objektTextures);
        }

        public void LoadLevel1Objekts(){

            var gd = Scene.GetGraphicsDevice();
            List<string> requiredTilesetTextures = new List<string>();
            List<string> requiredObjektTextures = new List<string>();

            // Tile Texturen
            foreach (var tile in this.World.Area.Tiles.Values)
                if (!requiredTilesetTextures.Contains(tile.Texture))
                    requiredTilesetTextures.Add(tile.Texture);

            // Objekt Texturen
            requiredObjektTextures.Add("sprite_player_3.png");
            requiredObjektTextures.Add("sprite_farmer.png");
            requiredObjektTextures.Add("collectables.png");
            foreach (var objekt in this.World.Area.Objects)
                if (!string.IsNullOrEmpty(objekt.Texture) && !requiredObjektTextures.Contains(objekt.Texture))
                    requiredObjektTextures.Add(objekt.Texture);




            // Tileset Texturen laden
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            Dictionary<string, Texture2D> tilesetTextures = new Dictionary<string, Texture2D>();
            foreach (var textureName in requiredTilesetTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(gd, stream);
                    tilesetTextures.Add(textureName, texture);
                }
            }            
            var map = this.World.Area;


            // Objekt Texturen laden
            mapPath = Path.Combine(Environment.CurrentDirectory, "Content");
            Dictionary<string, Texture2D>  objektTextures = new Dictionary<string, Texture2D>();
            foreach (var textureName in requiredObjektTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(gd, stream);
                    objektTextures.Add(textureName, texture);
                }
            }     


            this.Baldur.Position = this.World.Area.GetTeleportPosition();
            map.Objects.Add(this.Baldur);

            var flower1 = new Flower(this,1,new Vector2(15,21));
            map.Objects.Add(flower1);

            var farmer1 = new Farmer(this,new Vector2(16, 23));
            map.Objects.Add(farmer1);

            if (Baldur.KeycardCounter == 0)
            {
                var keycard1 = new Keycard(this, 1, new Vector2(13, 18));
                map.Objects.Add(keycard1);
            }

            Scene.SetContent(tilesetTextures,objektTextures);
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
                            bool teleporter = false;
                            bool safeZone = false;
                            if (tileset.tileproperties != null)
                            {
                                FileTileProperty property;
                                if (tileset.tileproperties.TryGetValue(j, out property))
                                {
                                    block = property.blocked;
                                    teleporter = property.teleporter;
                                    safeZone = property.safeZone;
                                }
                            }

                            Tile tile = new Tile()
                                {
                                    Texture = tileset.image,
                                    SourceRectangle = new Rectangle(x * width, y * width, width, width),
                                    Blocked = block,
                                    Teleporter = teleporter,
                                    SafeZone = safeZone
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

