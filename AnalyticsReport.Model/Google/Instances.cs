using System;
using Newtonsoft.Json;

namespace AnalyticsReport.Model
{
    public class Instances
    {
        [JsonProperty("messages")]
        public ICollection<Messages> Messages { get; set; }

        public Instances()
        {
            Messages = new List<Messages>();
        }
    }
}

