using System;

namespace BaldurSuchtFiona.Models
{
	public class Layer
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
        public int[,] Tiles { get; private set; }

        public Layer(int width, int height)
		{
			Width = width;
			Height = height;
			Tiles = new int[width, height];
		}
	}
}

