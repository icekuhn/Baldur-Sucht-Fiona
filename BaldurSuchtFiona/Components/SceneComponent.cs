using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using BaldurSuchtFiona.Rendering;
using System.IO;
using BaldurSuchtFiona.Models;
using System.Linq;

namespace BaldurSuchtFiona.Components
{
    public class SceneComponent : DrawableGameComponent
    {
        private readonly Game1 game;

        private SpriteBatch spriteBatch;
        private Texture2D pixel;
        private Dictionary<string, Texture2D> tilesetTextures;
        private Dictionary<string, Texture2D> objektTextures;
        private Dictionary<Objekt, ObjektRenderer> objektRenderer;

        public Camera Camera { get; private set; }

        public SceneComponent(Game1 game) : base(game)
        {
            this.game = game;
            this.tilesetTextures = new Dictionary<string, Texture2D>();
            objektTextures = new Dictionary<string, Texture2D>();
            objektRenderer = new Dictionary<Objekt, ObjektRenderer>();
        }

        public void ReloadContent(){
            LoadContent();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Camera = new Camera(GraphicsDevice.Viewport.Bounds.Size);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new [] { Color.White });


            List<string> requiredTilesetTextures = new List<string>();
            List<string> requiredObjektTextures = new List<string>();

                // Tile Texturen
            foreach (var tile in game.Simulation.World.Area.Tiles.Values)
                    if (!requiredTilesetTextures.Contains(tile.Texture))
                        requiredTilesetTextures.Add(tile.Texture);

                // Objekt Texturen
            requiredObjektTextures.Add("sprite_player_3.png");
            requiredObjektTextures.Add("sprite_farmer.png");
            requiredObjektTextures.Add("collectables.png");
            foreach (var objekt in game.Simulation.World.Area.Objects)
                if (!string.IsNullOrEmpty(objekt.Texture) && !requiredObjektTextures.Contains(objekt.Texture))
                    requiredObjektTextures.Add(objekt.Texture);
            

          

            // Tileset Texturen laden
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            tilesetTextures.Clear();
            foreach (var textureName in requiredTilesetTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(GraphicsDevice, stream);
                    tilesetTextures.Add(textureName, texture);
                }
            }            
            var map = game.Simulation.World.Area;


            // Objekt Texturen laden
            mapPath = Path.Combine(Environment.CurrentDirectory, "Content");
            objektTextures.Clear();
            foreach (var textureName in requiredObjektTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(GraphicsDevice, stream);
                    objektTextures.Add(textureName, texture);
                }
            }     


            game.Simulation.Baldur = new Baldur(game,new Vector2(15, 12));
            map.Objects.Add(game.Simulation.Baldur);

//            var iron1 = new Iron(game,1,new Vector2(18, 15));
//            map.Objects.Add(iron1);
//
//            var iron2 = new Iron(game,2,new Vector2(13, 17));
//            map.Objects.Add(iron2);
//
//            var iron3 = new Iron(game,3,new Vector2(19, 19));
//            map.Objects.Add(iron3);
//
//            var flower1 = new Flower(game,1,new Vector2(19, 16));
//            map.Objects.Add(flower1);
//
//            var flower2 = new Flower(game,2,new Vector2(14, 14));
//            map.Objects.Add(flower2);
//
            var flower3 = new Flower(game,3,new Vector2(15,21));
            map.Objects.Add(flower3);

            var keycard1 = new Keycard(game,1,new Vector2(13, 18));
            map.Objects.Add(keycard1);

            var farmer1 = new Farmer(game,new Vector2(14, 23));
            map.Objects.Add(farmer1);

            var farmer2 = new Farmer(game,new Vector2(16, 23));
            map.Objects.Add(farmer2);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 areaSize = new Vector2(game.Simulation.World.Area.Width, game.Simulation.World.Area.Height); 
            Camera.SetFocus(game.Simulation.Baldur.Position, areaSize);
        }

        public override void Draw(GameTime gameTime)
        {
            Area area = game.Simulation.World.Area;

            GraphicsDevice.Clear(area.Background);

            spriteBatch.Begin();

            Point offset = (Camera.Offset * Camera.Scale).ToPoint();

            for (int l = 0; l < area.Layers.Length; l++) {
                    RenderLayer(area, area.Layers[l], offset);
                if (l == 3)
                {
                    RendererObjekts(area, offset, gameTime);
                }
            }

            spriteBatch.End();

        }

        private void RenderLayer(Area area, Layer layer, Point offset)
        {
            for (int x = 0; x < area.Width; x++)
            {
                for (int y = 0; y < area.Height; y++)
                {
                    int tileId = layer.Tiles[x, y];
                    if (tileId == 0)
                        continue;

                    Tile tile = area.Tiles[tileId];
                    Texture2D texture = tilesetTextures[tile.Texture];

                    int offsetX = (int)(x * Camera.Scale) - offset.X;
                    int offsetY = (int)(y * Camera.Scale) - offset.Y;

                    spriteBatch.Draw(texture, new Rectangle(offsetX, offsetY, (int)Camera.Scale, (int)Camera.Scale), tile.SourceRectangle, Color.White);
                }
            }
        }

        private void RendererObjekts(Area area, Point offset, GameTime gameTime)
        {
          //  foreach (var charac in area.Objects)
          //  {
              //  Color color = Color.White;

                // Positionsermittlung und Ausgabe des Spielelements.
             //   int posX = (int)((charac.Position.X - charac.Radius) * Camera.Scale) - offset.X;
             //   int posY = (int)((charac.Position.Y - charac.Radius) * Camera.Scale) - offset.Y;
             //   int size = (int)((charac.Radius) * Camera.Scale);
             //   if(charac.DrawAll)
             //       spriteBatch.Draw(charac.Texture, new Rectangle(posX, posY, size*4, size*4), color);
             //   else
              //      spriteBatch.Draw(charac.Texture, new Rectangle(posX, posY, size, size), new Rectangle(charac.DrawX, charac.DrawY, charac.DrawWidth, charac.DrawHeight), color);

            // Objekte in Reihenfolge rendern
            // foreach (var objekt in area.Objects.OrderBy(i => i.Position.Y))
               foreach (var objekt in area.Objects)
            {
                // Renderer ermitteln
                ObjektRenderer renderer;
                if (!objektRenderer.TryGetValue(objekt, out renderer))
                {
                    // Texturen nachladen beachten
                    var texture = objektTextures[objekt.TextureName];

                        if(objekt is Character) {
                            renderer = new CharacterRenderer(objekt as Character, Camera, texture);
                        } else {
                            renderer = new SimpleObjektRenderer(objekt, Camera, texture);
                        }
                            objektRenderer.Add(objekt, renderer);
                }

                renderer.Draw(spriteBatch, offset, gameTime);
            }
        }
    }
}