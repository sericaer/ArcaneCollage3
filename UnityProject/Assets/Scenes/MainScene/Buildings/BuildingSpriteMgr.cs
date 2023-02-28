using RxPropertyChanged;
using UnityEngine;

class BuildingSpriteMgr : RxContainerBehaviour<Sessions.IBuilding, BuildingSprite>
{
    public RoadTilemap roadTilemap;

    public void Start()
    {
        OnAddedEvent.AddListener((item) =>
        {
            for (int i = -2; i < item.def.size.x + 2; i++)
            {
                for (int j = -2; j < item.def.size.y + 2; j++)
                {
                    roadTilemap.SetTile(new Vector3Int(item.pos.x + i, item.pos.y + j));
                }
            }
        });
    }
}
