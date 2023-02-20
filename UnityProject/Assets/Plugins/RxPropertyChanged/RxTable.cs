using ReactiveMarbles.PropertyChanged;
using Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using UnityEngine;

namespace RxPropertyChanged
{
    public abstract class RxTableItem<T> : IDisposable
where T : class, INotifyPropertyChanged
    {
        public T dataContext
        {
            get
            {
                return _dataContext;
            }
            set
            {
                DisposeBinding();

                _compositeDisposable = new CompositeDisposable();

                _dataContext = value;

                BindingInit();
            }
        }

        protected abstract void BindingInit();

        private T _dataContext;
        private CompositeDisposable _compositeDisposable;

        protected void Binding<TProperty>(Expression<Func<T, TProperty>> fromProperty, Action<TProperty> targtAct)
        {
            if (_compositeDisposable == null)
            {
                _compositeDisposable = new CompositeDisposable();
            }

            _compositeDisposable.Add(dataContext.WhenChanged(fromProperty).Subscribe(x => targtAct(x)));
        }

        private void DisposeBinding()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        public void Dispose()
        {
            DisposeBinding();
        }
    }


    public class RxTable<TItemFrom, TItemTarget> : MonoBehaviour
        where TItemFrom : class, INotifyPropertyChanged
        where TItemTarget : RxTableItem<TItemFrom>, new()
    {
        private CompositeDisposable _compositeDisposable;

        private IRxCollection<TItemFrom> fromItems;
        private List<TItemTarget> targetItems;

        public void SetItemSource(IRxCollection<TItemFrom> fromItems)
        {
            if(this.fromItems == fromItems)
            {
                return;
            }

            this.fromItems = fromItems;

            targetItems.Clear();

            DisposeBinding();

            foreach (var from in fromItems)
            {
                var item = new TItemTarget();
                item.dataContext = from;
                targetItems.Add(item);
            }

            _compositeDisposable = new CompositeDisposable();
            _compositeDisposable.Add(fromItems.OnAddItem.Subscribe(x =>
            {
                var item = new TItemTarget();
                item.dataContext = x;
                targetItems.Add(item);
            }));

            _compositeDisposable.Add(fromItems.OnRemoveItem.Subscribe(x =>
            {
                var item = targetItems.Single(x => x.dataContext == x);
                item.Dispose();

                targetItems.Remove(item);
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
}
