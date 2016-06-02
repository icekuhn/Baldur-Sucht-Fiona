using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models

{
    public class Area
        {
        public string Name { get; set; }

        public Color Background { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public List<Objekt> Objects { get; private set; }

        public List<Item> Items { get; private set; }   //zu viel

        public List<Player> Players { get; private set; }   //zu viel

        public Layer[] Layers { get; private set; }

        public Dictionary<int,Tile> Tiles;

        public Area()
        {
            Width = 35;
            Height = 20;

            Layers = new Layer[1];
            for (int l = 0; l < 1; l++)
                Layers[l] = new Layer(35, 20);

            Objects = new List<Objekt>();
            Tiles = new Dictionary<int, Tile>();
        }

        public Area(int layers, int width, int height)
    	{
            Width = width;
            Height = height;
            //Items = new List<Item>();
            Objects = new List<Objekt>();
            Players = new List<Player>();

            Layers = new Layer[layers];
            for (int l = 0; l < layers; l++)
            {
                Layers[l] = new Layer(width, height);
            }

            Tiles = new Dictionary<int,Tile>();
            Objects = new List<Objekt>();
        }

        public Area(FileArea area)
        {
            Objects = new List<Objekt>();
            Tiles = new Dictionary<int, Tile>();
            Layers = new Layer[area.layers.Length];
            for (int l = 0; l < area.layers.Length; l++)
                Layers[l] = new Layer(area.layers[l].name,area.width, area.height);
            Width = area.width;
            Height = area.height;
            Background = new Color(0, 0, 0);
            if (!string.IsNullOrEmpty(area.backgroundcolor))
            {
                // Hexwerte als Farbwert parsen
                Background = new Color(
                    Convert.ToInt32(area.backgroundcolor.Substring(1, 2), 16),
                    Convert.ToInt32(area.backgroundcolor.Substring(3, 2), 16),
                    Convert.ToInt32(area.backgroundcolor.Substring(5, 2), 16));
            }
            for (int i = 0; i < area.tilesets.Length; i++)
            {
                FileTileset tileset = area.tilesets[i];

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
                    Tiles.Add(start + j, tile);
                }
            }

            for (int l = 0; l < area.layers.Length; l++)
            {
                FileLayer layer = area.layers[l];

                for (int i = 0; i < layer.data.Length; i++)
                {
                    int x = i % Width;
                    int y = i / Width;
                    Layers[l].Tiles[x, y] = layer.data[i];
                }
            }
        }

        public bool IsCellBlocked(int x, int y)
        {
            for (int l = 0; l < Layers.Length; l++)
            {
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.Blocked)
                    return true;
            }
            return false;
        }
    }
}

