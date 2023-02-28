using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
class RoadTilemap : MonoBehaviour
{
    public void SetTile(Vector3Int pos)
    {
        GetComponent<Tilemap>().SetTileColor(pos, Color.gray);
    }

    public bool HasTile(Vector3Int pos)
    {
        return GetComponent<Tilemap>().HasTile(pos);
    }
}