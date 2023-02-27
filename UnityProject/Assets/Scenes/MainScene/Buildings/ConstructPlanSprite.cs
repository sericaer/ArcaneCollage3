using Mods.Defines;
using RxPropertyChanged;
using Sessions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class ConstructPlanSprite : RxBehaviour<IConstructPlan>
{
    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.image, (image) =>
        {
            var sprite = StreamingResources.sprites[image];
            GetComponent<SpriteRenderer>().sprite = sprite;

            var boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.size = sprite.bounds.size;
            boxCollider2D.offset = boxCollider2D.size / 2;
        });
    }
}

