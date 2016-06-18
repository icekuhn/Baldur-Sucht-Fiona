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
using System.Xml.Linq;
using System.Xml;
using System.Linq;


namespace BaldurSuchtFiona
{
	/// <summary>
	/// 	This is the main type for your game.
	/// </summary>
	public class Game1 : Game
    {
        private readonly string _saveGameFileLocation;
        private bool isStarted;
        public World World { get; set; }
        public Baldur Baldur { get; set; }
        public bool AllowTeleport { get; set; }
        public bool AllowPotionScreen { get; set; }
        public bool AllowWorkBenchScreen { get; set; }
        public bool AllowBedScreen { get; set; }
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
            _saveGameFileLocation = Path.Combine(Environment.CurrentDirectory, "Content","saveGame.xml");
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

        public void SaveGame(){
            int ores1 = 0;
            int ores2 = 0;
            int ores3 = 0;
            int flower1 = 0;
            int flower2 = 0;
            int flower3 = 0;
            foreach (var item in Baldur.Inventory) {
                if ((item is Iron)) {
                    if ((item as Iron).Value == 1) { ores1 += 1; }
                    if ((item as Iron).Value == 2) { ores2 += 1; }
                    if ((item as Iron).Value == 3) { ores3 += 1; }
                }
                if ((item is Flower)) {
                    if ((item as Flower).Value == 1) { flower1 += 1; }
                    if ((item as Flower).Value == 2) { flower2 += 1; }
                    if ((item as Flower).Value == 3) { flower3 += 1; }
                }
            }

            var doc = new XDocument();
            doc.Add (new XElement("saveGameValue",
                new XElement("Ores",
                    new XElement("Ore1", ores1),
                    new XElement("Ore2", ores2),
                    new XElement("Ore3", ores3)),
                new XElement("Flowers",
                    new XElement("Flower1", flower1),
                    new XElement("Flower2", flower2),
                    new XElement("Flower3", flower3)),
                new XElement("Potions",Baldur.Potions),
                new XElement("Keycards",Baldur.KeycardCounter),
                new XElement("Armor",Baldur.ArmorCounter),
                new XElement("Weapon",Baldur.WeaponCounter),
                new XElement("HitPoints",Baldur.CurrentHitpoints),
                new XElement("GameTimeTicks",this.GameTime.Ticks)
            ));


            if (File.Exists (_saveGameFileLocation))
                File.Delete (_saveGameFileLocation);

            doc.Save (_saveGameFileLocation);
        }

        public void LoadGame(){
            if (!File.Exists (_saveGameFileLocation))
                throw new NotImplementedException ("SpeicherDatei nicht vorhanden.Wichtig zu prüfen");

            var startPosition = new Vector2(15, 12);
            this.Baldur = new Baldur(this,startPosition);
            isStarted = true;
            World = new World();
            LoadLevel(0);
            AllowTeleport = true;

            Baldur.Position = new Vector2(17.8f,27.25f); 

            var xdoc = XDocument.Load(_saveGameFileLocation);

            var test = xdoc.Elements ();



            var ores = xdoc.Root.Elements().FirstOrDefault(el => el.Name == "Ores");
            var ores1 = ores.Elements().FirstOrDefault(el => el.Name == "Ore1").Value;
            var ores2 = ores.Elements().FirstOrDefault(el => el.Name == "Ore2").Value;
            var ores3 = ores.Elements().FirstOrDefault(el => el.Name == "Ore3").Value;
            var flowers = xdoc.Root.Elements ().FirstOrDefault (el => el.Name == "Flowers");
            var flower1 = flowers.Elements().FirstOrDefault(el => el.Name == "Flower1").Value;
            var flower2 = flowers.Elements().FirstOrDefault(el => el.Name == "Flower2").Value;
            var flower3 = flowers.Elements().FirstOrDefault(el => el.Name == "Flower3").Value;
            var potions = xdoc.Root.Elements ().FirstOrDefault (el => el.Name == "Potions").Value;
            var keycards = xdoc.Root.Elements ().FirstOrDefault (el => el.Name == "Keycards").Value;
            var armor = xdoc.Root.Elements ().FirstOrDefault (el => el.Name == "Armor").Value;
            var weapon = xdoc.Root.Elements ().FirstOrDefault (el => el.Name == "Weapon").Value;
            var hitPoints = xdoc.Root.Elements ().FirstOrDefault (el => el.Name == "HitPoints").Value;
            var gameTimeTicks = xdoc.Root.Elements ().FirstOrDefault (el => el.Name == "GameTimeTicks").Value;

            int intReturnValue;

            Baldur.Inventory.Clear ();

            if(int.TryParse (ores1,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Iron(this,1));
            if(int.TryParse (ores2,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Iron(this,2));
            if(int.TryParse (ores3,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Iron(this,3));
            
            if(int.TryParse (flower1,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Flower(this,1));
            if(int.TryParse (flower2,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Flower(this,2));
            if(int.TryParse (flower3,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Flower(this,3));


            if(int.TryParse (potions,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Healpot());

            if(int.TryParse (keycards,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Keycard(this,i+1));
            
            if(int.TryParse (armor,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Armor(this,i+1));
           
            if(int.TryParse (weapon,out intReturnValue))
                for (var i = 0; i < intReturnValue; i++) 
                    Baldur.Inventory.Add (new Weapon(this,i+1));
            
            if(int.TryParse (hitPoints,out intReturnValue))
                Baldur.CurrentHitpoints = intReturnValue;

            long tickReturnValue;
            if(long.TryParse (gameTimeTicks,out tickReturnValue))
                this.GameTime = new TimeSpan(tickReturnValue);
        }

        public void LoadLevel(int levelNumber){
            Scene.ClearRenderer();
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
            case 3:
                area = LoadFromJson ("level3");
                World.Area = area;
                LoadLevel3Objekts ();
                break;
            case 4:
                area = LoadFromJson ("level4");
                World.Area = area;
                LoadLevel4Objekts ();
                break;
            case 5:
                area = LoadFromJson ("level5");
                World.Area = area;
                LoadLevel5Objekts ();
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

            var flower1 = new Flower(this,2,new Vector2(9.5f,18f));
            map.Objects.Add(flower1);

            var flower2 = new Flower (this, 1, new Vector2 (10.5f, 18f));
            map.Objects.Add (flower2);

            var flower3 = new Flower (this, 1, new Vector2 (11.5f, 18f));
            map.Objects.Add (flower3);

            var flower4 = new Flower (this, 1, new Vector2 (12.5f, 18f));
            map.Objects.Add (flower4);

            var flower5 = new Flower (this, 1, new Vector2 (13.5f, 18f));
            map.Objects.Add (flower5);

            var flower6 = new Flower (this, 1, new Vector2 (14.5f, 18f));
            map.Objects.Add (flower6);

            var flower7 = new Flower (this, 1, new Vector2 (15.5f, 18f));
            map.Objects.Add (flower7);

            var flower8 = new Flower (this, 1, new Vector2 (16.5f, 18f));
            map.Objects.Add (flower8);

            var flower9 = new Flower (this, 1, new Vector2 (10.5f, 19f));
            map.Objects.Add (flower9);

            var flower10 = new Flower (this, 1, new Vector2 (11.5f, 19f));
            map.Objects.Add (flower10);

            var flower11 = new Flower (this, 1, new Vector2 (12.5f, 19f));
            map.Objects.Add (flower11);

            var flower12 = new Flower (this, 1, new Vector2 (13.5f, 19f));
            map.Objects.Add (flower12);

            var flower13 = new Flower (this, 1, new Vector2 (14.5f, 19f));
            map.Objects.Add (flower13);

            var flower14 = new Flower (this, 1, new Vector2 (15.5f, 19f));
            map.Objects.Add (flower14);

            var flower15 = new Flower (this, 1, new Vector2 (16.5f, 19f));
            map.Objects.Add (flower15);

            var farmer1 = new Farmer (this, new Vector2 (9.5f, 20f), new List<Flower> {flower1, flower2, flower3, flower4, flower5, flower6, flower7, flower8, flower9, flower10, flower11, flower12, flower13, flower14, flower15});
            map.Objects.Add (farmer1);


            var flower21 = new Flower (this, 1, new Vector2 (9.5f, 16f));
            map.Objects.Add (flower21);

            var flower22 = new Flower (this, 1, new Vector2 (10.5f, 16f));
            map.Objects.Add (flower22);

            var flower23 = new Flower (this, 1, new Vector2 (11.5f, 16f));
            map.Objects.Add (flower23);

            var flower24 = new Flower (this, 1, new Vector2 (12.5f, 16f));
            map.Objects.Add (flower24);

            var flower25 = new Flower (this, 1, new Vector2 (13.5f, 16f));
            map.Objects.Add (flower25);

            var flower26 = new Flower (this, 1, new Vector2 (14.5f, 16f));
            map.Objects.Add (flower26);

            var flower27 = new Flower (this, 1, new Vector2 (15.5f, 16f));
            map.Objects.Add (flower27);

            var flower28 = new Flower (this, 1, new Vector2 (16.5f, 16f));
            map.Objects.Add (flower28);

            var flower29 = new Flower (this, 1, new Vector2 (9.5f, 15f));
            map.Objects.Add (flower29);

            var flower30 = new Flower (this, 1, new Vector2 (10.5f, 15f));
            map.Objects.Add (flower30);
                
            var flower31 = new Flower (this, 1, new Vector2 (11.5f, 15f));
            map.Objects.Add (flower31);

            var flower32 = new Flower (this, 1, new Vector2 (12.5f, 15f));
            map.Objects.Add (flower32);

            var flower33 = new Flower (this, 1, new Vector2 (13.5f, 15f));
            map.Objects.Add (flower33);

            var flower34 = new Flower (this, 1, new Vector2 (14.5f, 15f));
            map.Objects.Add (flower34);

            var flower35 = new Flower (this, 1, new Vector2 (15.5f, 15f));
            map.Objects.Add (flower35);

            var flower36 = new Flower (this, 1, new Vector2 (16.5f, 15f));
            map.Objects.Add (flower36);

            var farmer2 = new Farmer(this,new Vector2(8.5f, 16f),new List<Flower>{flower21, flower22, flower23, flower24, flower25, flower26, flower27, flower28, flower29, flower30, flower31, flower32, flower33, flower34, flower35, flower36});
            map.Objects.Add(farmer2);


            var flower41 = new Flower (this, 1, new Vector2 (9.5f, 13f));
            map.Objects.Add (flower41);

            var flower42 = new Flower (this, 1, new Vector2 (10.5f, 13f));
            map.Objects.Add (flower42);

            var flower43 = new Flower (this, 1, new Vector2 (11.5f, 13f));
            map.Objects.Add (flower43);

            var flower44 = new Flower (this, 1, new Vector2 (12.5f, 13f));
            map.Objects.Add (flower44);

            var flower45 = new Flower (this, 1, new Vector2 (13.5f, 13f));
            map.Objects.Add (flower45);

            var flower46 = new Flower (this, 1, new Vector2 (14.5f, 13f));
            map.Objects.Add (flower46);

            var flower47 = new Flower (this, 2, new Vector2 (16.5f, 13f));
            map.Objects.Add (flower47);

            var flower48 = new Flower (this, 1, new Vector2 (9.5f, 12f));
            map.Objects.Add (flower48);

            var flower49 = new Flower (this, 1, new Vector2 (10.5f, 12f));
            map.Objects.Add (flower49);

            var flower50 = new Flower (this, 1, new Vector2 (11.5f, 12f));
            map.Objects.Add (flower50);

            var flower51 = new Flower (this, 1, new Vector2 (12.5f, 12f));
            map.Objects.Add (flower51);

            var flower52 = new Flower (this, 1, new Vector2 (13.5f, 12f));
            map.Objects.Add (flower52);

            var flower53 = new Flower (this, 1, new Vector2 (14.5f, 12f));
            map.Objects.Add (flower53);

            var flower54 = new Flower (this, 1, new Vector2 (15.5f, 12f));
            map.Objects.Add (flower54);

            var flower55 = new Flower (this, 1, new Vector2 (16.5f, 12f));
            map.Objects.Add (flower55);

            var farmer3 = new Farmer(this,new Vector2(8.5f, 13f),new List<Flower>{flower41, flower42, flower43, flower44, flower45, flower46, flower47, flower48, flower49, flower50, flower51, flower52, flower53, flower54, flower55  });
            map.Objects.Add(farmer3);

            var iron1 = new Iron (this, 1, new Vector2 (38.5f, 2.8f));
            map.Objects.Add(iron1);

            var iron2 = new Iron (this, 1, new Vector2 (37.5f, 2.8f));
            map.Objects.Add(iron2);

            var iron3 = new Iron (this, 1, new Vector2 (36.5f, 2.8f));
            map.Objects.Add (iron3);

            var iron4 = new Iron (this, 1, new Vector2 (35.5f, 2.8f));
            map.Objects.Add (iron4);

            var iron5 = new Iron (this, 1, new Vector2 (38.5f, 3.8f));
            map.Objects.Add (iron5);

            var iron6 = new Iron (this, 1, new Vector2 (37.5f, 3.8f));
            map.Objects.Add (iron6);

            var iron7 = new Iron (this, 1, new Vector2 (36.5f, 3.8f));
            map.Objects.Add (iron7);

            var iron8 = new Iron (this, 1, new Vector2 (35.5f, 3.8f));
            map.Objects.Add (iron8);


            var iron11 = new Iron (this, 1, new Vector2 (22.5f, 5.8f));
            map.Objects.Add (iron11);

            var iron12 = new Iron (this, 1, new Vector2 (23.5f, 5.8f));
            map.Objects.Add (iron12);

            var iron13 = new Iron (this, 1, new Vector2 (24.5f, 5.8f));
            map.Objects.Add (iron13);

            var iron14 = new Iron (this, 1, new Vector2 (22.5f, 6.8f));
            map.Objects.Add (iron14);

            var iron15 = new Iron (this, 1, new Vector2 (23.5f, 6.8f));
            map.Objects.Add (iron15);

            var iron16 = new Iron (this, 1, new Vector2 (24.5f, 6.8f));
            map.Objects.Add (iron16);

            var iron17 = new Iron (this, 1, new Vector2 (22.5f, 7.8f));
            map.Objects.Add (iron17);

            var iron18 = new Iron (this, 1, new Vector2 (23.5f, 7.8f));
            map.Objects.Add (iron18);

            var iron19 = new Iron (this, 1, new Vector2 (24.5f, 7.8f));
            map.Objects.Add (iron19);


            var iron20 = new Iron (this, 2, new Vector2 (37.5f, 12.8f));
            map.Objects.Add (iron20);


            var miner1 = new Miner(this,new Vector2(34.5f, 4.8f));
            map.Objects.Add(miner1);

            var miner2 = new Miner(this,new Vector2(25.5f, 6.8f));
            map.Objects.Add(miner2);

            var miner3 = new Miner(this,new Vector2(35.5f, 12.8f));
            map.Objects.Add(miner3);

            var miner4 = new Miner(this,new Vector2(35.5f, 11.8f));
            map.Objects.Add(miner4);


            if (Baldur.KeycardCounter < 3)
            {
                var keycard1 = new Keycard(this, 3, new Vector2(18.5f, 6.8f));
                map.Objects.Add(keycard1);
            }
        }

        public void LoadLevel2Objekts(){
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            LoadDefaultObjekts(mapPath);   

            var map = this.World.Area;

            this.Baldur.Position = this.World.Area.GetTeleportPosition();
            map.Objects.Add(this.Baldur);

            var flower1 = new Flower (this, 1, new Vector2 (10f, 4.6f));
            map.Objects.Add (flower1);

            var flower2 = new Flower (this, 1, new Vector2 (11f, 4.6f));
            map.Objects.Add (flower2);

            var flower3 = new Flower (this, 1, new Vector2 (12f, 4.6f));
            map.Objects.Add (flower3);

            var flower4 = new Flower (this, 1, new Vector2 (13f, 4.6f));
            map.Objects.Add (flower4);

            var flower5 = new Flower (this, 1, new Vector2 (14f, 4.6f));
            map.Objects.Add (flower5);

            var flower6 = new Flower (this, 1, new Vector2 (15f, 4.6f));
            map.Objects.Add (flower6);

            var flower7 = new Flower (this, 1, new Vector2 (16f, 4.6f));
            map.Objects.Add (flower7);

            var flower8 = new Flower (this, 1, new Vector2 (17f, 4.6f));
            map.Objects.Add (flower8);

            var farmer1 = new Farmer(this,new Vector2(10f, 5.6f),new List<Flower>{flower1, flower2, flower3, flower4, flower5, flower6, flower7, flower8 });
            map.Objects.Add(farmer1);


            var flower11 = new Flower (this, 1, new Vector2 (11.5f, 8f));
            map.Objects.Add(flower11);

            var flower12 = new Flower (this, 1, new Vector2 (13.5f, 8f));
            map.Objects.Add (flower12);

            var flower13 = new Flower (this, 1, new Vector2 (15.5f, 8f));
            map.Objects.Add (flower13);

            var flower14 = new Flower (this, 1, new Vector2 (11.5f, 10f));
            map.Objects.Add (flower14);

            var flower15 = new Flower (this, 2, new Vector2 (13.5f, 10f));
            map.Objects.Add (flower15);

            var flower16 = new Flower (this, 1, new Vector2 (15.5f, 10f));
            map.Objects.Add (flower16);

            var flower17 = new Flower (this, 1, new Vector2 (11.5f, 12.5f));
            map.Objects.Add (flower17);

            var flower18 = new Flower (this, 1, new Vector2 (13.5f, 12.5f));
            map.Objects.Add (flower18);

            var flower19 = new Flower (this, 1, new Vector2 (15.5f, 12.5f));
            map.Objects.Add (flower19);

            var farmer2 = new Farmer(this,new Vector2(10.5f, 9.5f),new List<Flower>{flower11, flower12, flower13, flower14, flower15, flower16, flower17, flower18, flower19 });
            map.Objects.Add(farmer2);


            var flower21 = new Flower (this, 1, new Vector2 (23f, 13f));
            map.Objects.Add(flower21);

            var flower22 = new Flower (this, 1, new Vector2 (25f, 13f));
            map.Objects.Add (flower22);

            var flower23 = new Flower (this, 1, new Vector2 (23f, 15f));
            map.Objects.Add (flower23);

            var flower24 = new Flower (this, 1, new Vector2 (25f, 15f));
            map.Objects.Add (flower24);

            var flower25 = new Flower (this, 1, new Vector2 (23f, 17f));
            map.Objects.Add (flower25);

            var flower26 = new Flower (this, 1, new Vector2 (25f, 17f));
            map.Objects.Add (flower26);

            var farmer3 = new Farmer(this,new Vector2(21f, 15f),new List<Flower>{flower21, flower22, flower23, flower24, flower25, flower26 });
            map.Objects.Add(farmer3);


            var flower31 = new Flower (this, 2, new Vector2(40.5f, 25.8f));
            map.Objects.Add(flower31);

            var flower32 = new Flower (this, 2, new Vector2 (42.5f, 25.8f));
            map.Objects.Add (flower32);

            var flower33 = new Flower (this, 2, new Vector2 (43.5f, 25.8f));
            map.Objects.Add (flower33);

            var flower34 = new Flower (this, 2, new Vector2 (44.5f, 25.8f));
            map.Objects.Add (flower34);

            var flower35 = new Flower (this, 2, new Vector2 (45.5f, 25.8f));
            map.Objects.Add (flower35);

            var flower36 = new Flower (this, 2, new Vector2 (46.5f, 25.8f));
            map.Objects.Add (flower36);

            var farmer4 = new Farmer(this,new Vector2(35.5f, 25.8f),new List<Flower>{flower31, flower32, flower33, flower34, flower35, flower36 });
            map.Objects.Add(farmer4);


            var flower41 = new Flower (this, 1, new Vector2 (47f, 9.3f));
            map.Objects.Add(flower41);

            var flower42 = new Flower (this, 1, new Vector2 (47f, 10.3f));
            map.Objects.Add (flower42);

            var flower43 = new Flower (this, 1, new Vector2 (47f, 11.3f));
            map.Objects.Add (flower43);

            var flower44 = new Flower (this, 1, new Vector2 (47f, 12.3f));
            map.Objects.Add (flower44);

            var flower45 = new Flower (this, 1, new Vector2 (47f, 13.3f));
            map.Objects.Add (flower45);

            var flower46 = new Flower (this, 1, new Vector2 (47f, 14.3f));
            map.Objects.Add (flower46);

            var flower47 = new Flower (this, 1, new Vector2 (47f, 15.3f));
            map.Objects.Add (flower47);

            var flower48 = new Flower (this, 1, new Vector2 (47f, 16.3f));
            map.Objects.Add (flower48);

            var flower50 = new Flower (this, 1, new Vector2 (47f, 17.3f));
            map.Objects.Add (flower50);

            var flower51 = new Flower (this, 1, new Vector2 (47f, 18.3f));
            map.Objects.Add (flower51);

            var flower52 = new Flower (this, 1, new Vector2 (47f, 19.3f));
            map.Objects.Add (flower52);

            var flower53 = new Flower (this, 1, new Vector2 (46f, 17.3f));
            map.Objects.Add (flower53);

            var flower54 = new Flower (this, 1, new Vector2 (46f, 18.3f));
            map.Objects.Add (flower54);

            var flower55 = new Flower (this, 1, new Vector2 (46f, 19.3f));
            map.Objects.Add (flower55);

            var flower56 = new Flower (this, 1, new Vector2 (45f, 18.3f));
            map.Objects.Add (flower56);

            var flower57 = new Flower (this, 1, new Vector2 (45f, 19.3f));
            map.Objects.Add (flower57);

            var flower58 = new Flower (this, 1, new Vector2 (44f, 19.3f));
            map.Objects.Add (flower58);

            var flower59 = new Flower (this, 1, new Vector2 (43f, 19.3f));
            map.Objects.Add (flower59);

            var flower60 = new Flower (this, 1, new Vector2 (42f, 19.3f));
            map.Objects.Add (flower60);

            var flower61 = new Flower (this, 1, new Vector2 (41f, 19.3f));
            map.Objects.Add (flower61);

            var farmer5 = new Farmer(this,new Vector2(42f, 16.5f),new List<Flower>{flower41, flower42, flower43, flower44, flower45, flower46, flower47, flower48, flower50, flower51, flower52, flower53, flower54, flower55, flower56, flower57, flower58, flower59, flower60, flower61 });
            map.Objects.Add(farmer5);


            var flower71 = new Flower (this, 1, new Vector2 (3.4f, 24.9f));
            map.Objects.Add (flower71);

            var flower72 = new Flower (this, 1, new Vector2 (3.4f, 25.9f));
            map.Objects.Add (flower72);

            var flower73 = new Flower (this, 1, new Vector2 (3.4f, 26.9f));
            map.Objects.Add (flower73);

            var flower74 = new Flower (this, 1, new Vector2 (4.4f, 24.9f));
            map.Objects.Add (flower74);

            var flower75 = new Flower (this, 1, new Vector2 (4.4f, 26.9f));
            map.Objects.Add (flower75);

            var flower76 = new Flower (this, 1, new Vector2 (5.4f, 24.9f));
            map.Objects.Add (flower76);

            var flower77 = new Flower (this, 3, new Vector2 (5.4f, 25.9f));
            map.Objects.Add (flower77);

            var flower78 = new Flower (this, 1, new Vector2 (5.4f, 26.9f));
            map.Objects.Add (flower78);

            var flower79 = new Flower (this, 1, new Vector2 (6.4f, 24.9f));
            map.Objects.Add (flower79);

            var flower80 = new Flower (this, 3, new Vector2 (6.4f, 25.9f));
            map.Objects.Add (flower80);

            var flower81 = new Flower (this, 1, new Vector2 (6.4f, 26.9f));
            map.Objects.Add (flower81);

            var flower82 = new Flower (this, 1, new Vector2 (7.4f, 24.9f));
            map.Objects.Add (flower82);

            var flower83 = new Flower (this, 3, new Vector2 (7.4f, 25.9f));
            map.Objects.Add (flower83);

            var flower84 = new Flower (this, 1, new Vector2 (7.4f, 26.9f));
            map.Objects.Add (flower84);

            var flower85 = new Flower (this, 1, new Vector2 (8.4f, 24.9f));
            map.Objects.Add (flower85);

            var flower86 = new Flower (this, 3, new Vector2 (8.4f, 25.9f));
            map.Objects.Add (flower86);

            var flower87 = new Flower (this, 1, new Vector2 (8.4f, 26.9f));
            map.Objects.Add (flower87);

            var flower88 = new Flower (this, 1, new Vector2 (9.4f, 24.9f));
            map.Objects.Add (flower88);

            var flower89 = new Flower (this, 3, new Vector2 (9.4f, 25.9f));
            map.Objects.Add (flower89);

            var flower90 = new Flower (this, 1, new Vector2 (9.4f, 26.9f));
            map.Objects.Add (flower90);

            var flower91 = new Flower (this, 1, new Vector2 (10.4f, 24.9f));
            map.Objects.Add (flower91);

            var flower92 = new Flower (this, 3, new Vector2 (10.4f, 25.9f));
            map.Objects.Add (flower92);

            var flower93 = new Flower (this, 1, new Vector2 (10.4f, 26.9f));
            map.Objects.Add (flower93);

            var flower94 = new Flower (this, 1, new Vector2 (11.4f, 24.9f));
            map.Objects.Add (flower94);

            var flower95 = new Flower (this, 1, new Vector2 (11.4f, 25.9f));
            map.Objects.Add (flower95);

            var flower96 = new Flower (this, 1, new Vector2 (11.4f, 26.9f));
            map.Objects.Add (flower96);


            var farmer6 = new Farmer (this, new Vector2 (4.4f, 23.9f), new List<Flower> { flower71, flower72, flower73, flower74, flower75, flower76, flower77, flower78, flower79, flower80, flower81, flower82, flower83, flower84, flower85, flower86, flower87, flower88, flower89, flower90, flower91, flower92, flower93, flower94, flower95, flower96 });
            map.Objects.Add (farmer6);

            var farmer7 = new Farmer (this, new Vector2 (9.4f, 23.9f), new List<Flower> { flower71, flower72, flower73, flower74, flower75, flower76, flower77, flower78, flower79, flower80, flower81, flower82, flower83, flower84, flower85, flower86, flower87, flower88, flower89, flower90, flower91, flower92, flower93, flower94, flower95, flower96 });
            map.Objects.Add (farmer7);


            if (Baldur.KeycardCounter < 4)
            {
                var farmLeader = new FarmLeader(this, new Vector2(34.4f, 6.9f));
                map.Objects.Add(farmLeader);
            }

        }

        public void LoadLevel3Objekts(){
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            LoadDefaultObjekts(mapPath);   

            var map = this.World.Area;

            this.Baldur.Position = this.World.Area.GetTeleportPosition();
            map.Objects.Add(this.Baldur);


            var iron1 = new Iron (this, 1, new Vector2 (2.5f, 21.8f));
            map.Objects.Add (iron1);

            var iron2 = new Iron (this, 1, new Vector2 (3.5f, 21.8f));
            map.Objects.Add (iron2);

            var iron3 = new Iron (this, 1, new Vector2 (4.5f, 21.8f));
            map.Objects.Add (iron3);

            var iron4 = new Iron (this, 1, new Vector2 (2.5f, 22.8f));
            map.Objects.Add (iron4);

            var iron5 = new Iron (this, 1, new Vector2 (3.5f, 22.8f));
            map.Objects.Add (iron5);

            var iron6 = new Iron (this, 1, new Vector2 (4.5f, 22.8f));
            map.Objects.Add (iron6);

            var iron7 = new Iron (this, 1, new Vector2 (2.5f, 23.8f));
            map.Objects.Add (iron7);

            var iron8 = new Iron (this, 1, new Vector2 (3.5f, 23.8f));
            map.Objects.Add (iron8);

            var iron9 = new Iron (this, 1, new Vector2 (4.5f, 23.8f));
            map.Objects.Add (iron9);

            var miner1 = new Miner (this, new Vector2 (5.5f, 20.8f));
            map.Objects.Add (miner1);


            var iron11 = new Iron (this, 1, new Vector2 (18.5f, 10.8f));
            map.Objects.Add (iron11);

            var iron12 = new Iron (this, 1, new Vector2 (19.5f, 10.8f));
            map.Objects.Add (iron12);

            var iron13 = new Iron (this, 1, new Vector2 (20.5f, 10.8f));
            map.Objects.Add (iron13);

            var iron14 = new Iron (this, 1, new Vector2 (18.5f, 11.8f));
            map.Objects.Add (iron14);

            var iron15 = new Iron (this, 3, new Vector2 (19.5f, 11.8f));
            map.Objects.Add (iron15);

            var iron16 = new Iron (this, 1, new Vector2 (20.5f, 11.8f));
            map.Objects.Add (iron16);

            var iron17 = new Iron (this, 1, new Vector2 (18.5f, 12.8f));
            map.Objects.Add (iron17);

            var iron18 = new Iron (this, 3, new Vector2 (19.5f, 12.8f));
            map.Objects.Add (iron18);

            var iron19 = new Iron (this, 1, new Vector2 (20.5f, 12.8f));
            map.Objects.Add (iron19);

            var iron20 = new Iron (this, 1, new Vector2 (18.5f, 13.8f));
            map.Objects.Add (iron20);

            var iron21 = new Iron (this, 1, new Vector2 (19.5f, 13.8f));
            map.Objects.Add (iron21);

            var iron22 = new Iron (this, 1, new Vector2 (20.5f, 13.8f));
            map.Objects.Add (iron22);

            var miner2 = new Miner (this, new Vector2 (21.5f, 11.8f));
            map.Objects.Add (miner2);

            var miner3 = new Miner (this, new Vector2 (21.5f, 12.8f));
            map.Objects.Add (miner3);


            var iron25 = new Iron (this, 3, new Vector2 (28.5f, 16.8f));
            map.Objects.Add (iron22);


            var iron31 = new Iron (this, 1, new Vector2 (37.5f, 12.8f));
            map.Objects.Add (iron31);

            var iron32 = new Iron (this, 1, new Vector2 (38.5f, 12.8f));
            map.Objects.Add (iron32);

            var iron33 = new Iron (this, 1, new Vector2 (39.5f, 12.8f));
            map.Objects.Add (iron33);

            var iron34 = new Iron (this, 1, new Vector2 (37.5f, 13.8f));
            map.Objects.Add (iron34);

            var iron35 = new Iron (this, 3, new Vector2 (38.5f, 13.8f));
            map.Objects.Add (iron35);

            var iron36 = new Iron (this, 1, new Vector2 (39.5f, 13.8f));
            map.Objects.Add (iron36);

            var iron37 = new Iron (this, 1, new Vector2 (37.5f, 14.8f));
            map.Objects.Add (iron37);

            var iron38 = new Iron (this, 3, new Vector2 (38.5f, 14.8f));
            map.Objects.Add (iron38);

            var iron39 = new Iron (this, 1, new Vector2 (39.5f, 14.8f));
            map.Objects.Add (iron39);

            var iron40 = new Iron (this, 1, new Vector2 (37.5f, 15.8f));
            map.Objects.Add (iron40);

            var iron41 = new Iron (this, 1, new Vector2 (38.5f, 15.8f));
            map.Objects.Add (iron41);

            var iron42 = new Iron (this, 1, new Vector2 (39.5f, 15.8f));
            map.Objects.Add (iron42);

            var miner4 = new Miner (this, new Vector2 (36.5f, 13.8f));
            map.Objects.Add (miner4);

            var miner5 = new Miner (this, new Vector2 (36.5f, 14.8f));
            map.Objects.Add (miner5);


            var iron45 = new Iron (this, 3, new Vector2 (38.5f, 26.8f));
            map.Objects.Add (iron45);


            var iron51 = new Iron (this, 1, new Vector2 (44.5f, 14.8f));
            map.Objects.Add (iron51);

            var iron52 = new Iron (this, 1, new Vector2 (45.5f, 14.8f));
            map.Objects.Add (iron52);

            var iron53 = new Iron (this, 1, new Vector2 (46.5f, 14.8f));
            map.Objects.Add (iron53);

            var iron54 = new Iron (this, 1, new Vector2 (44.5f, 15.8f));
            map.Objects.Add (iron54);

            var iron55 = new Iron (this, 3, new Vector2 (45.5f, 15.8f));
            map.Objects.Add (iron55);

            var iron56 = new Iron (this, 1, new Vector2 (46.5f, 15.8f));
            map.Objects.Add (iron56);

            var iron57 = new Iron (this, 1, new Vector2 (44.5f, 16.8f));
            map.Objects.Add (iron57);

            var iron58 = new Iron (this, 1, new Vector2 (45.5f, 16.8f));
            map.Objects.Add (iron58);

            var iron59 = new Iron (this, 1, new Vector2 (46.5f, 16.8f));
            map.Objects.Add (iron59);

            var miner6 = new Miner (this, new Vector2 (47.5f, 15.8f));
            map.Objects.Add (miner6);


            if (Baldur.KeycardCounter < 5)
            {
                var farmLeader = new MineLeader(this, new Vector2(45.5f, 26.8f));
                map.Objects.Add(farmLeader);
            }
        }

        public void LoadLevel4Objekts(){
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            LoadDefaultObjekts(mapPath);   

            var map = this.World.Area;

            this.Baldur.Position = this.World.Area.GetTeleportPosition();
            map.Objects.Add(this.Baldur);


            var fighter1= new Fighter (this, new Vector2 (12.5f, 7.8f));
            map.Objects.Add (fighter1);

            var fighter2 = new Fighter (this, new Vector2 (13.5f, 7.8f));
            map.Objects.Add (fighter2);

            var fighter3 = new Fighter (this, new Vector2 (19.5f, 14.8f));
            map.Objects.Add (fighter3);

            var fighter4 = new Fighter (this, new Vector2 (19.5f, 14.8f));
            map.Objects.Add (fighter4);

            var fighter5 = new Fighter (this, new Vector2 (30.5f, 18.2f));
            map.Objects.Add (fighter5);

            var fighter6 = new Fighter (this, new Vector2 (31.5f, 18.2f));
            map.Objects.Add (fighter6);

            var fighter7 = new Fighter (this, new Vector2 (27.5f, 15.8f));
            map.Objects.Add (fighter7);

            var fighter8 = new Fighter (this, new Vector2 (28.5f, 5.8f));
            map.Objects.Add (fighter8);

            var fighter9 = new Fighter (this, new Vector2 (3.5f, 9.8f));
            map.Objects.Add (fighter9);

            var fighter10 = new Fighter (this, new Vector2 (35.5f, 11.2f));
            map.Objects.Add (fighter10);

            var fighter11 = new Fighter (this, new Vector2 (35.5f, 15.2f));
            map.Objects.Add (fighter11);


            if (Baldur.KeycardCounter < 6) //todo: keycard zeigen, wenn alle tot sind
            {
                var keycard = new Keycard (this, 6, new Vector2(29.5f, 7.25f));
                map.Objects.Add (keycard);
            }
        }

        public void LoadLevel5Objekts(){
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            LoadDefaultObjekts(mapPath);   

            var map = this.World.Area;

            this.Baldur.Position = this.World.Area.GetTeleportPosition();
            map.Objects.Add(this.Baldur);



            if (Baldur.KeycardCounter < 7)
            {

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

