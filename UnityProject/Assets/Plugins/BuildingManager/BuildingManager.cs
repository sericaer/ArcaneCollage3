using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : MonoBehaviour
{
    public Tilemap tilemap;

    public BuildingPlanRender defaultPlan;
    public SpriteRenderer defaultBuilding;

    public void DefaultStartBuilding(Vector3Int pos)
    {
        var newBuilding = Instantiate(defaultBuilding, this.transform);

        newBuilding.sprite = defaultPlan.spriteRenderer.sprite;
        newBuilding.transform.position = tilemap.CellToWorld(pos);
        newBuilding.gameObject.SetActive(true);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                tilemap.SetTileColor(pos + new Vector3Int(i, j), Color.gray);
            }
        }
    }
}
