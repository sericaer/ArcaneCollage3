using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : MonoBehaviour
{
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            tilemap.ClearAllTiles();

            var newCellPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            for(int i=-1; i<=1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var pos = new Vector3Int(newCellPos.x + i, newCellPos.y + j);
                    tilemap.SetTileColor(pos, Color.white);
                }
            }

        }
    }
}
