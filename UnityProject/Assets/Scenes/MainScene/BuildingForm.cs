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

    private Dictionary<object, CompositeDisposable> dictComposite;

    internal void SetItemSource(IBuildingMgr buildings)
    {
        items.Clear();

        DisposeBinding();

        _compositeDisposable = new CompositeDisposable();

        _compositeDisposable.Add(buildings.OnAddItem.Subscribe(x => items.Add(new BuildingFormItemWithContext(x))));
        _compositeDisposable.Add(buildings.OnRemoveItem.Subscribe(x =>
        {
            var item = items.OfType<BuildingFormItemWithContext>().Single(x => x.dataContext == x);
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
public class BuildingFormItem
{
    public string name;
}

public class BuildingFormItemWithContext: BuildingFormItem, IDisposable
{
    public IBuilding dataContext;

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}