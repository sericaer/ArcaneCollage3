using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class StreamingResources
{
    public static Sprite[] sprites;

    public static void Load()
    {
        sprites = Directory.EnumerateFiles(Application.streamingAssetsPath, "*.png")
            .Select(imgPath =>
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(imgPath));
                var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100);
                sprite.name = Path.GetFileNameWithoutExtension(imgPath);
                return sprite;
            }).ToArray();
    }
}