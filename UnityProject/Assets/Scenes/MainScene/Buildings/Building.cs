using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public Tilemap tilemap;

    public Sprite sprite
    {
        get
        {
            return GetComponent<SpriteRenderer>().sprite;
        }
        set
        {
            GetComponent<SpriteRenderer>().sprite = value;

            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
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

    private Vector3Int _cellPos;

    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
    }
}
