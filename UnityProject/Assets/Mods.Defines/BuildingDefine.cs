using GMEngine;
using Newtonsoft.Json;
using System.IO;

namespace Mods.Defines
{
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
}
