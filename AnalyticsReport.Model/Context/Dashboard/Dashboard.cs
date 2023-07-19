using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsReport.Model
{
    public class Dashboard
    {
        public string? OrderDateTime { get; set; }

        public string? Tenant { get; set; }

        public int Sales { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Vat { get; set; }

        public decimal GrandTotal { get; set; }

        public string? Currency { get; set; }

        public bool IncludeVat { get; set; }
    }
}
