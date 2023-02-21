using GMEngine;
using Mods.Defines;
using RxPropertyChanged;
using Sessions;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class MainScene : RxBehaviour<ISession>
{
    public Tilemap tilemap;
    public TileBase tileset;

    public BuildingTop buildingTop;
    public CashTop cashTop;

    public BuildingSpriteMgr buildingSpriteMgr;
    public ConstructCmdContainer constructCmdContainer;
    public ConstructPlanSpriteMgr constructPlanSpriteMgr;

    public ViewBox center;

    public BuildingForm buildingForm;

    public void OnCreateBuilding(IConstructPlan plan, Vector3 pos)
    {
        dataContext.CreateBuilding(plan, (pos.x, pos.y, pos.z));
    }

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.buildings.count, buildingTop.buildingCount);
        Binding(dataContext => dataContext.cashMgr.current, cashTop.current);
        Binding(dataContext => dataContext.constructPlan, (plan) => constructPlanSpriteMgr.StartPlan(plan));
    }

    void Start()
    {
        StreamingResources.Load();

        dataContext = new Session(StreamingResources.mods);

        dataContext.cashMgr.current = 100;

        buildingSpriteMgr.itemSource = dataContext.buildings;
        constructCmdContainer.itemSource = dataContext.constructCommands;

        buildingTop.button.onClick.AddListener(() =>
        {
            var form = center.CreateInstance(buildingForm);
            form.SetItemSource(dataContext?.buildings);
        });

        for (int i=-50; i<=50; i++)
        {
            for(int j=-50; j<=50; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j), tileset);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(dataContext != null)
        //{
        //    dataContext.buildings.count++;
        //}
    }
}

