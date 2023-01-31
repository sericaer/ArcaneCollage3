using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour
{
    public Tilemap tilemap;

    public Sprite sprite;

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

    void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = sprite.bounds.size;
        boxCollider2D.offset = boxCollider2D.size / 2;
    }

    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
    }
}
