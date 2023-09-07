using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Playniax.Ignition
{
    public class TilemapHelpers
    {
        public static TileBase CreateTile(Sprite sprite)
        {
            var tile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
            tile.sprite = sprite;

            return tile;
        }
        public static List<Bounds> GetBounds(Tilemap tilemap)
        {
            if (tilemap == null) return null;

            tilemap.CompressBounds();

            BoundsInt bounds = tilemap.cellBounds;

            var positions = bounds.allPositionsWithin;

            var list = new List<Bounds>();

            foreach (var position in positions)
            {
                var sprite = tilemap.GetSprite(position);
                if (sprite)
                {
                    var tileBounds = new Bounds(tilemap.CellToWorld(position) + Vector3.Scale(tilemap.cellSize, tilemap.tileAnchor), tilemap.cellSize);

                    list.Add(tileBounds);
                }
            }

            return list;
        }
    }
}