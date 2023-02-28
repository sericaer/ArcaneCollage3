using Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using UnityEngine;
using UnityEngine.Events;

namespace RxPropertyChanged
{
    public class RxContainerBehaviour<TDataSource, TTargetItem> : MonoBehaviour
        where TDataSource : class, INotifyPropertyChanged
        where TTargetItem : RxBehaviour<TDataSource>
    {
        public TTargetItem defaultItem;

        public UnityEvent<TTargetItem> OnAddedEvent;

        public IRxCollection<TDataSource> itemSource
        {
            get
            {
                return _itemSource;
            }
            set
            {
                if (_itemSource == value)
                {
                    return;
                }

                DisposeBinding();
                _compositeDisposable = new CompositeDisposable();

                _itemSource = value;

                _compositeDisposable.Add(_itemSource.OnAddItem.Subscribe(item=>AddTargerItem(item)));

                _compositeDisposable.Add(_itemSource.OnRemoveItem.Subscribe(item => RemoveData(item)));


                foreach (var sprite in sprites)
                {
                    Destroy(sprite.gameObject);
                }

                sprites.Clear();

                foreach (var item in itemSource)
                {
                    AddTargerItem(item);
                }
            }
        }

        private void RemoveData(TDataSource dataSource)
        {
            var sprite = sprites.Single(s => s.dataContext == dataSource);
            sprites.Remove(sprite);

            Destroy(sprite.gameObject);
        }

        private void AddTargerItem(TDataSource dataSource)
        {
            var sprite = Instantiate(defaultItem, defaultItem.transform.parent);
            sprite.dataContext = dataSource;
            sprite.gameObject.SetActive(true);

            sprites.Add(sprite);
        }

        private IRxCollection<TDataSource> _itemSource;

        private CompositeDisposable _compositeDisposable;

        private List<TTargetItem> sprites { get; } = new List<TTargetItem>();

        protected void DisposeBinding()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        protected virtual void OnDestroy()
        {
            DisposeBinding();
        }

        private void Start()
        {
            defaultItem.gameObject.SetActive(false);
        }
    }
}
