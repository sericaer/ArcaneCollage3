using Sessions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using UnityEngine;

namespace RxPropertyChanged
{
    public class RxSpriteMgrBehaviour<TData, TSprite> : MonoBehaviour
        where TData : class, INotifyPropertyChanged
        where TSprite : RxBehaviour<TData>
    {
        public TSprite spritePrototype;

        public IRxCollection<TData> itemSource
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

                _compositeDisposable.Add(_itemSource.OnAddItem.Subscribe(item=>AddSprite(item)));

                _compositeDisposable.Add(_itemSource.OnRemoveItem.Subscribe(item => RemoveSprite(item)));


                foreach (var sprite in sprites)
                {
                    Destroy(sprite.gameObject);
                }

                sprites.Clear();

                foreach (var item in itemSource)
                {
                    AddSprite(item);
                }
            }
        }

        private void RemoveSprite(TData dataSource)
        {
            var sprite = sprites.Single(s => s.dataContext == dataSource);
            sprites.Remove(sprite);

            Destroy(sprite.gameObject);
        }

        private void AddSprite(TData dataSource)
        {
            var sprite = Instantiate(spritePrototype, this.transform);
            sprite.dataContext = dataSource;

            sprites.Add(sprite);
        }

        private IRxCollection<TData> _itemSource;

        private CompositeDisposable _compositeDisposable;

        private List<TSprite> sprites { get; } = new List<TSprite>();

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
