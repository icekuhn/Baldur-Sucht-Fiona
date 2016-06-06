using System;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona.Models
{
	public class Tile
	{        
		public string Texture { get; set; }
		public Rectangle SourceRectangle { get; set; }
        public bool Blocked { get; set; }
        public bool Teleporter { get; set; }
        public bool SafeZone { get; set; }

		public Tile ()
		{
		}
	}
}

