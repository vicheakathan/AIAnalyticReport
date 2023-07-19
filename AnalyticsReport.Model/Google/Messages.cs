using System;
using Newtonsoft.Json;

namespace AnalyticsReport.Model
{
    public class Messages
    {
        [JsonProperty("author")]
        public string? Author { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }
    }
}

