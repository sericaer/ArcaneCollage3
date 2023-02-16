using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MainScene : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileset;

    public Button test;
    public GameObject des;

    private void Start()
    {
        test.onClick.AddListener(() =>
            Instantiate(des, des.transform.parent)
        );
        for (int i=-50; i<=50; i++)
        {
            for(int j=-50; j<=50; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j), tileset);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
