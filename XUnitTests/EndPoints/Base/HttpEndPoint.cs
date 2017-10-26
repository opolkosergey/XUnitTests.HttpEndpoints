using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XUnitTests.EndPoints.Base
{
    public abstract class HttpEndPoint<TResponseModel> where TResponseModel : class
    {
        public class HttpEndPointResult
        {
            public TResponseModel ResponseModel { get; set; }

            public HttpStatusCode HttpStatusCode { get; set; }
        }

        public abstract string Uri { get; }

        public abstract string RequestUri { get; }

        public abstract HttpMethod HttpMethod { get; }

        //public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        //public HttpEndPoint<TResponseModel> WithHeader(KeyValuePair<string, string> header)
        //{
        //    Headers.Add(header.Key, header.Value);

        //    return this;
        //}

        public async Task<HttpEndPointResult> GetResult()
        {
            TResponseModel responseModel;
            HttpStatusCode httpStatusCode;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri, UriKind.Absolute);
                var requestMessage = CreateRequest();

                var response = await client.SendAsync(requestMessage);

                httpStatusCode = response.StatusCode;

                var content = await response.Content.ReadAsStringAsync();

                if (typeof(TResponseModel) == typeof(string))
                {
                    responseModel = content as TResponseModel;
                }
                else
                {
                    responseModel = JsonConvert.DeserializeObject<TResponseModel>(content);
                }
            }

            return new HttpEndPointResult
            {
                ResponseModel = responseModel,
                HttpStatusCode = httpStatusCode
            };
        }

        protected virtual HttpRequestMessage CreateRequest()
        {
            return new HttpRequestMessage(HttpMethod, RequestUri ?? "/");            
        }        
    }
}