using ReactiveMarbles.PropertyChanged;
using RxPropertyChanged;
using Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using UnityEngine;



[System.Serializable]
public class BuildingFormItem : RxTableItem<IBuilding>
{
    public string name;

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.name, (x)=> name = x);
    }
}

public class BuildingForm : RxTable<IBuilding, BuildingFormItem>
{
}