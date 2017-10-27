using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpEndpointsTests.EndPoints.Base
{
    public abstract class HttpEndPoint<TResponseModel> where TResponseModel : class
    {
        public class HttpEndPointResult
        {
            public TResponseModel ResponseModel { get; set; }

            public HttpResponseHeaders ResponseHeaders { get; set; }

            public HttpStatusCode HttpStatusCode { get; set; }
        }

        public abstract string Uri { get; }

        public abstract string RequestUri { get; }

        public abstract HttpMethod HttpMethod { get; }

        public Dictionary<string, string> RequestHeaders { get; set; } = new Dictionary<string, string>();

        public async Task<HttpEndPointResult> GetResult()
        {
            var httpEndpointResult = new HttpEndPointResult();

            TResponseModel responseModel;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri, UriKind.Absolute);
                var requestMessage = CreateRequest();

                AddHeadersToRequest(requestMessage);

                var response = await client.SendAsync(requestMessage);

                httpEndpointResult.ResponseHeaders = response.Headers;
                httpEndpointResult.HttpStatusCode = response.StatusCode;

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

            httpEndpointResult.ResponseModel = responseModel;

            return httpEndpointResult;
        }

        public HttpEndPoint<TResponseModel> WithHeader(string header, string headerValue)
        {
            RequestHeaders.Add(header, headerValue);

            return this;
        }

        protected virtual HttpRequestMessage CreateRequest()
        {
            return new HttpRequestMessage(HttpMethod, RequestUri ?? "/");
        }

        private void AddHeadersToRequest(HttpRequestMessage requestMessage)
        {
            foreach (var header in RequestHeaders)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }
    }
}