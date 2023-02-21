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

    void Start()
    {
        StreamingResources.Load();

        foreach(var def in StreamingResources.mods.GetDefines<BuildingDefine>())
        {
            var newItem = Instantiate(defaultItem, defaultItem.transform.parent);
            newItem.name = def.name;
            newItem.GetComponentInChildren<Text>().text = newItem.name;

            newItem.onClick.AddListener(() => StartPlan.Invoke(def));
        }

        defaultItem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

