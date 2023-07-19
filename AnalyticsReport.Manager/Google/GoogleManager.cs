using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsReport.Manager
{
    public class GoogleManager
    {
        private string _endPoint = "https://us-central1-aiplatform.googleapis.com/v1/projects/dinex-subscription/locations/asia-southeast2/publishers/google/models/chat-bison:predict";

        private HttpClient _httpClient = new HttpClient();

        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/auth";

        private const string TokenEndpoint = "https://accounts.google.com/o/oauth2/token";

        private string clientId = "471999407327-u1kpp39jafed33jjb7hkml255qp9ldsd.apps.googleusercontent.com";

        private string clientSecret = "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";

        private string redirectUri = "http://localhost:8080/";

        private string scope = "https://www.googleapis.com/auth/cloud-platform";

        public GoogleManager()
        {

        }

        public async Task<object> Chat(GoogleVertexAI entity)
        {
            var jsonContent = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "ya29.a0AbVbY6OmSqI1NTCCvXPFFf2IsMT0l-DK2JXuMz5McY295B0igl4lR6ARbSVmQ1h49CLO-H0nZtU8X3R4xKF-E37-xy6Ulw3NuQSXXLeDJfXlv3RMlfi20mcruiERf-Lly-MVAUyI20vAYFSH8Ye3UolT37g0aCgYKAR0SARISFQFWKvPlu7mxfSUQPXsWZYzLPt4S2A0163"); // token from google oauth2

            var response = await _httpClient.PostAsync(_endPoint, content);
            //response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject(responseBody);

            return result;
        }
    }

    
}
