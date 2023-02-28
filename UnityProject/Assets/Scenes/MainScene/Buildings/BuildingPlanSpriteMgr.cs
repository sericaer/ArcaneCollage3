using Sessions;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

class BuildingPlanSpriteMgr : MonoBehaviour
{
    public BuildingPlanSprite plan;

    public RoadTilemap roadTilemap;
    public Tilemap buildingTilemap;

    public UnityEvent<IConstructPlan, Vector3Int> CreateBuilding;

    public void StartPlan(IConstructPlan dataSource)
    {
        if(dataSource == null)
        {
            plan.gameObject.SetActive(false);
            return;
        }

        plan.gameObject.SetActive(true);
        plan.dataContext = dataSource;
    }

    private void Start()
    {
        roadTilemap.SetTile(new Vector3Int());

        var planRender = plan.GetComponent<BuildingPlanRender>();

        planRender.CheckLegeal = () =>
        {
            return !IsOverlapBuilding() && IsOnRoadEdge();
        };

        planRender.StartBuilding.AddListener((pos) =>
        {
            plan.gameObject.SetActive(false);

            CreateBuilding.Invoke(plan.dataContext, pos);
        });
    }

    private bool IsOverlapBuilding()
    {
        var planRender = plan.GetComponent<BuildingPlanRender>();

        var array = planRender.GetMaxtrix().ToArray();
        return array.Any(x => buildingTilemap.HasTile(x));
    }

    private bool IsOnRoadEdge()
    {
        var planRender = plan.GetComponent<BuildingPlanRender>();

        return planRender.GetRing(2).Any(x => buildingTilemap.HasTile(x));
    }
}