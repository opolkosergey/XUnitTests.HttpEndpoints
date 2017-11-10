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
        
        private List<string> ExcludedRequestHeaders { get; } = new List<string>();

        public async Task<HttpEndPointResult> GetResult()
        {
            HttpEndPointResult httpEndpointResult;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri, UriKind.Absolute);
                
                var requestMessage = CreateRequestMessage();

                AddHeadersToRequest(requestMessage);
                
                if (!ExcludedRequestHeaders.Contains("Authorization") && ExcludedRequestHeaders.Contains("Cookie"))
                {
                    Authorize(client, requestMessage);
                }
                
                RemoveRequestHeaders(requestMessage);

                var response = await client.SendAsync(requestMessage);

                httpEndpointResult = await CreateHttpEndPointResult(response);
            }

            return httpEndpointResult;
        }

        public HttpEndPoint<TResponseModel> WithHeader(string header, string headerValue)
        {
            RequestHeaders.Add(header, headerValue);
            return this;
        }
        
        public HttpEndPoint<TResponseModel> WithoutHeader(string header)
        {
            RequestHeaders.Remove(header);
            return this;
        }

        protected virtual void Authorize(HttpClient httpClient, HttpRequestMessage httpRequestMessage) => ExcludedRequestHeaders.Remove("Authorization");       

        protected virtual HttpRequestMessage CreateRequestMessage() => new HttpRequestMessage(HttpMethod, RequestUri ?? "/");

        private async Task<HttpEndPointResult> CreateHttpEndPointResult(HttpResponseMessage response)
        {
            var httpEndpointResult = new HttpEndPointResult
            {
                ResponseHeaders = response.Headers,
                HttpStatusCode = response.StatusCode
            };

            var content = await response.Content.ReadAsStringAsync();

            TResponseModel responseModel;
            if (typeof(TResponseModel) == typeof(string))
            {
                responseModel = content as TResponseModel;
            }
            else
            {
                responseModel = JsonConvert.DeserializeObject<TResponseModel>(content);
            }

            httpEndpointResult.ResponseModel = responseModel;
            
            return httpEndpointResult;
        }
        
        private void AddHeadersToRequest(HttpRequestMessage requestMessage)
        {
            foreach (var header in RequestHeaders)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        private void RemoveRequestHeaders(HttpRequestMessage requestMessage) =>
            ExcludedRequestHeaders.ForEach(header =>
            {
                if (requestMessage.Headers.Contains(header))
                {
                    requestMessage.Headers.Remove(header);
                }                
            });        
    }
}