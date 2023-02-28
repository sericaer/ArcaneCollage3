using Sessions;
using UnityEngine;
using UnityEngine.Events;

class BuildingPlanSpriteMgr : MonoBehaviour
{
    public BuildingPlanSprite plan;
    public RoadTilemap roadTilemap;

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
        var planRender = plan.GetComponent<BuildingPlanRender>();

        planRender.IsOverlapBuilding = () =>
        {
            return planRender.DefaultIsOverlapBuilding() && roadTilemap.HasTile(planRender.cellPos);
        };

        planRender.StartBuilding.AddListener((pos) =>
        {
            plan.gameObject.SetActive(false);

            CreateBuilding.Invoke(plan.dataContext, pos);
        });
    }
}