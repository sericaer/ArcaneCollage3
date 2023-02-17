using System;
using System.ComponentModel;
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
        public IObservable<int> count => throw new NotImplementedException();

        public IObservable<IBuilding> OnAddItem => throw new NotImplementedException();

        public IObservable<IBuilding> OnRemoveItem => throw new NotImplementedException();

        public IBuilding AddBuilding()
        {
            throw new NotImplementedException();
        }

        public void RemoveBuilding(IBuilding building)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBuildingMgr : IRxCollection<IBuilding>
    {
        IBuilding AddBuilding();
        void RemoveBuilding(IBuilding building);
    }

    public interface IRxCollection<T>
        where T:class, INotifyPropertyChanged
    {
        IObservable<int> count { get; }

        IObservable<T> OnAddItem { get; }

        IObservable<T> OnRemoveItem { get; }
    }

    public interface IBuilding : INotifyPropertyChanged
    {
    }
}
