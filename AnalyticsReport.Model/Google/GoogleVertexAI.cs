using System;
using Newtonsoft.Json;

namespace AnalyticsReport.Model
{
    public class GoogleVertexAI
    {
        [JsonProperty("instances")]
        public ICollection<Instances> Instances { get; set; }

        [JsonProperty("parameters")]
        public ICollection<Parameters> Parameters { get; set; }

        public GoogleVertexAI()
        {
            Instances = new List<Instances>();

            Parameters = new List<Parameters>();
        }
    }
    
}

