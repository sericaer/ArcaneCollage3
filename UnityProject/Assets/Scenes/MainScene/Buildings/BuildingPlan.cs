using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BuildingPlan : MonoBehaviour
{
    public Building buildingPrototype;

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

            var boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.size = sprite.bounds.size;
            boxCollider2D.offset = boxCollider2D.size / 2;
        }
    }

    public bool isLegal
    {
        get
        {
            return GetComponent<SpriteRenderer>().color == Color.green;
        }
        set
        {
            GetComponent<SpriteRenderer>().color = value ? Color.green : Color.red;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isLegal = !OverlapBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            var cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            var pos = tilemap.CellToWorld(cellPos);

            transform.position = new Vector3(pos.x, pos.y);

            isLegal = !OverlapBuilding();
        }
    }

    private bool OverlapBuilding()
    {
        var fliter = new ContactFilter2D();
        fliter.useTriggers = true;
        fliter.SetLayerMask(1<<LayerMask.NameToLayer("Buildings"));

        var list = new List<Collider2D>();
        var count = GetComponent<BoxCollider2D>().OverlapCollider(fliter, list);
        return count != 0;
    }

    void OnMouseDown()
    {
        if (isLegal)
        {
            var cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            var building = Instantiate<Building>(buildingPrototype);
            building.tilemap = tilemap;
            building.cellPos = cellPos;
            building.sprite = sprite;

            Destroy(gameObject);
        }
    }
}