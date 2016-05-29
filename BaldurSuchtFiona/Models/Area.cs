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

        public List<Item> Items { get; private set; }

        public List<Player> Players { get; private set; }

        public Layer[] Layers { get; private set; }

        public Dictionary<int,Tile> Tiles;

        public Area(int layers, int width, int height)
    	{
            Width = width;
            Height = height;
            Items = new List<Item>();
            Players = new List<Player>();

            Layers = new Layer[layers];
            for (int l = 0; l < layers; l++)
            {
                Layers[l] = new Layer(width, height);
            }

            Tiles = new Dictionary<int,Tile>();
            //Objects = new List<Object>();
        }

        // isCellBocked: siehe 5.4 letzter Teil
    }
}

