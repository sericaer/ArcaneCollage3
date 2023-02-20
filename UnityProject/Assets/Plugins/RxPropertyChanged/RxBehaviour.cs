using ReactiveMarbles.PropertyChanged;
using Sessions;
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
        protected T dataContext;

        private CompositeDisposable _compositeDisposable;

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
            if (_compositeDisposable == null)
            {
                _compositeDisposable = new CompositeDisposable();
            }

            _compositeDisposable.Add(dataContext.WhenChanged(fromProperty).Subscribe(x => text.text = x.ToString()));
        }

        protected void Binding<TFrom, TTarget>(Expression<Func<T, IRxCollection<TFrom>>> fromProperty, RxSpriteMgrBehaviour<TFrom, TTarget> spriteMgr)
            where TFrom : class, INotifyPropertyChanged
        {
            if (_compositeDisposable == null)
            {
                _compositeDisposable = new CompositeDisposable();
            }

            _compositeDisposable.Add(dataContext.WhenChanged(fromProperty).Subscribe(x => text.text = x.ToString()));
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

    public class RxSpriteMgrBehaviour<TData, TSprite> : MonoBehaviour
    {

    }
}
