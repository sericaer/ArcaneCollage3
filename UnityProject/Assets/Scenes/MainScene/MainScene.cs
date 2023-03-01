using GMEngine;
using Mods.Defines;
using RxPropertyChanged;
using Sessions;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

class MainScene : RxBehaviour<ISession>
{
    public BuildingTop buildingTop;
    public CashTop cashTop;

    public BuildingSpriteMgr buildingSpriteMgr;
    public BuildingPlanSpriteMgr constructPlanSpriteMgr;
    public PersonSpriteMgr personSpriteMgr;

    public ConstructCmdContainer constructCmdContainer;

    public ViewBox center;

    public BuildingForm buildingForm;

    public void OnCreateBuilding(IConstructPlan plan, Vector3Int pos)
    {
        dataContext.CreateBuilding(plan, (pos.x, pos.y));
    }

    public void OnTimeInc()
    {
        Debug.Log("OnTimeInc");

        //dataContext.currTime.hour++;
    }

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.buildings.count, buildingTop.buildingCount);
        Binding(dataContext => dataContext.cashMgr.current, cashTop.current);
        Binding(dataContext => dataContext.constructPlan, (plan) => constructPlanSpriteMgr.StartPlan(plan));

        buildingSpriteMgr.itemSource = dataContext.buildings;
        constructCmdContainer.itemSource = dataContext.constructCommands;
        personSpriteMgr.itemSource = dataContext.persons;
    }

    void Start()
    {
        StreamingResources.Load();

        dataContext = new Session(StreamingResources.mods);

        dataContext.cashMgr.current = 100;

        buildingTop.button.onClick.AddListener(() =>
        {
            var form = center.CreateInstance(buildingForm);
            form.SetItemSource(dataContext?.buildings);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}

