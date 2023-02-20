using Mods.Defines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingPlanList : MonoBehaviour
{
    public UnityEvent<BuildingDefine> StartPlan;
    public Button defaultItem;

    //public Tilemap tilemap;
    //public BuildingPlan planPrototype;

    //private BuildingPlan currPlan;
    // Start is called before the first frame update
    void Start()
    {
        StreamingResources.Load();

        foreach(var def in StreamingResources.mods.GetDefines<BuildingDefine>())
        {
            var newItem = Instantiate(defaultItem, defaultItem.transform.parent);
            newItem.name = def.name;
            newItem.GetComponentInChildren<Text>().text = newItem.name;

            newItem.onClick.AddListener(() => StartPlan.Invoke(def));

            //newItem.onClick.AddListener(() => 
            //{
            //    if (currPlan != null)
            //    {
            //        Destroy(currPlan.gameObject);
            //        currPlan = null;
            //    }

            //    currPlan = Instantiate<BuildingPlan>(planPrototype);
            //    currPlan.tilemap = tilemap;
            //    currPlan.sprite = StreamingResources.sprites[def.image];

            //    currPlan.PlanStart.AddListener((def) => CreateBuilding.Invoke(def));
            //});
        }

        defaultItem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

