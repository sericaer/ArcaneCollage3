using Hjson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GMEngine
{
    public class Mods
    {
        public static Dictionary<string, Type> path2Type = new Dictionary<string, Type>();

        private Dictionary<Type, List<IDefine>> type2Define = new Dictionary<Type, List<IDefine>>();
        
        public Mods(string root)
        {
            foreach(var dir in Directory.EnumerateDirectories(root))
            {
                foreach(var pair in path2Type)
                {
                    var definePath = pair.Key;
                    var defineType = pair.Value;

                    if (!type2Define.ContainsKey(defineType))
                    {
                        type2Define.Add(defineType, new List<IDefine>());
                    }
                    
                    var absolutPath = Path.Combine(dir, definePath);

                    EnumerateFiles(absolutPath, (scriptPath) =>
                    {
                        var jsonStr = HjsonValue.Load(scriptPath).ToString();

                        var define = JsonConvert.DeserializeObject(jsonStr, defineType) as IDefine;
                        define.path = scriptPath;

                        type2Define[defineType].Add(define);
                    });
                }
            }
        }

        public IEnumerable<T> GetDefines<T>()
            where T: IDefine
        {
            return type2Define[typeof(T)].OfType<T>();
        }

        public static void RegisterDefine<T>(string path)
            where T: IDefine
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

    public interface IDefine
    {
        string path { get; set; }
    }
}
