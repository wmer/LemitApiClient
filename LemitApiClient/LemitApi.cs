using LemitApiClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemitApiClient {
    public class LemitApi {
        private ApiConsumerHelper _consumingApiHelper;
        private string _baseApi = "https://lemitti.com/api/v1/consulta/";

        public LemitApi(string apiKey) {
            _consumingApiHelper = new ApiConsumerHelper(_baseApi, apiKey);
        }


    }
}
