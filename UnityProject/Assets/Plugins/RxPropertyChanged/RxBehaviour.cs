using ReactiveMarbles.PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace RxPropertyChanged
{
    public abstract class RxBehaviour<T> : MonoBehaviour
        where T : class, INotifyPropertyChanged
    {
        protected T _dataContext;

        private CompositeDisposable _compositeDisposable;

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

        public void SetDataContext(T dataContext)
        {
            DisposeBinding();

            this.dataContext = dataContext;
            if(dataContext == null)
            {
                return;
            }

            BindingInit();
        }

        protected abstract void BindingInit();

        protected void Binding<TProperty>(Expression<Func<T,TProperty>> fromProperty, Text text)
        {
            _compositeDisposable.Add(dataContext.WhenChanged(fromProperty).Subscribe(x => text.text = x.ToString()));
        }

        protected void Binding<TProperty>(Expression<Func<T, TProperty>> fromProperty, Action<TProperty> action)
        {
            _compositeDisposable.Add(dataContext.WhenChanged(fromProperty).Subscribe(x => action(x)));
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
