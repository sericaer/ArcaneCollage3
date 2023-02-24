using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    public Tilemap tilemap;

    public BuildingPlanSprite defaultPlan;
    public BuildingSprite defaultBuilding;

    // Start is called before the first frame update
    void Awake()
    {
        defaultBuilding.gameObject.SetActive(false);
        defaultPlan.gameObject.SetActive(false);
    }

    void Start()
    {
        StartPlan();
    }

    public void StartPlan()
    {
        defaultPlan.gameObject.SetActive(true);
    }

    public void StartBuilding(Vector3Int cellPos)
    {
        defaultPlan.gameObject.SetActive(false);

        var newBuilding = Instantiate(defaultBuilding, this.transform);
        newBuilding.xLength = defaultPlan.xLength;
        newBuilding.yLength = defaultPlan.yLength;
        newBuilding.spriteRenderer.sprite = defaultPlan.spriteRenderer.sprite;
        newBuilding.cellPos = cellPos;

        newBuilding.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
