using System;
using System.Net.Http.Headers;

namespace AnalyticsReport.Manager
{
	public class DinexMachineLearningManager
	{
        private string _apiKey = "39gkDTWB0n5hSmPGW4CSFCdrSw198PQW";

        private string _endPoint = "https://dinex-machine-learning-hrxnm.southeastasia.inference.ml.azure.com/score";

        public DinexMachineLearningManager()
		{
		}

        public async Task<object> InvokeRequestResponseService()
        {
            // apiKey, endPoint from Azure Machine Learning

            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            var client = new HttpClient(handler);
            var requestBody = "{\"question\": \"Hello\"}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            client.BaseAddress = new Uri(_endPoint);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            content.Headers.Add("azureml-model-deployment", "blue");

            HttpResponseMessage response = await client.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}

