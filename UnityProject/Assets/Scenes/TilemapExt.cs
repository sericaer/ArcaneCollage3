using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExt
{
    public static Tile tile
    {
        get
        {
            if (_tile == null)
            {
                _tile = ScriptableObject.CreateInstance<Tile>();
                _tile.sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 0.5f, 0.5f), new Vector2(0.5f, 0.5f), 1);
            }

            return _tile;
        }
    }

    private static Tile _tile;

    public static void SetTileColor(this Tilemap tilemap, Vector3Int pos, Color color)
    {
        tilemap.SetTile(pos, tile);
        tilemap.SetTileFlags(pos, TileFlags.None);
        tilemap.SetColor(pos, color);
    }
}