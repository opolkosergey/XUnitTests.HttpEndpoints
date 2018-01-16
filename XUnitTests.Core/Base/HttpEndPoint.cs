using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        private bool UseDefaultAuthorization = true;

        private Dictionary<string, string> RequestHeaders { get; } = new Dictionary<string, string>();       

        public async Task<HttpEndPointResult> GetResult()
        {
            HttpEndPointResult httpEndpointResult;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri, UriKind.Absolute);
                
                var requestMessage = CreateRequestMessage();

                AddHeadersToRequest(requestMessage);
                
                if (UseDefaultAuthorization)
                {
                    Authorize(client, requestMessage);
                }                               

                var response = await client.SendAsync(requestMessage);

                httpEndpointResult = await CreateHttpEndPointResult(response);
            }

            return httpEndpointResult;
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

        protected virtual void Authorize(HttpClient httpClient, HttpRequestMessage httpRequestMessage) { }

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

        private async Task<HttpEndPointResult> CreateHttpEndPointResult(HttpResponseMessage response)
        {
            var httpEndpointResult = new HttpEndPointResult
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
                IEnumerable<string> values;
                response.Headers.TryGetValues("content-type", out values);                
                string contentType = values?.SingleOrDefault() ?? "application/json";
                responseModel = DeserializeResponseModel(contentType, content);                
            }

            return responseModel;
        }

        private TResponseModel DeserializeResponseModel(string contentType, string content)
        {
            switch (contentType)
            {
                case "application/json":
                    return JsonConvert.DeserializeObject<TResponseModel>(content);
                case "text/xml":
                case "application/xml":
                    return DeserializeAsXML(content);
                default:
                    throw new Exception("Unable to deserialize response content.");
            }
        }

        private TResponseModel DeserializeAsXML(string content)
        {
            var serializer = new XmlSerializer(typeof(TResponseModel));
            TResponseModel result;
            using (var reader = new StringReader(content))
            {
                result = (TResponseModel)serializer.Deserialize(reader);
            }

            return result;
        }     
    }
}