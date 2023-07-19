using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsReport.Model
{
    public class SaleSummary
    {
        public string? Label { get; set; }

        public decimal[]? Data { get; set; }

        public string? Type { get; set; }
    }
}
