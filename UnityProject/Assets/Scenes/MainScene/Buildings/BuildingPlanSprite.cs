using Mods.Defines;
using RxPropertyChanged;
using Sessions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BuildingPlanSprite : RxBehaviour<IConstructPlan>
{
    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.image, (image) =>
        {
            var sprite = StreamingResources.sprites[image];
            GetComponent<SpriteRenderer>().sprite = sprite;

            GetComponent<BuildingPlanRender>().x = dataContext.def.size.x;
            GetComponent<BuildingPlanRender>().y = dataContext.def.size.y;
        });
    }
}

