using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : MonoBehaviour
{
    public BuildingPlan buildingPlanPrototype;
    public Building buildingPrototype;

    public Tilemap tilemap;

    public List<Building> buildings;

    private Sprite[] sprites;

    private void Awake()
    {
        sprites = Directory.EnumerateFiles(Application.streamingAssetsPath, "*.png")
            .Select(imgPath =>
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(imgPath));
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 64);
                return sprite;
            }).ToArray();
    }
    // Start is called before the first frame update
    void Start()
    {
        buildings = new List<Building>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNewBuilding()
    {
        var buildingPlan = Instantiate<BuildingPlan>(buildingPlanPrototype);
        buildingPlan.tilemap = tilemap;
        buildingPlan.sprite = sprites[0];

        buildingPlan.confirmLocalEvent.AddListener((pos) =>
        {
            Debug.Log($"{buildingPlan.GetComponent<SpriteRenderer>().color }");
            if (buildingPlan.GetComponent<SpriteRenderer>().color == Color.green)
            {
                Destroy(buildingPlan.gameObject);

                var building = Instantiate<Building>(buildingPrototype);
                building.tilemap = tilemap;
                building.cellPos = pos;
                building.sprite = sprites[0];

                buildings.Add(building);
            }
        });
    }
}
