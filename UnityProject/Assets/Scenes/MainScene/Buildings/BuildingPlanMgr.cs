using Mods.Defines;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BuildingPlanMgr : MonoBehaviour
{
    public UnityEvent<BuildingDefine, Vector3> CreateBuilding;

    public Tilemap tilemap;

    public BuildingPlan planPrototype;

    private BuildingPlan currPlan;

    public void OnStartPlan(BuildingDefine def)
    {
        if (currPlan != null)
        {
            Destroy(currPlan.gameObject);
            currPlan = null;
        }

        currPlan = Instantiate<BuildingPlan>(planPrototype);
        currPlan.tilemap = tilemap;
        currPlan.def = def;

        currPlan.StartBuilding.AddListener((def, pos) => CreateBuilding.Invoke(def, pos));
    }
}