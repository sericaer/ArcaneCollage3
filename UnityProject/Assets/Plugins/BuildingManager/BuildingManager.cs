using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    public Tilemap buildingTilemap;

    public Tilemap overlapBuilding;

    public Tilemap edge;

    public BuildingPlanRender defaultPlan;
    public SpriteRenderer defaultBuilding;

    public void Start()
    {
        defaultPlan.CheckLegeal = () => !IsOverlapBuilding() && IsOnEdge();

        buildingTilemap.SetTileColor(new Vector3Int(0, 1), Color.gray);
    }

    public void DefaultStartBuilding(Vector3Int pos)
    {
        var newBuilding = Instantiate(defaultBuilding, this.transform);

        newBuilding.sprite = defaultPlan.spriteRenderer.sprite;
        newBuilding.transform.position = buildingTilemap.CellToWorld(pos);
        newBuilding.gameObject.SetActive(true);

        for (int i = -1; i <= defaultPlan.x; i++)
        {
            for (int j = -1; j <= defaultPlan.y; j++)
            {
                buildingTilemap.SetTileColor(pos + new Vector3Int(i, j), Color.gray);
            }
        }
    }

    private bool IsOverlapBuilding()
    {
        return defaultPlan.GetMaxtrix().Any(x => buildingTilemap.HasTile(x));
    }

    private bool IsOnEdge()
    {
        return defaultPlan.GetRing(2).Any(x => buildingTilemap.HasTile(x));
    }
}
