using Azure;
using Azure.AI.OpenAI;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text;

namespace AnalyticsReport.Manager
{
    public class ChatManager
    {
        private readonly IConfiguration _config;

        private string _apiKey = "2853f7e93506403180d81f84400c5abd";

        private string _endPoint = "https://dinexreportengine.openai.azure.com/";

        private string _modelName = "AtechGPT-3";

        private string _filePath = "wwwroot/assets/uploads/";

        private string _apiEndPoint = "https://api.openai.com/v1/chat/completions";

        public ChatManager(IConfiguration _config)
        {
            this._config = _config;
        }

        public object SQLCommandExecuteQuery(string query)
        {
            var connection = new SqlConnection(_config.GetSection("ConnectionStrings").GetValue<string>("CentralDb"));
            SqlCommand cmd = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader DataReader = cmd.ExecuteReader();

            return DataReader;
        }

        public OpenAIClient CredentialsOpenAI()
        {
            OpenAIClient client = new OpenAIClient(new Uri(_endPoint), new AzureKeyCredential(_apiKey));

            return client;
        }

        public async Task<object> GetInsights(Chat prompt)
        {
            var client = CredentialsOpenAI();

            string fileContent = File.ReadAllText(_filePath + "dataset.csv");

            Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync(_modelName,
                new ChatCompletionsOptions()
                {
                    Messages =
                    {
                        new ChatMessage(ChatRole.System, @"You are an AI assistant power by ATECH GROUP CO., LTD. that helps people find information."),
                        new ChatMessage(ChatRole.User, prompt + fileContent),
                    },
                    Temperature = (float)0.7,
                    MaxTokens = 800,
                    NucleusSamplingFactor = (float)0.95,
                    FrequencyPenalty = 0,
                    PresencePenalty = 0,
                });

            ChatCompletions response = responseWithoutStream.Value;
            //return completions.Choices[0].Message.Content;

            return response;
        }

        public async Task<object> GetFineTuning()
        {
            string apiKey = "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";
            string model = "gpt-3.5-turbo";

            var dataset = LoadDatasetFromCsv();

            List<FineTuneModel> trainingData = PrepareTrainingData(dataset);

            var response = await FineTuneModel(model, apiKey, trainingData);

            return response;
        }

        public List<ChatExample> LoadDatasetFromCsv()
        {
            List<ChatExample> dataset = new List<ChatExample>();

            var reader = new StreamReader(_filePath + "train.csv");
            var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);

            while (csv.Read())
            {
                string input = csv.GetField<string>(0);
                string output = csv.GetField<string>(1);

                dataset.Add(new ChatExample { Input = input, Output = output });
            }

            return dataset;
        }

        public List<FineTuneModel> PrepareTrainingData(List<ChatExample> dataset)
        {
            List<FineTuneModel> fineTune = new List<FineTuneModel>();

            foreach (var example in dataset)
            {
                fineTune.Add(new FineTuneModel
                {
                    Prompt = example.Input,
                    Completion = example.Output
                });
            }

            return fineTune;
        }

        public async Task<object> FineTuneModel(string modelName, string apiKey, List<FineTuneModel> trainingData)
        {
            var client = new HttpClient();

            var requestData = new
            {
                model = modelName,
                examples = trainingData
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await client.PostAsync(_apiEndPoint, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }
    }

    public class ChatExample
    {
        public string? Input { get; set; }
        public string? Output { get; set; }
    }
}
