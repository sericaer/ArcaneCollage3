using Mods.Defines;
using RxPropertyChanged;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSprite :  RxBehaviour<Sessions.IBuilding>
{
    public Tilemap tilemap;

    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
    }

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.pos, (pos) =>
        {
            transform.position = tilemap.CellToWorld(new Vector3Int(pos.x, pos.y, pos.z));

            for (int i = -1; i <= dataContext.def.size.x; i++)
            {
                for (int j = -1; j <= dataContext.def.size.y; j++)
                {
                    tilemap.SetTileColor(new Vector3Int(pos.x + i, pos.y + j), Color.gray);
                }
            }
        });

        Binding(dataContext => dataContext.image, (image) =>
        {
            var sprite = StreamingResources.sprites[image];
            GetComponent<SpriteRenderer>().sprite = sprite;
        });
    }
}
