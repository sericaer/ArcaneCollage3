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
        IDate date { get; }
        IBuildingMgr buildings { get; }
        ICommandMgr constructCommands { get; }
        IPersonMgr persons { get; }
        ICashMgr cashMgr { get; }
        IConstructPlan constructPlan { get; }

        IBuilding CreateBuilding(IConstructPlan def, (int x, int y) pos);
        void HoursInc();
    }

    public interface IDate : INotifyPropertyChanged
    {
        int year { get; }
        int month { get; }
        int day { get; }
        int hour { get; }
    }

    public interface IPersonMgr : IRxCollection<IPerson>
    {
        void AddPerson(IPerson person);
        void RemovePerson(IPerson person);
    }

    public interface IPerson : INotifyPropertyChanged
    {
        (float x, float y) pos { get; set; }
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

        public IDate date { get; } = new Date();

        public IBuildingMgr buildings { get; } = new BuildingMgr();

        public ICashMgr cashMgr { get; } = new CashMgr();

        public ICommandMgr constructCommands { get; }

        public IConstructPlan constructPlan { get; set; }

        public IPersonMgr persons { get; } = new PersonMgr();

        public IBuilding CreateBuilding(IConstructPlan plan, (int x, int y) pos)
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

            for(int i=0; i<10; i++)
            {
                persons.AddPerson(new Person());
            }
        }

        public void HoursInc()
        {
            var dateTime = date as Date;
            dateTime.hour++;
        }
    }

    internal class Person : IPerson
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public (float x, float y) pos { get; set; }

    }

    internal class PersonMgr : IPersonMgr
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<IPerson> persons = new List<IPerson>();

        private readonly Subject<IPerson> _OnAddItem = new Subject<IPerson>();
        private readonly Subject<IPerson> _OnRemoveItem = new Subject<IPerson>();

        public int count { get; set; }

        public IObservable<IPerson> OnAddItem => _OnAddItem;
        public IObservable<IPerson> OnRemoveItem => _OnRemoveItem;

        public IEnumerator<IPerson> GetEnumerator()
        {
            return ((IEnumerable<IPerson>)persons).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)persons).GetEnumerator();
        }

        public void AddPerson(IPerson person)
        {
            persons.Add(person);
            _OnAddItem.OnNext(person);
            count = persons.Count;
        }

        public void RemovePerson(IPerson person)
        {
            persons.Remove(person);
            _OnRemoveItem.OnNext(person);
            count = persons.Count;
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

        public IBuilding AddBuilding(BuildingDefine def, (int x, int y) pos)
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

        public (int x, int y) pos { get; }
        public Building(BuildingDefine def, (int x, int y) pos)
        {
            this.def = def;
            this.pos = pos;

            image = def.image;
        }
    }

    public interface IBuildingMgr : IRxCollection<IBuilding>
    {
        IBuilding AddBuilding(BuildingDefine def, (int x, int y) pos);
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

        (int x, int y) pos { get; }
    }

    public interface ICashMgr : INotifyPropertyChanged
    {
        float current { get; set; }
    }

    public class Date : IDate
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int year
        {
            get
            {
                return _year;
            }
            private set
            {
                _year = value;
            }
        }

        public int month
        {
            get
            {
                return _month;
            }
            private set
            {
                _month = value;
                if (_month > 4)
                {
                    year += 1;
                    _month = 1;
                }
            }
        }

        public int day
        {
            get
            {
                return _day;
            }
            private set
            {
                if (_day == value)
                {
                    return;
                }

                _day = value;
                if (_day > 30)
                {
                    month += 1;
                    _day = 1;
                }
            }
        }

        public int hour
        {
            get
            {
                return _hour;
            }
            internal set
            {
                if (_hour == value)
                {
                    return;
                }

                _hour = value;
                if (_hour > 24)
                {
                    day += 1;
                    _hour = 0;
                }
            }
        }

        private int _year;
        private int _month;
        private int _day;
        private int _hour;

        public Date()
        {
            year = 1;
            month = 1;
            day = 1;
            hour = 7;
        }
    }
}
