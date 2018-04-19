using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;            
using System.Threading.Tasks;
using XUnitTests.Http.Interfaces;
using XUnitTests.Http.Models;
using XUnitTests.Http.ResponseDeserializers;

namespace XUnitTests.Http.Base
{
    public abstract class HttpEndPoint<TResponseModel> where TResponseModel : class
    {        
        protected abstract string Uri { get; }

        protected abstract string RequestUri { get; }

        protected abstract HttpMethod HttpMethod { get; }

        private bool UseDefaultAuthorization = true;

        private Dictionary<string, string> RequestHeaders { get; } = new Dictionary<string, string>();       

        public async Task<HttpEndPointResult<TResponseModel>> GetResult()
        {
            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Uri, UriKind.Absolute);

                var requestMessage = CreateRequestMessage();

                AddHeadersToRequest(requestMessage);

                if (UseDefaultAuthorization)
                {
                    Authorize(httpClient, requestMessage);
                }

                var response = await httpClient.SendAsync(requestMessage);

                return await CreateHttpEndPointResult(response);
            }
        }

        public HttpEndPoint<TResponseModel> PreventDefaultAuthorization()
        {
            UseDefaultAuthorization = false;
            return this;
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

        protected virtual void Authorize(HttpClient httpClient, HttpRequestMessage httpRequestMessage){ }

        protected virtual HttpRequestMessage CreateRequestMessage()
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

        private async Task<HttpEndPointResult<TResponseModel>> CreateHttpEndPointResult(HttpResponseMessage response)
        {
            var httpEndpointResult = new HttpEndPointResult<TResponseModel>
            {
                ResponseHeaders = response.Headers,
                HttpStatusCode = response.StatusCode
            };

            var content = await response.Content.ReadAsStringAsync();

            httpEndpointResult.ResponseModel = GetResponseModel(response, content);

            return httpEndpointResult;
        }

        private TResponseModel GetResponseModel(HttpResponseMessage response, string content)
        {
            TResponseModel responseModel = null;

            if (typeof(TResponseModel) == typeof(string))
            {
                responseModel = content as TResponseModel;
            }
            else
            {
                response.Headers.TryGetValues("content-type", out var values);                
                string contentType = values?.SingleOrDefault() ?? "application/json";
                responseModel = DeserializeResponseModel(contentType, content);                
            }

            return responseModel;
        }

        private TResponseModel DeserializeResponseModel(string contentType, string content)
        {
            var desealizer = GetResponseDesealizer(contentType);

            return desealizer.Deserialize<TResponseModel>(content);
        }
        
        private IResponseDeserializer GetResponseDesealizer(string contentType)
        {
            switch (contentType)
            {
                case "application/json":
                    return new JsonDeserializer();
                case "text/xml":
                case "application/xml":
                    return new XmlDeserializer();
                default:
                    throw new Exception($"Unable to deserialize response content with '{contentType}' content type.");
            }
        }
    }
}