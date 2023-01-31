using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BuildingPlan : MonoBehaviour
{
    public Tilemap tilemap;
    public UnityEvent<Vector3Int> confirmLocalEvent;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = OverlapBuilding() ? Color.red : Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            var cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            var pos = tilemap.CellToWorld(cellPos);

            transform.position = new Vector3(pos.x, pos.y);

            spriteRenderer.color = OverlapBuilding() ? Color.red : Color.green;
        }
    }

    private bool OverlapBuilding()
    {
        var fliter = new ContactFilter2D();
        fliter.NoFilter();

        var list = new List<Collider2D>();
        var count = GetComponent<BoxCollider2D>().OverlapCollider(fliter, list);
        return count != 0;
    }

    void OnMouseDown()
    {
        var cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        confirmLocalEvent.Invoke(cellPos);
    }
}