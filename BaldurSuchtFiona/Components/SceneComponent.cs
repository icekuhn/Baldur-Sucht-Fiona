using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace BaldurSuchtFiona.Components
{
    internal class SceneComponent : DrawableGameComponent
    {
        private readonly Game1 game;

        private Dictionary<string, Texture2D> textures;

        private Texture2D Baldur;

        private SpriteBatch spriteBatch;

        public SceneComponent(Game1 game)
            : base(game)
        {
            this.game = game;
            textures = new Dictionary<string, Texture2D>();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Vector2 areaSize = new Vector2(
                game.Simulation.World.Areas[0].Width,
                game.Simulation.World.Areas[0].Height);

            List<string> requiredTilesetTextures = new List<string>();
            foreach (var area in game.Simulation.World.Areas)
            {
                foreach (var tile in area.Tiles.Values)
                    if (!requiredTilesetTextures.Contains(tile.Texture))
                        requiredTilesetTextures.Add(tile.Texture);

            }
            string mapPath = Path.Combine(Environment.CurrentDirectory, "Maps");
            foreach (var textureName in requiredTilesetTextures)
            {
                using (Stream stream = File.OpenRead(mapPath + "\\" + textureName))
                {
                    Texture2D texture = Texture2D.FromStream(GraphicsDevice, stream);
                    textures.Add(textureName, texture);
                }
            }
            Baldur = game.Content.Load<Texture2D>("Character_Armor_front");
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 areaSize = new Vector2(
                game.Simulation.World.Areas[0].Width,
                game.Simulation.World.Areas[0].Height);
        }

        public override void Draw(GameTime gameTime)
        {
            Area area = game.Simulation.World.Areas[0];

            GraphicsDevice.Clear(area.Background);

            float scaleX = (GraphicsDevice.Viewport.Width - 20) / area.Width;
            float scaleY = (GraphicsDevice.Viewport.Height - 20) / area.Height;

            spriteBatch.Begin();

            for (int l = 0; l < area.Layers.Length; l++)
            {
                RenderLayer(area, area.Layers[l], scaleX, scaleY);
            }
            spriteBatch.Draw (Baldur, game.Simulation.Baldur.Position, Color.White);
            spriteBatch.End();
        }
            
        private void RenderLayer(Area area, Layer layer, float scaleX, float scaleY)
        {
            for (int x = 0; x < area.Width; x++)
            {
                for (int y = 0; y < area.Height; y++)
                {
                    int tileId = layer.Tiles[x, y];
                    if (tileId == 0)
                        continue;

                    Tile tile = area.Tiles[tileId];
                    Texture2D texture = textures[tile.Texture];

                    int offsetX = (int)(x * scaleX) + 10;
                    int offsetY = (int)(y * scaleY) + 10;

                    spriteBatch.Draw(texture, new Rectangle(offsetX, offsetY, (int)scaleX, (int)scaleY), tile.SourceRectangle, Color.White);
                }
            }
        }
    }
}