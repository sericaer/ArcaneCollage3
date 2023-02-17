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
        public event PropertyChangedEventHandler PropertyChanged;

        public int count { get; set; }
    }

    public interface IBuildingMgr
    {
        IObservable<int> count { get; }

        IObservable<IBuilding> OnAddItem { get; }

        IObservable<IBuilding> OnRemoveItem { get; }
    }

    public interface IBuilding
    {
    }
}
