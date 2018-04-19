using System;
using System.Net.Http;
using XUnitTests.Http.Helpers;

namespace XUnitTests.Http.Base
{
    public abstract class HttpEndPointWithUrlRequestModel<TUrlRequestModel, TReponseModel> : HttpEndPoint<TReponseModel>
        where TReponseModel : class         
    {
        private TUrlRequestModel UrlRequestModel { get; set; }

        public HttpEndPointWithUrlRequestModel<TUrlRequestModel, TReponseModel> WithUrlRequestModel(TUrlRequestModel urlRequestModel)
        {
            UrlRequestModel = urlRequestModel;
            return this;
        }

        protected override HttpRequestMessage CreateRequestMessage()
        {            
            if (string.IsNullOrEmpty(RequestUri))
            {
                throw new ArgumentException($"Parameter {nameof(RequestUri)} is required.");
            }

            var requestUri = RequestHelper.CreateUrlUsingRequestModel(RequestUri, UrlRequestModel) ?? "/";

            return new HttpRequestMessage(HttpMethod, requestUri);
        }
    }
}