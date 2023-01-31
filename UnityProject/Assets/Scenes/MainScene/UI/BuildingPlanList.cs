using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingPlanList : MonoBehaviour
{
    public Button defaultItem;

    public Tilemap tilemap;
    public BuildingPlan planPrototype;

    private BuildingPlan currPlan;
    // Start is called before the first frame update
    void Start()
    {
        StreamingResources.Load();

        foreach(var sprite in StreamingResources.sprites)
        {
            var newItem = Instantiate(defaultItem, defaultItem.transform.parent);
            newItem.name = sprite.name;
            newItem.GetComponentInChildren<Text>().text = newItem.name = sprite.name;

            newItem.onClick.AddListener(() =>
            {
                if(currPlan != null)
                {
                    Destroy(currPlan.gameObject);
                    currPlan = null;
                }

                currPlan = Instantiate<BuildingPlan>(planPrototype);
                currPlan.tilemap = tilemap;
                currPlan.sprite = sprite;
            });

        }

        defaultItem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
