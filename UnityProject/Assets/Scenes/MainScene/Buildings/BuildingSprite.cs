using Mods.Defines;
using RxPropertyChanged;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSprite :  RxBehaviour<Sessions.IBuilding>
{
    public Grid grid;

    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
    }

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.pos, (pos) =>
        {
            transform.position = grid.CellToWorld(new Vector3Int(pos.x, pos.y, pos.z));
        });

        Binding(dataContext => dataContext.image, (image) =>
        {
            var sprite = StreamingResources.sprites[image];
            GetComponent<SpriteRenderer>().sprite = sprite;
        });
    }
}
