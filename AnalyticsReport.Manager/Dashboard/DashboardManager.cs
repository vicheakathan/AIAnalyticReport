using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace AnalyticsReport.Manager
{
    public class DashboardManager
    {
        private readonly IConfiguration _configuration;

        public DashboardManager(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        public object SQLCommandExecuteQuery(string query)
        {
            var connection = new SqlConnection(_configuration.GetSection("ConnectionStrings").GetValue<string>("CentralDb"));
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader DataReader = cmd.ExecuteReader();

            return DataReader;
        }

        public object PerformanceByDay(string? tenantName)
        {
            DateTime date = DateTime.Today;
            var firstDayOfTheWeek = date.AddDays(-(int)date.DayOfWeek);
            var lastDayOfTheWeek = firstDayOfTheWeek.AddDays(6);

            string startDate = Convert.ToDateTime(firstDayOfTheWeek).ToString("yyyy-MM-dd");
            string endDate = Convert.ToDateTime(lastDayOfTheWeek.AddDays(1)).ToString("yyyy-MM-dd");

            List<string> month = new List<string>();
            for (DateTime current = new DateTime(date.Year, date.Month, 1); current.Month == date.Month; current = current.AddDays(1))
            {
                if (current >= firstDayOfTheWeek && current <= lastDayOfTheWeek)
                {
                    month.Add(current.ToString("dddd"));
                }
            }

            string query = @"
                SELECT CONVERT(CHAR(10),t1.OrderDateTime,120) AS OrderDateTime, SUM(t1.TotalDiscount) AS Discount, SUM(t1.GrandTotal) AS GrandTotal
                FROM SaleTransactions AS t1
                LEFT JOIN SaleRequests AS t2 ON t1.RequestId = t2.Id
                LEFT JOIN Tanants AS t3 ON t2.TanantId = t3.Id 
                WHERE t1.OrderDateTime BETWEEN CONVERT(DATE,'" + startDate + "') AND CONVERT(DATE,'" + endDate + "') " +
                "AND t3.Name = '" + tenantName + "'" +
                "GROUP BY CONVERT(CHAR(10),t1.OrderdateTime,120)" +
                "ORDER BY CONVERT(CHAR(10),t1.OrderDateTime,120) ASC";

            SqlDataReader DataReader = (SqlDataReader)SQLCommandExecuteQuery(query);
            var charts = new List<SaleSummary>();
            var dashboard = new List<Dashboard>();

            while (DataReader.Read())
            {
                dashboard.Add(new Dashboard
                {
                    OrderDateTime = Convert.ToDateTime(DataReader["OrderDateTime"]).ToString("dddd"),
                    GrandTotal = Math.Round((decimal)DataReader["GrandTotal"], 2)
                });
            }

            List<decimal> listData = new List<decimal>();

            foreach (var months in month)
            {
                var x = 0;
                decimal data = 0;

                foreach (var item in dashboard)
                {
                    data = dashboard.Where(x => x.OrderDateTime == months.ToString()).Sum(x => x.GrandTotal);

                    if (months.ToString() == item.OrderDateTime)
                    {
                        x = 1;
                        break;
                    }
                }

                if (x == 1)
                {
                    listData.Add(data);
                }
                else
                {
                    //listData.Add(0);
                    //listDiscount.Add(0);
                }
            }

            charts.Add(new SaleSummary { Label = "Total Revenue", Data = listData.ToArray(), Type = "bar" });

            return new ChartsData { SaleSummary = charts, ChartLabels = month.ToArray() };
        }
    }
}
