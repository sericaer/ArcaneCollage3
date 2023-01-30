using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : MonoBehaviour
{
    public Tilemap tilemap;
    public SpriteRenderer spriteRenderer;
    private bool isMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //    isMove = true;
        //if (Input.GetMouseButtonUp(0))
        //    isMove = false;

        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            var cellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            var pos = tilemap.CellToWorld(cellPos);

            spriteRenderer.gameObject.transform.position = new Vector3(pos.x, pos.y);
            spriteRenderer.color = Color.red;

            //spriteRenderer.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //tilemap.ClearAllTiles();

            //var newCellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //for (int i = -1; i <= 1; i++)
            //{
            //    for (int j = -1; j <= 1; j++)
            //    {
            //        var pos = new Vector3Int(newCellPos.x + i, newCellPos.y + j);
            //        tilemap.SetTileColor(pos, Color.white);
            //    }
            //}
        }

        if (Input.GetMouseButtonDown(0))
        {
            spriteRenderer.color = Color.white;
            Debug.Log($"mousePosition{Input.mousePosition}, worldPosition{Camera.main.ScreenToWorldPoint(Input.mousePosition)}");
        }
    }
}

//public class BuildPlanMap : MonoBehaviour
//{
//    public Tilemap tilemap;

//    public BuildPlanItem buildPlanItem;

//    void Update()
//    {
//        if(buildPlanItem == null)
//        {
//            return;
//        }

//        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
//        {
//            tilemap.ClearAllTiles();

//            var newCellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
//            for (int i = -1; i <= 1; i++)
//            {
//                for (int j = -1; j <= 1; j++)
//                {
//                    var pos = new Vector3Int(newCellPos.x + i, newCellPos.y + j);
//                    tilemap.SetTileColor(pos, Color.white);
//                }
//            }

//        }
//    }
//}

//public class BuildPlanItem
//{
//    public int width;
//    public int height;
//}