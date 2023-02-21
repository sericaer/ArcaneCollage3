using Mods.Defines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace Sessions
{
    public interface ISession : INotifyPropertyChanged
    {
        IBuildingMgr buildings { get; }
        ICashMgr cashMgr { get; }

        IBuilding CreateBuilding(BuildingDefine def, (float x, float y, float z) pos);
    }


    public class Session : ISession
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IBuildingMgr buildings { get; } = new BuildingMgr();

        public ICashMgr cashMgr { get; } = new CashMgr();

        public IBuilding CreateBuilding(BuildingDefine def, (float x, float y, float z) pos)
        {
            var resource = def.constructionCost.SingleOrDefault(x => x.type == ResourceType.Cash);
            if(resource != null)
            {
                cashMgr.current -= resource.value;
            }
            
            return buildings.AddBuilding(def, pos);
        }
    }

    internal class CashMgr : ICashMgr
    {
        public float current { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    internal class BuildingMgr : IBuildingMgr
    {

        private readonly Subject<IBuilding> _OnAddItem = new Subject<IBuilding>();
        private readonly Subject<IBuilding> _OnRemoveItem = new Subject<IBuilding>();

        private List<IBuilding> items = new List<IBuilding>();

        public int count { get; set; }

        public IObservable<IBuilding> OnAddItem => _OnAddItem;

        public IObservable<IBuilding> OnRemoveItem => _OnRemoveItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public IBuilding AddBuilding(BuildingDefine def, (float x, float y, float z) pos)
        {
            var building = new Building(def, pos);

            building.name = DateTime.Now.ToString();

            items.Add(building);

            _OnAddItem.OnNext(building);

            count = this.Count();

            return building;
        }

        public void RemoveBuilding(IBuilding building)
        {
            items.Remove(building);

            _OnRemoveItem.OnNext(building);
            count = this.Count();
        }

        public IEnumerator<IBuilding> GetEnumerator()
        {
            return ((IEnumerable<IBuilding>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }

        public BuildingMgr()
        {

        }
    }

    public class Building : IBuilding
    {
        public string name { get; set; }
        public string image { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BuildingDefine def { get; }

        public (float x, float y, float z) pos { get; }
        public Building(BuildingDefine def, (float x, float y, float z) pos)
        {
            this.def = def;
            this.pos = pos;

            image = def.image;
        }
    }

    public interface IBuildingMgr : IRxCollection<IBuilding>
    {
        IBuilding AddBuilding(BuildingDefine def, (float x, float y, float z) pos);
        void RemoveBuilding(IBuilding building);
    }

    public interface IRxCollection<T> : INotifyPropertyChanged, IEnumerable<T>
        where T:class, INotifyPropertyChanged
    {
        int count { get; }

        IObservable<T> OnAddItem { get; }

        IObservable<T> OnRemoveItem { get; }
    }

    public interface IBuilding : INotifyPropertyChanged
    {
        string name { get; }

        string image { get; }
        BuildingDefine def { get; }

        (float x, float y, float z) pos { get; }
    }

    public interface ICashMgr : INotifyPropertyChanged
    {
        float current { get; set; }
    }
}
