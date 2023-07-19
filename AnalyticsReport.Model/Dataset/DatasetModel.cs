using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsReport.Model
{
    public class DatasetModel
    {
        public DateTime OrderDate { get; set; }

        public string? Tenant { get; set; }

        public string? Group { get; set; }

        public string? Category { get; set; }

        public string? Item { get; set; }

        public string? ItemType { get; set; }

        public string? Size { get; set; }

        public decimal Qty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Sale { get; set; }

        public decimal Discount { get; set; }

        public string? OrderBy { get; set; }

        public string? Channel { get; set; }

        public string? Source { get; set; }
    }
}
