using Mods.Defines;
using Sessions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class ConstructPlanSpriteMgr : MonoBehaviour
{
    public UnityEvent<IConstructPlan, Vector3> CreateBuilding;

    public Tilemap tilemap;

    public ConstructPlanSprite planPrototype;

    private ConstructPlanSprite currPlan;

    internal void StartPlan(IConstructPlan dataSource)
    {
        if (currPlan != null)
        {
            Destroy(currPlan.gameObject);
            currPlan = null;
        }

        if (dataSource == null)
        {
            return;
        }

        currPlan = Instantiate<ConstructPlanSprite>(planPrototype, this.transform);
        currPlan.tilemap = tilemap;
        currPlan.dataContext = dataSource;

        currPlan.StartBuilding.AddListener((pos) => CreateBuilding.Invoke(dataSource, pos));
    }
}

