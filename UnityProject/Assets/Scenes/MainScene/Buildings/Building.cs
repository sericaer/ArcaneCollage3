using Mods.Defines;
using RxPropertyChanged;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Building :  RxBehaviour<Sessions.IBuilding>
{
    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
    }

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.pos, (pos) =>
        {
            transform.position = new Vector3(pos.x, pos.y, pos.z);
        });

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