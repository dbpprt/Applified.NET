using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Applified.Core.Extensibility
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigurationModel
    {
        [JsonProperty("featureName")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
