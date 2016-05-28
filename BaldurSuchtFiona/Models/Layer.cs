using System;

namespace BaldurSuchtFiona
{
	public class Layer
	{
        public string Name { get; set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
        public int[,] Tiles { get; private set; }

        public Layer(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new int[width, height];
        }

        public Layer(string name,int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
            Tiles = new int[width, height];
        }
	}
}

