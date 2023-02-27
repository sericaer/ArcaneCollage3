using Sessions;
using UnityEngine;
using UnityEngine.Events;

public class ConstructPlanSpriteMgr2 : MonoBehaviour
{

    public ConstructPlanSprite plan;

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
        plan.GetComponent<BuildingPlanRender>().StartBuilding.AddListener((pos) =>
        {
            plan.gameObject.SetActive(false);

            CreateBuilding.Invoke(plan.dataContext, pos);
        });
    }
}