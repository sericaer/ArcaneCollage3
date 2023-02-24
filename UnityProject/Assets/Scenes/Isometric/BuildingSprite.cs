using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSprite : MonoBehaviour
{
    public Tilemap tilemap;
    public SpriteRenderer spriteRenderer;
    public int xLength;
    public int yLength;

    public Vector3Int cellPos
    {
        get
        {
            return _cellPos;
        }
        set
        {
            var pos = tilemap.GetCellCenterWorld(cellPos);
            transform.position = new Vector3(pos.x, pos.y);

            _cellPos = value;

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    tilemap.SetTileColor(_cellPos + new Vector3Int(x, y), Color.red);
                }
            }
        }
    }

    private Vector3Int _cellPos;
}
