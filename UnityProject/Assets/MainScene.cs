using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : MonoBehaviour
{
    public BuildingPlan buildingPlanPrototype;
    public Building buildingPrototype;

    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNewBuilding()
    {
        var buildingPlan = Instantiate<BuildingPlan>(buildingPlanPrototype);
        buildingPlan.tilemap = tilemap;

        buildingPlan.confirmLocalEvent.AddListener((pos) =>
        {
            Destroy(buildingPlan.gameObject);

            var building = Instantiate<Building>(buildingPrototype);
            building.tilemap = tilemap;
            building.cellPos = pos;
        });
    }
}
