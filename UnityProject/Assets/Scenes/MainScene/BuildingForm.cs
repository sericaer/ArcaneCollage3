using RxPropertyChanged;
using Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using UnityEngine;
using UnityEngine.UI;

public class BuildingForm : MonoBehaviour
{
    public List<BuildingFormItem> items;
    private CompositeDisposable _compositeDisposable;

    internal void SetItemSource(IBuildingMgr buildings)
    {
        items.Clear();

        DisposeBinding();

        _compositeDisposable = new CompositeDisposable();

        _compositeDisposable.Add(buildings.OnAddItem.Subscribe(x =>
        {
            var item = new BuildingFormItem();
            item.SetDataContext(x);
            items.Add(item);
        }));

        _compositeDisposable.Add(buildings.OnRemoveItem.Subscribe(x =>
        {
            var item = items.Single(x => x.dataContext == x);
            item.Dispose();

            items.Remove(item);
        }));
    }

    protected void DisposeBinding()
    {
        _compositeDisposable?.Dispose();
        _compositeDisposable = null;
    }

    protected virtual void OnDestroy()
    {
        DisposeBinding();
    }
}

[System.Serializable]
public class BuildingFormItem : IDisposable
{
    public string name;

    public IBuilding dataContext;

    private CompositeDisposable _compositeDisposable;

    public void SetDataContext(IBuilding building)
    {
        _compositeDisposable?.Dispose();

        _compositeDisposable = new CompositeDisposable();

        this.dataContext = building;

        name = dataContext.name;
    }

    public void Dispose()
    {
        _compositeDisposable?.Dispose();
    }
}