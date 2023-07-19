using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsReport.Model
{
    public class ChartsData
    {
        public List<SaleSummary>? SaleSummary { get; set; }

        public string[]? ChartLabels { get; set; }
    }
}
