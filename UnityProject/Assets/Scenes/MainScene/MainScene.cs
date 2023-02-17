using RxPropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : RxBehaviour<ISession>
{
    public Tilemap tilemap;
    public TileBase tileset;


    public BuildingTop buildingTop;

    public Transform center;

    //public BuildingForm buildingForm;

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.buildings.count, buildingTop.buildingCount);
    }

    void Start()
    {
        SetDataContext(new Session());

        buildingTop.button.onClick.AddListener(() =>
        {
            //var form = Instantiate(buildingForm, center);
            //form.dataContext = session?.buildings;
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
        if(dataContext != null)
        {
            dataContext.buildings.count++;
        }
    }
}

//public class BuildingForm : MonoBehaviour
//{
//    public List<IBuilding> dataContext { get; set; }
//}

public interface ISession : INotifyPropertyChanged
{
    public IBuildingMgr buildings { get; }

    public int a { get; set; }
}


public class Session : ISession
{
    public IBuildingMgr buildings { get; } = new BuildingMgr();

    public int a
    {
        get
        {
            return _a;
        }
        set
        {
            _a = value;

            NotifyPropertyChanged();
        }
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private int _a;


    public event PropertyChangedEventHandler PropertyChanged;
}

internal class BuildingMgr : IBuildingMgr
{
    public event PropertyChangedEventHandler PropertyChanged;

    public int count
    {
        get
        {
            return _a;
        }
        set
        {
            _a = value;

            NotifyPropertyChanged();
        }
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private int _a;
}

public interface IBuildingMgr : INotifyPropertyChanged
{
    int count { get; set; }
}