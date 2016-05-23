using System;
using System.Collections.Generic;

namespace BaldurSuchtFiona
{
	public class FileArea
	{
		public string backgroundcolor { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public FileLayer[] layers { get; set; }
		public FileTileset[] tilesets { get; set; }
	}

	public class FileLayer
	{
		public int[] data { get; set; }
	}

	public class FileTileset
	{
		public int firstgid { get; set; }
		public string image { get; set; }
		public int tilewidth { get; set; }
		public int imagewidth { get; set; }
		public int tilecount { get; set; }
		public Dictionary<int, FileTileProperty> tileproperties { get; set; }
	}
		
	public class FileTileProperty
	{
		public bool Block { get; set; }
	}
}

