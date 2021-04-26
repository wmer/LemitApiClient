using ManyHelpers.Strings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LemitApiClient.Helpers {
    public class ApiConsumerHelper {
        private HttpClient _client;
        private string _baseAdress;

        public ApiConsumerHelper(string baseAdress, string token) {
            _baseAdress = baseAdress;

            var handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true
            };

            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Token", token);
            _client.Timeout = TimeSpan.FromMinutes(30);
        }

        public (T result, string statusCode, string message) Get<T>(string endPoint) {
            return GetAssync<T>(endPoint).Result;
        }

        public async Task<(T result, string statusCode, string message)> GetAssync<T>(string endPoint) {
            try {
                var response = await _client.GetAsync($"{_client.BaseAddress}{endPoint}");
                var header = response.Headers;
                return DeserializeResponse<T>(response);
            } catch (Exception e) {
                return (default(T), null, e.Message);
            }
        }

        public (T result, string statusCode, string message) DeserializeResponse<T>(HttpResponseMessage response) {
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var statusCode = response.StatusCode.ToString();
            try {
                if (response.IsSuccessStatusCode) {
                    return (JsonConvert.DeserializeObject<T>(responseContent), statusCode, responseContent);
                } else {
                    return (default(T), statusCode, responseContent);
                }
            } catch (Exception e) {
                return (default(T), statusCode, responseContent);
            }

        }

        public HttpContent ObjectToHttpContent(object obj) {
            if (obj.GetType().IsPrimitive || obj.GetType() == typeof(string) || obj.GetType() == typeof(decimal)) {
                return new StringContent(obj.ToString());
            }
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
