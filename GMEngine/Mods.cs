using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GMEngine
{
    public class Mods
    {
        public static Dictionary<string, Type> path2Type = new Dictionary<string, Type>();

        private Dictionary<Type, List<object>> defines = new Dictionary<Type, List<object>>();
        
        public Mods(string root)
        {
            foreach(var dir in Directory.EnumerateDirectories(root))
            {
                foreach(var pair in path2Type)
                {
                    if (!defines.ContainsKey(pair.Value))
                    {
                        defines.Add(pair.Value, new List<object>());
                    }
                    
                    var absolutPath = Path.Combine(dir, pair.Key);

                    EnumerateFiles(absolutPath, (scriptPath) =>
                    {
                        defines[pair.Value].Add(Activator.CreateInstance(pair.Value, scriptPath.Replace(root, ""), File.ReadAllText(scriptPath)));
                    });
                }
            }
        }

        public IEnumerable<T> GetDefines<T>()
        {
            return defines[typeof(T)].OfType<T>();
        }

        public static void RegisterDefine<T>(string path)
        {
            path2Type.Add(path, typeof(T));
        }

        private static void EnumerateFiles(string path, Action<string> iterFileAction)
        {
            var splits = path.Split('\\');
            EnumerateFiles(splits[0], new Queue<string>(splits.Skip(1)), iterFileAction);
        }

        private static void EnumerateFiles(string curr, Queue<string> subPaths, Action<string> iterFileAction)
        {
            var next = subPaths.Dequeue();
            if (subPaths.Count == 0)
            {
                foreach (var file in Directory.EnumerateFiles(curr, next))
                {
                    iterFileAction(file);
                }

                return;
            }

            if (next != "*")
            {
                curr = Path.Combine(curr, next);
                EnumerateFiles(curr, subPaths, iterFileAction);
            }
            else
            {
                foreach (var subPath in Directory.EnumerateDirectories(curr))
                {
                    curr = Path.Combine(curr, subPath);
                    EnumerateFiles(curr, new Queue<string>(subPaths), iterFileAction);
                }
            }
        }
    }
}
