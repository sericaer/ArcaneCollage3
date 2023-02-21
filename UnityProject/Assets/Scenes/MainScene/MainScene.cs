using Mods.Defines;
using RxPropertyChanged;
using Sessions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : RxBehaviour<ISession>
{
    public Tilemap tilemap;
    public TileBase tileset;

    public BuildingTop buildingTop;

    public BuildingSpriteMgr buildingSpriteMgr;
    
    public ViewBox center;

    public BuildingForm buildingForm;

    public void OnCreateBuilding(BuildingDefine def, Vector3 pos)
    {
        dataContext.buildings.AddBuilding(def, (pos.x, pos.y, pos.z));
    }

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.buildings.count, buildingTop.buildingCount);
    }

    void Start()
    {
        dataContext = new Session();

        buildingSpriteMgr.itemSource = dataContext.buildings;

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

