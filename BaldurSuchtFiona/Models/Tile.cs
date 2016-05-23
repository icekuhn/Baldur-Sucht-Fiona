using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona
{
	public class Tile
	{        
		public string Texture { get; set; }
		public Rectangle SourceRectangle { get; set; }
		public bool Blocked { get; set; }

		public Tile ()
		{
		}
	}
}

