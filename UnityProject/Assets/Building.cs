using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public Tilemap tilemap;
    internal Vector3Int cellPos
    {
        get
        {
            return _cellPos;
        }
        set
        {
            _cellPos = value;

            var pos = tilemap.CellToWorld(cellPos);
            transform.position = new Vector3(pos.x, pos.y);
        }
    }

    private Vector3Int _cellPos;
}
