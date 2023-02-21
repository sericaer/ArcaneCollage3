using GMEngine;
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
        ICommandMgr constructCommands { get; }

        ICashMgr cashMgr { get; }

        IBuilding CreateBuilding(IConstructPlan def, (float x, float y, float z) pos);

        IConstructPlan constructPlan { get; }
    }

    public interface IConstructPlan : INotifyPropertyChanged
    {
        string image { get; }

        BuildingDefine def { get; }
    }

    public interface ICommand : INotifyPropertyChanged
    {
        string title { get; set; }

        Action Exec { get; set; }
        Func<bool> isVaild { get; set; }
    }

    public interface ICommandMgr : IRxCollection<ICommand>
    {

    }

    public class Command : ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string title { get; set; }

        public Action Exec { get; set; }

        public Func<bool> isVaild { get; set; }
    }

    public class ConstructPlan : IConstructPlan
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string image => def.image;

        public BuildingDefine def { get; }
        public ConstructPlan(BuildingDefine def)
        {
            this.def = def;
        }
    }

    public class ConstructCommandMgr : ICommandMgr
    {
        private List<ICommand> commands;

        private readonly Subject<ICommand> _OnAddItem = new Subject<ICommand>();
        private readonly Subject<ICommand> _OnRemoveItem = new Subject<ICommand>();

        public ConstructCommandMgr(IEnumerable<BuildingDefine> defines, Session session)
        {
            commands = defines.Select(def =>
            {
                ICommand cmd = new Command()
                {
                    title = def.name,
                    isVaild = () => true,
                    Exec = ()=> session.constructPlan = new ConstructPlan(def)
                };
                return cmd;
            }).ToList();

            count = commands.Count();
        }

        public int count { get; set; }

        public IObservable<ICommand> OnAddItem => _OnAddItem;

        public IObservable<ICommand> OnRemoveItem => _OnRemoveItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerator<ICommand> GetEnumerator()
        {
            return ((IEnumerable<ICommand>)commands).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)commands).GetEnumerator();
        }
    }

    public class Session : ISession
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IBuildingMgr buildings { get; } = new BuildingMgr();

        public ICashMgr cashMgr { get; } = new CashMgr();

        public ICommandMgr constructCommands { get; }

        public IConstructPlan constructPlan { get; set; }

        public IBuilding CreateBuilding(IConstructPlan plan, (float x, float y, float z) pos)
        {
            var resource = plan.def.constructionCost.SingleOrDefault(x => x.type == ResourceType.Cash);
            if(resource != null)
            {
                cashMgr.current -= resource.value;
            }
            
            return buildings.AddBuilding(plan.def, pos);
        }

        public Session(GMEngine.Mods mods)
        {
            constructCommands = new ConstructCommandMgr(mods.GetDefines<BuildingDefine>(), this);
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
        public event PropertyChangedEventHandler PropertyChanged;

        public string name { get; set; }
        public string image { get; }

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
