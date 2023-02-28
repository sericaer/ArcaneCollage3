using Mods.Defines;
using RxPropertyChanged;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Maths.Matrix;

class BuildingSprite :  RxBehaviour<Sessions.IBuilding>
{
    public RoadTilemap roadTilemap;
    public Tilemap buildingTileap;

    void OnMouseDown()
    {
        Debug.Log("Sprite Clicked");
    }

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.pos, (pos) =>
        {
            transform.position = buildingTileap.CellToWorld(new Vector3Int(pos.x, pos.y));

            AddRoadTileSets(pos);
            AddBuildingTileSets(pos);
        });

        Binding(dataContext => dataContext.image, (image) =>
        {
            var sprite = StreamingResources.sprites[image];
            GetComponent<SpriteRenderer>().sprite = sprite;
        });
    }

    private void AddBuildingTileSets((int x, int y) pos)
    {
        foreach(var index in MatrixMethod.Generate(pos, dataContext.def.size.x, dataContext.def.size.y))
        {
            buildingTileap.SetTileColor(new Vector3Int(index.x, index.y), Color.red);
        }
    }

    private void AddRoadTileSets((int x, int y) pos)
    {
        foreach (var index in MatrixMethod.GetRing(pos, 2, dataContext.def.size.x, dataContext.def.size.y))
        {
            roadTilemap.SetTile(new Vector3Int(index.x, index.y));
        }
    }
}
