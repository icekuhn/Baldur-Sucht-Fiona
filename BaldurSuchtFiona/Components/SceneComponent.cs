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

        public void SetContent(Dictionary<string, Texture2D> _tilesetTextures,Dictionary<string, Texture2D> _objektTextures){
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Camera = new Camera(GraphicsDevice.Viewport.Bounds.Size);
            tilesetTextures = _tilesetTextures;
            objektTextures = _objektTextures;
        }

        public GraphicsDevice GetGraphicsDevice(){
            return GraphicsDevice;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Camera = new Camera(GraphicsDevice.Viewport.Bounds.Size);

            game.LoadBaseObjekts();
        }

        public override void Update(GameTime gameTime)
        {
            if(game.World.Area != null){
                Vector2 areaSize = new Vector2(game.World.Area.Width, game.World.Area.Height); 
                Camera.SetFocus(game.Baldur.Position, areaSize);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Area area = game.World.Area;
            if (area == null)
                return;
            GraphicsDevice.Clear(area.Background);

            spriteBatch.Begin();

            Point offset = (Camera.Offset * Camera.Scale).ToPoint();

            for (int l = 0; l < area.Layers.Length; l++) {
                    RenderLayer(area, area.Layers[l], offset);
                if (l == 4)
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
            foreach (var objekt in area.Objects.OrderBy(o => o.Position.Y))
            {
                // Renderer ermitteln
                ObjektRenderer renderer;
                if (!objektRenderer.TryGetValue(objekt, out renderer))
                {
                    // Texturen nachladen beachten
                    var texture = objektTextures[objekt.TextureName];

                    if(objekt is Character) {
                        if (objekt is Player)
                        {
                            var player = objekt as Player;
                            renderer = new CharacterRenderer(objekt as Character, Camera, texture, objektTextures[player.AttackTextureName]);
                        }else if (objekt is Enemy)
                        {
                            var enemy = objekt as Enemy;
                            renderer = new CharacterRenderer(objekt as Character, Camera, texture,objektTextures[enemy.AttackTextureName]);
                        }
                        else
                        {
                            renderer = new CharacterRenderer(objekt as Character, Camera, texture);                            
                        }
                    } else {
                        renderer = new SimpleObjektRenderer(objekt, Camera, texture);
                    }
                    objektRenderer.Add(objekt, renderer);
                }

                renderer.Draw(spriteBatch, offset, gameTime);
            }
        }

        public void ClearRenderer(){
            objektRenderer.Clear();
        }

        public void RemoveBaldurFromRenderer(){
            if (objektRenderer.ContainsKey (game.Baldur))
                objektRenderer.Remove(game.Baldur);
        }
    }
}