using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BuildingPlanSprite : MonoBehaviour
{
    public Tilemap tilemap;

    public SpriteRenderer spriteRenderer;

    public int xLength;
    public int yLength;

    public UnityEvent<Vector3Int> StartBuilding;

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

            isLegal = !OverlapBuilding();
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

    private Vector3Int _cellPos;

    // Start is called before the first frame update
    void Start()
    {
        cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private bool OverlapBuilding()
    {
        for(int x=0; x<xLength; x++)
        {
            for(int y=0; y<yLength; y++)
            {
                if(tilemap.HasTile(cellPos + new Vector3Int(x, y)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void OnMouseDown()
    {
        if (isLegal)
        {
            StartBuilding.Invoke(cellPos);
        }
    }
}