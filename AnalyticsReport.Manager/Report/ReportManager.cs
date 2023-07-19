using System.Data.SqlClient;
using System.Formats.Asn1;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI_API;

namespace AnalyticsReport.Manager
{
    public class ReportManager
    {
        private readonly IConfiguration _config;

        private string _apiKey = "2853f7e93506403180d81f84400c5abd";

        private string _endPoint = "https://dinexreportengine.openai.azure.com/";

        private string _modelName = "AtechGPT-3";

        private string _filePath = "wwwroot/assets/uploads/";

        private readonly ApplicationDbContext _context;

        public ReportManager(IConfiguration _config, ApplicationDbContext _context)
        {
            this._config = _config;
            this._context = _context;
        }

        public object SQLCommandExecuteQuery(string query)
        {
            var connection = new SqlConnection(_config.GetSection("ConnectionStrings").GetValue<string>("CentralDb"));
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader DataReader = cmd.ExecuteReader();

            return DataReader;
        }

        public async Task<object> GetInsights(string? prompt)
        {
            string apiKey = "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";

            var client = new OpenAIAPI(apiKey);

            return null;
        }

        public async Task<object> GetFineTuning(string? prompt)
        {
            string apiKey = "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";
            string model = "gpt-3.5-turbo";
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            HttpClient _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new
            {
                prompt = prompt,
                model = model
            };

            var json = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);

            string responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        public async Task<List<string>> RetrieveDataAsync(string query)
        {
            List<string> dataset = new List<string>();

            SqlDataReader reader = (SqlDataReader)SQLCommandExecuteQuery(query);

            while (await reader.ReadAsync())
            {
                //dataset.Add(new Test
                //{
                //    Date = reader.GetDateTime("OrderDateTime"),
                //    Channel = reader.GetString(reader.GetOrdinal("ChannelName")),
                //    GrandTotal = reader.GetDecimal(reader.GetOrdinal("GrandTotal")),
                //});
                dataset.Add(reader.GetString(reader.GetOrdinal("ChannelName")));
            }

            return dataset;
        }
    }
}
