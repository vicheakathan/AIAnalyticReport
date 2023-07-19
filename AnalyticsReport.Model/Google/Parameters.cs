using System;
using Newtonsoft.Json;

namespace AnalyticsReport.Model
{
    public class Parameters
    {
        [JsonProperty("maxOutputTokens")]
        public decimal MaxOutputTokens { get; set; }

        [JsonProperty("temperature")]
        public decimal Temperature { get; set; }
    }
}

