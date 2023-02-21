using Hjson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GMEngine
{
    public class Mods
    {
        public IEnumerable<Image> images { get; }

        private Dictionary<Type, List<IDefine>> type2Define = new Dictionary<Type, List<IDefine>>();
        
        public Mods(string root)
        {
            images = Directory.EnumerateFiles(root, "*.png", SearchOption.AllDirectories)
                .Select(imgPath => new Image(imgPath))
                .ToArray();

            var path2Type = GernerateScriptType();
            foreach (var dir in Directory.EnumerateDirectories(root))
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

        private Dictionary<string, Type> GernerateScriptType()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name == "Mods");
            var defineTypes = assembly.GetTypes().Where(x => typeof(IDefine).IsAssignableFrom(x));
            return defineTypes.ToDictionary(t => t.GetCustomAttribute<DefineProperty>().scriptPath, t => t);
        }

        public IEnumerable<T> GetDefines<T>()
            where T: IDefine
        {
            return type2Define[typeof(T)].OfType<T>();
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

    public class Image
    {
        public string path { get; set; }
        public byte[] bytes { get; set; }

        public Image(string path)
        {
            this.path = path;
            this.bytes = File.ReadAllBytes(path);
        }

    }

    public class DefineProperty : Attribute
    {
        public string scriptPath;
        public DefineProperty(string scriptPath)
        {
            this.scriptPath = scriptPath;
        }
    }
}
