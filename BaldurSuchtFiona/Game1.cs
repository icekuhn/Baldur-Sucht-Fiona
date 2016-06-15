using System;
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
        public TimeSpan GameTime{ get; set;}
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
            this.GameTime = new TimeSpan();
            var startPosition = new Vector2(15, 12);
            this.Baldur = new Baldur(this,startPosition);
            World = new World();
            isStarted = true;
            Area area = LoadFromJson("base");
            World.Area = area; 
            AllowTeleport = true;
        }

        public void Respawn()
        {
            this.GameTime += new TimeSpan(0, 0, 30);
            this.Baldur.Position = new Vector2(15, 12);
            this.Baldur.CurrentHitpoints = this.Baldur.MaxHitpoints;
            this.Baldur.IsDead = false;
            World = new World();
            isStarted = true;
            LoadLevel(0);
            AllowTeleport = true;
        }

        public void NewGame()
        {
            this.GameTime = new TimeSpan();
            var startPosition = new Vector2(15, 12);
            this.Baldur = new Baldur(this,startPosition);
            isStarted = true;
            World = new World();
            LoadLevel(0);
            AllowTeleport = true;
        }

        public void LoadLevel(int levelNumber){
            AllowTeleport = false;
            Area area;
            switch (levelNumber) {
            case 0:
                area = LoadFromJson ("base");
                World.Area = area;
                LoadBaseObjekts ();
                break;
            case 1:
                area = LoadFromJson ("level1");
                World.Area = area;
                LoadLevel1Objekts ();
                break;
            case 2:
                area = LoadFromJson ("level2");
                World.Area = area;
                LoadLevel2Objekts ();
                break;
            default:
                area = LoadFromJson ("base");
                World.Area = area;
                LoadBaseObjekts ();
                break;
            }

        }

        public void LoadDefaultObjekts(string mapPath){
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
            requiredObjektTextures.Add("sprite_miner.png");
            requiredObjektTextures.Add("collectables.png");
            requiredObjektTextures.Add("attack1.png");
            requiredObjektTextures.Add("attack2.png");
            requiredObjektTextures.Add("attack3.png");
            foreach (var objekt in this.World.Area.Objects)
                if (!string.IsNullOrEmpty(objekt.Texture) && !requiredObjektTextures.Contains(objekt.Texture))
                    requiredObjektTextures.Add(objekt.Texture);

            // Tileset Texturen laden
            Dictionary<string, Texture2D> tilesetTextures = new Dictionary<string, Texture2D>();
            foreach (var textureName in requiredTilesetTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(gd, stream);
                    tilesetTextures.Add(textureName, texture);
                }
            }            

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

            Scene.SetContent(tilesetTextures,objektTextures); 
        }

        public void LoadBaseObjekts(){
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            LoadDefaultObjekts(mapPath);


            var map = this.World.Area;
            var startPosition = new Vector2(15, 12);

            if (!isStarted)
            {
                startPosition = this.World.Area.GetTeleportPosition();
            }
            else
                isStarted = false;
            
            this.Baldur.Position = startPosition;

            if (this.Baldur.ArmorCounter < 1)
            {
                var armor = new Armor(this, 1);
                Baldur.Inventory.Add(armor);
            }

            if (this.Baldur.WeaponCounter < 1)
            {
                var weapon = new Weapon(this, 1);
                Baldur.Inventory.Add(weapon);
            }

            if (this.Baldur.KeycardCounter < 1)
            {
                var keycard = new Keycard(this, 1);
                Baldur.Inventory.Add(keycard);
            }

            if (this.Baldur.KeycardCounter == 1) {
                var keycard = new Keycard (this, 2, new Vector2(19, 19));
                map.Objects.Add (keycard);
            }
            map.Objects.Add(this.Baldur);
        }

        public void LoadLevel1Objekts(){
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            LoadDefaultObjekts(mapPath);   


            var map = this.World.Area;

            this.Baldur.Position = this.World.Area.GetTeleportPosition();
            map.Objects.Add(this.Baldur);

            var flower1 = new Flower(this,1,new Vector2(10,18.5f));
            map.Objects.Add(flower1);

            var farmer1 = new Farmer(this,new Vector2(9, 19),flower1);
            map.Objects.Add(farmer1);

            var flower2 = new Flower(this,1,new Vector2(16,18.5f));
            map.Objects.Add(flower2);

            var farmer2 = new Farmer(this,new Vector2(17, 17),flower2);
            map.Objects.Add(farmer2);

            var flower3 = new Flower(this,1,new Vector2(13,12.5f));
            map.Objects.Add(flower3);

            var farmer3 = new Farmer(this,new Vector2(10, 12),flower3);
            map.Objects.Add(farmer3);

            var iron1 = new Iron(this,1,new Vector2(18.5f,3));
            map.Objects.Add(iron1);

            var iron2 = new Iron(this,1,new Vector2(34.5f,5));
            map.Objects.Add(iron2);

            var miner1 = new Miner(this,new Vector2(28, 3));
            map.Objects.Add(miner1);

            var miner2 = new Miner(this,new Vector2(32, 7));
            map.Objects.Add(miner2);

            var miner3 = new Miner(this,new Vector2(27, 8));
            map.Objects.Add(miner3);

            var miner4 = new Miner(this,new Vector2(29, 2));
            map.Objects.Add(miner4);


            if (Baldur.KeycardCounter < 3)
            {
                var keycard1 = new Keycard(this, 3, new Vector2(30.5f, 3.1f));
                map.Objects.Add(keycard1);
            }
        }

        public void LoadLevel2Objekts(){
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            LoadDefaultObjekts(mapPath);   

            var map = this.World.Area;

            this.Baldur.Position = this.World.Area.GetTeleportPosition();
            map.Objects.Add(this.Baldur);

            var flower1 = new Flower(this,1,new Vector2(11,8.5f));
            map.Objects.Add(flower1);

            var farmer1 = new Farmer(this,new Vector2(10, 12),flower1);
            map.Objects.Add(farmer1);

            var flower2 = new Flower(this,2,new Vector2(13,9.5f));
            map.Objects.Add(flower2);

            var farmer2 = new Farmer(this,new Vector2(14, 5),flower2);
            map.Objects.Add(farmer2);

            var flower3 = new Flower(this,1,new Vector2(15,8.5f));
            map.Objects.Add(flower3);

            var farmer3 = new Farmer(this,new Vector2(16, 10),flower3);
            map.Objects.Add(farmer3);

            var flower4 = new Flower(this,1,new Vector2(11,10.5f));
            map.Objects.Add(flower4);

            var farmer4 = new Farmer(this,new Vector2(9, 13),flower4);
            map.Objects.Add(farmer4);

            var flower5 = new Flower(this,1,new Vector2(15,10.5f));
            map.Objects.Add(flower5);

            var farmer5 = new Farmer(this,new Vector2(14, 13),flower5);
            map.Objects.Add(farmer5);

            if (Baldur.KeycardCounter < 4)
            {
                var farmLeader = new FarmLeader(this, new Vector2(14.5f, 26.3f));
                map.Objects.Add(farmLeader);
            }

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

                    Area area = new Area(result);
                    area.Name = name;

                    area.Background = new Color(128,128,128);
                    if (!string.IsNullOrEmpty(result.backgroundcolor))
                    {
                        area.Background = new Color(
                            Convert.ToInt32(result.backgroundcolor.Substring(1, 2), 16),
                            Convert.ToInt32(result.backgroundcolor.Substring(3, 2), 16),
                            Convert.ToInt32(result.backgroundcolor.Substring(1, 2), 16));
                    }

                    return area;
                }
            }
        }

	}
}

