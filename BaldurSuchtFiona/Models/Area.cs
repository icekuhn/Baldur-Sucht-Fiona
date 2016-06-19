using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace BaldurSuchtFiona.Models

{
    public class Area
    {
        public string Name { get; set; }

        public string SongName { get; set; }

        public Color Background { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public List<Objekt> Objects { get; private set; }

        public List<Item> Items { get { return Objects.Where(o => o is Item) as List<Item>; }  }  

        public List<Player> Players { get { return Objects.Where(o => o is Player) as List<Player>; }  } 

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
            Objects = new List<Objekt>();

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
                    bool teleporter = false;
                    bool safeZone = false;
                    bool isInteractable = false;
                    string actionId = null;
                    if (tileset.tileproperties != null)
                    {
                        FileTileProperty property;
                        if (tileset.tileproperties.TryGetValue(j, out property))
                        {
                            block = property.blocked;
                            teleporter = property.teleporter;
                            safeZone = property.safeZone;
                            isInteractable = property.isInteractable;
                            actionId = property.actionId;
                        }
                    }

                    Tile tile = new Tile()
                        { 
                            Texture = tileset.image,
                            SourceRectangle = new Rectangle(x * width, y * width, width, width),
                            Blocked = block,
                            Teleporter = teleporter,
                            SafeZone = safeZone,
                            IsInteractable = isInteractable,
                            ActionId = actionId
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
                var firstDimensionLength = Layers [l].Tiles.GetLength (0);
                var secondDimensionLength = Layers [l].Tiles.GetLength (1);
                if (x < 0 || y < 0 || x >= firstDimensionLength || y >= secondDimensionLength)
                    return true;
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.Blocked)
                    return true;
            }
            return false;
        }

        public bool IsTeleporter(int x, int y)
        {
            for (int l = 0; l < Layers.Length; l++)
            {
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.Teleporter)
                    return true;
            }
            return false;
        }

        public bool IsSafeZone(int x, int y)
        {
            for (int l = 0; l < Layers.Length; l++)
            {
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.SafeZone)
                    return true;
            }
            return false;
        }

        public bool IsInteractable(int x, int y)
        {
            for (int l = 0; l < Layers.Length; l++)
            {
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.IsInteractable)
                    return true;
            }
            return false;
        }

        public bool IsPotionStation(int x, int y)
        {
            for (int l = 0; l < Layers.Length; l++)
            {
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.ActionId != null)
                {
                    if (tile.ActionId == "potionStation")
                        return true;
                }
            }
            return false;
        }

        public bool IsWorkbench(int x, int y)
        {
            for (int l = 0; l < Layers.Length; l++)
            {
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.ActionId != null)
                {
                    if (tile.ActionId == "workbench")
                        return true;
                }
            }
            return false;
        }

        public bool IsBed(int x, int y)
        {
            for (int l = 0; l < Layers.Length; l++)
            {
                int tileId = Layers[l].Tiles[x, y];
                if (tileId == 0)
                    continue;

                Tile tile = Tiles[tileId];

                if (tile.ActionId != null)
                {
                    if (tile.ActionId == "bed")
                        return true;
                }
            }
            return false;
        }

        public Vector2 GetTeleportPosition()
        {
            var vector = new Vector2();
            foreach (var layer in this.Layers)
            {
                for (var i = 0; i < Width; i++)
                {
                    for (var j = 0; j < Height; j++)
                    {
                        var tileId = layer.Tiles[i, j];
                        if (tileId == 0)
                            continue;
                        var tile = this.Tiles[tileId];
                        if (tile != null)
                        {
                            if (tile.Teleporter)
                            {
                                vector = new Vector2(i,j);
                            }
                        }
                    }
                }
            }
            return vector;
        }

        public Vector2 GetPotStationDistancePosition(Baldur baldur)
        {
            var potStationDistance = Vector2.Zero;
            var vector = new Vector2 ();

            foreach (var layer in this.Layers)
            {
                for (var i = 0; i < Width; i++)
                {
                    for (var j = 0; j < Height; j++)
                    {
                        var tileId = layer.Tiles[i, j];
                        if (tileId == 0)
                            continue;
                        var tile = this.Tiles[tileId];
                        if (tile != null)
                        {
                            if (tile.IsInteractable)
                            {
                                if (tile.ActionId == "potionStation") {
                                    if (potStationDistance == Vector2.Zero) {
                                        vector = new Vector2(i,j);
                                        potStationDistance = vector - baldur.Position;
                                    } else {
                                        vector = new Vector2(i,j);
                                        var currentPotStationDistance = vector - baldur.Position;
                                        if (currentPotStationDistance.Length () < potStationDistance.Length ())
                                            potStationDistance = currentPotStationDistance;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return potStationDistance;
        }

        public Vector2 GetWorkBenchDistancePosition(Baldur baldur)
        {
            var workBenchDistance = Vector2.Zero;
            var vector = new Vector2 ();

            foreach (var layer in this.Layers)
            {
                for (var i = 0; i < Width; i++)
                {
                    for (var j = 0; j < Height; j++)
                    {
                        var tileId = layer.Tiles[i, j];
                        if (tileId == 0)
                            continue;
                        var tile = this.Tiles[tileId];
                        if (tile != null)
                        {
                            if (tile.IsInteractable)
                            {
                                if (tile.ActionId == "workbench") {
                                    if (workBenchDistance == Vector2.Zero) {
                                        vector = new Vector2(i,j);
                                        workBenchDistance = vector - baldur.Position;
                                    } else {
                                        vector = new Vector2(i,j);
                                        var currentWorkBenchDistance = vector - baldur.Position;
                                        if (currentWorkBenchDistance.Length () < workBenchDistance.Length ())
                                            workBenchDistance = currentWorkBenchDistance;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return workBenchDistance;
        }

        public Vector2 GetBedDistancePosition(Baldur baldur)
        {
            var bedDistance = Vector2.Zero;
            var vector = new Vector2 ();

            foreach (var layer in this.Layers)
            {
                for (var i = 0; i < Width; i++)
                {
                    for (var j = 0; j < Height; j++)
                    {
                        var tileId = layer.Tiles[i, j];
                        if (tileId == 0)
                            continue;
                        var tile = this.Tiles[tileId];
                        if (tile != null)
                        {
                            if (tile.IsInteractable)
                            {
                                if (tile.ActionId == "bed") {
                                    if (bedDistance == Vector2.Zero) {
                                        vector = new Vector2(i,j);
                                        bedDistance = vector - baldur.Position;
                                    } else {
                                        vector = new Vector2(i,j);
                                        var currentBedDistance = vector - baldur.Position;
                                        if (currentBedDistance.Length () < bedDistance.Length ())
                                            bedDistance = currentBedDistance;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return bedDistance;
        }
    }
}

