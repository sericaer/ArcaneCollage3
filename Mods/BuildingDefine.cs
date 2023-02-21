using GMEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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
        public ResourceCost[] constructionCost;

        [JsonProperty("maintenance_cost")]
        public ResourceCost[] maintenanceCost;
    }

    public class ResourceCost
    {
        public ResourceType type;
        public float value;
    }

    [JsonConverter(typeof(SafeStringEnumConverter), Unknown)]
    public enum ResourceType
    {
        Unknown,
        Cash,
        Stone,
        Crystal,
    }

    public class SafeStringEnumConverter : StringEnumConverter
    {
        public object DefaultValue { get; }

        public SafeStringEnumConverter(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
            catch
            {
                return DefaultValue;
            }
        }
    }
}