using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
public class BuildingPlanRender : MonoBehaviour
{
    public Grid grid;
    public int x;
    public int y;

    public UnityEvent<Vector3Int> StartBuilding;

    public Func<bool> CheckLegeal;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            return GetComponent<SpriteRenderer>();
        }
    }

    public Vector3Int cellPos
    {
        get
        {
            return _cellPos;
        }
        set
        {
            _cellPos = value;

            var pos = grid.CellToWorld(_cellPos);

            transform.position = new Vector3(pos.x, pos.y);

            isLegal = CheckLegeal();
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
        if(CheckLegeal == null)
        {
            CheckLegeal = ()=> true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            cellPos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
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
}
