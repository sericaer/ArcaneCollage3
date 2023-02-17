using RxPropertyChanged;
using Sessions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingForm : MonoBehaviour
{
    public List<BuildingFormItem> items;

    //protected override void BindingInit()
    //{
    //    Binding(dataContext => dataContext.count, buildingCount);
    //}

    private void Awake()
    {
        
    }
}

[System.Serializable]
public class BuildingFormItem
{
    public string name;
}
