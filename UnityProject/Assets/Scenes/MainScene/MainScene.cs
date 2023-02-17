using RxPropertyChanged;
using Sessions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : RxBehaviour<ISession>
{
    public Tilemap tilemap;
    public TileBase tileset;


    public BuildingTop buildingTop;

    public Transform center;

    public BuildingForm buildingForm;

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.buildings.count, buildingTop.buildingCount);
    }

    void Start()
    {
        SetDataContext(new Session());

        buildingTop.button.onClick.AddListener(() =>
        {
            var form = Instantiate(buildingForm, center);
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

