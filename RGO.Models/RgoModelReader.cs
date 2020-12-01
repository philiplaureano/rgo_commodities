using System.IO;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RGO.Models
{
    public class RgoModelReader
    {
        public SolarSystem[] LoadFrom(string markup)
        {
            return JsonConvert.DeserializeObject<SolarSystem[]>(markup);
        }
    }
}