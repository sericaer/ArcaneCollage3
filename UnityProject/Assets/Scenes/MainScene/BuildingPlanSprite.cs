using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildingPlanSprite : MonoBehaviour
{
    public Tilemap tilemap;
    public UnityEvent<Vector3Int> StartBuilding;

    public int x;
    public int y;

    public Func<bool> IsOverlapBuilding;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            return GetComponent<SpriteRenderer>();
        }
    }

    private Vector3Int cellPos
    {
        get
        {
            return _cellPos;
        }
        set
        {
            _cellPos = value;

            var pos = tilemap.CellToWorld(_cellPos);

            transform.position = new Vector3(pos.x, pos.y);

            isLegal = !IsOverlapBuilding();
        }
    }

    private bool isLegal
    {
        get
        {
            return _isLegal;
        }
        set
        {
            _isLegal = value;

            spriteRenderer.color = _isLegal ? Color.green : Color.red;
        }
    }

    private Vector3Int _cellPos;
    private bool _isLegal;

    // Start is called before the first frame update
    void Start()
    {
        if(IsOverlapBuilding == null)
        {
            IsOverlapBuilding = DefaultIsOverlapBuilding;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void OnMouseDown()
    {
        if (!isLegal)
        {
            return;
        }

        StartBuilding.Invoke(cellPos);
    }

    private bool DefaultIsOverlapBuilding()
    {
        for(int i=0; i<x; i++)
        {
            for(int j=0; j<y; j++)
            {
                if(tilemap.HasTile(cellPos + new Vector3Int(i,j)))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
