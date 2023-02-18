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

        int a { get; set; }
    }


    public class Session : ISession
    {
        public IBuildingMgr buildings { get; } = new BuildingMgr();

        public int a { get; set; }


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

        public IBuilding AddBuilding()
        {
            var building = new Building();
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

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public interface IBuildingMgr : IRxCollection<IBuilding>
    {
        IBuilding AddBuilding();
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
    }
}
