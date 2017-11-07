using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace XUnitTests.Core.Base
{
    public abstract class HttpEndPoint<TResponseModel> where TResponseModel : class
    {
        public class HttpEndPointResult
        {
            public TResponseModel ResponseModel { get; set; }

            public HttpResponseHeaders ResponseHeaders { get; set; }

            public HttpStatusCode HttpStatusCode { get; set; }
        }

        protected abstract string Uri { get; }

        protected abstract string RequestUri { get; }

        protected abstract HttpMethod HttpMethod { get; }

        private Dictionary<string, string> RequestHeaders { get; } = new Dictionary<string, string>();
        
        private List<string> ExcludedRequestHeaders { get; }= new List<string>();

        public async Task<HttpEndPointResult> GetResult()
        {
            var httpEndpointResult = new HttpEndPointResult();

            TResponseModel responseModel;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri, UriKind.Absolute);
                var requestMessage = CreateRequest();

                AddHeadersToRequest(requestMessage);
                Authorize(client, requestMessage);
                RemoveRequestHeaders(requestMessage);

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

        protected virtual void Authorize(HttpClient httpClient, HttpRequestMessage httpRequestMessage)
        {
            ExcludedRequestHeaders.Remove("Authorization");
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
                
        private void RemoveRequestHeaders(HttpRequestMessage requestMessage)
        {
            foreach (var header in ExcludedRequestHeaders)
            {
                requestMessage.Headers.Remove(header);
            }
        }
    }
}