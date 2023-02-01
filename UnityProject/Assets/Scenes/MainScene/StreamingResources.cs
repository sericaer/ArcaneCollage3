using GMEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class StreamingResources
{
    public static Dictionary<string, Sprite> sprites;
    public static Mods mods;

    public static void Load()
    {
        mods = new Mods(Path.Combine(Application.streamingAssetsPath, "Mods"));

        sprites = mods.images.ToDictionary(i => i.path, i =>
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(i.bytes);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100);
        });
    }
}

[DefineProperty(scriptPath: @"Buildings\*\Define.hjson")]
public class BuildingDefine : IDefine
{
    public string path { get; set; }
    public string name => Directory.GetParent(path).Name;
    public string key => path;
    public string image => path.Replace("Define.hjson", "Image.png");

    [JsonProperty("construction_cost")]
    public int constructionCost;

    [JsonProperty("maintenance_cost")]
    public int maintenanceCost;
}