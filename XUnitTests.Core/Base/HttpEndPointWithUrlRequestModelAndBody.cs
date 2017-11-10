using System;
using System.Net.Http;
using XUnitTests.Core.Helpers;

namespace XUnitTests.Core.Base
{
    public abstract class HttpEndPointWithUrlRequestModelAndBody<TUrlRequestModel, TRequestBodyModel, TReponseModel> : HttpEndPoint<TReponseModel> where TReponseModel : class
    {
        private TUrlRequestModel UrlRequestModel { get; set; }

        private TRequestBodyModel Body { get; set; }

        public HttpEndPointWithUrlRequestModelAndBody<TUrlRequestModel, TRequestBodyModel, TReponseModel> WithUrlRequestModel(TUrlRequestModel urlRequestModel)
        {
            UrlRequestModel = urlRequestModel;

            return this;
        }
        
        public HttpEndPointWithUrlRequestModelAndBody<TUrlRequestModel, TRequestBodyModel, TReponseModel> WithRequestBodyModel(TRequestBodyModel requestBodyModel)
        {
            Body = requestBodyModel;

            return this;
        }

        protected override HttpRequestMessage CreateRequest()
        {
            if (string.IsNullOrEmpty(RequestUri))
            {
                throw new ArgumentException($"Parametr {nameof(RequestUri)} is required.");
            }

            string requestUri = RequestHelper.CreateUrlUsingRequestModel(RequestUri, UrlRequestModel) ?? "/";

            var request = new HttpRequestMessage(HttpMethod, requestUri);

            RequestHelper.AddRequestBody(request, Body);
            
            return request;
        }               
    }
}