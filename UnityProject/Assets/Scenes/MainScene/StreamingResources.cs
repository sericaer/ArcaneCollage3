using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StreamingResources
{
    public static Dictionary<string, Sprite> sprites;
    public static GMEngine.Mods mods;

    public static void Load()
    {
        mods = new GMEngine.Mods(Path.Combine(Application.streamingAssetsPath, "Mods"));

        sprites = mods.images.ToDictionary(i => i.path, i =>
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(i.bytes);
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100);
            sprite.name = i.path;
            return sprite;
        });
    }
}