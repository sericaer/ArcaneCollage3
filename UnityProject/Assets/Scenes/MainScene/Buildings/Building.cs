using Mods.Defines;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public Tilemap tilemap;

    public BuildingDefine def
    {
        get
        {
            return _def;
        }
        set
        {
            _def = value;


            var sprite = StreamingResources.sprites[def.image];
            GetComponent<SpriteRenderer>().sprite = sprite;

            var boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.size = sprite.bounds.size;
            boxCollider2D.offset = boxCollider2D.size / 2;
        }
    }

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

    private BuildingDefine _def;
    private Vector3Int _cellPos;

    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
    }
}
