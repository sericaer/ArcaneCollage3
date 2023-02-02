using ArcaneCollage.Sessions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainScene : MonoBehaviour
{
    public Tilemap tilemap;

    private Session session;
    private IEnumerable<System> systems;

    void Start()
    {
        session = new Session();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var system in systems)
        {
            foreach(var entity in session.entities)
            {
                system.Do(entity);
            }
        }    
    }
}

class System
{
    public virtual void Do()
    {
        foreach(var (newFlag, pos) in SystemQuery<NewFlag, Position>())
        {

        }
    }
}
