using GMEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StreamingResources
{
    public static Dictionary<string, Sprite> sprites;
    public static Mods mods;

    public static void Load()
    {
        var modRoot = Path.Combine(Application.streamingAssetsPath, "Mods");

        sprites = Directory.EnumerateFiles(modRoot, "*.png", SearchOption.AllDirectories)
            .Select(imgPath =>
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(imgPath));
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100);
                sprite.name = imgPath.Replace(modRoot, "");
                return sprite;
            })
            .ToDictionary(p=>p.name, p=>p);

        Mods.RegisterDefine<BuildingDefine>(@"Buildings\*\Define.hjson");

        mods = new Mods(modRoot);
    }
}

class BuildingDefine
{
    public string name;
    public string key;

    public int constructionCost;
    public int maintenanceCost;

    public BuildingDefine(string key, string content)
    {
        this.key = key.Replace("Define.hjson", "Image.png");

        this.name = Path.GetFileNameWithoutExtension(key);
    }
}