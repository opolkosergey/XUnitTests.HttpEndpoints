using System;
using System.Net.Http;
using XUnitTests.Core.Helpers;

namespace XUnitTests.Core.Base
{
    public abstract class HttpEndPointWithUrlRequestModel<TUrlRequestModel, TReponseModel> : HttpEndPoint<TReponseModel> where TReponseModel : class
    {
        private TUrlRequestModel UrlRequestModel { get; set; }

        public HttpEndPointWithUrlRequestModel<TUrlRequestModel, TReponseModel> WithUrlRequestModel(TUrlRequestModel urlRequestModel)
        {
            UrlRequestModel = urlRequestModel;

            return this;
        }

        protected override HttpRequestMessage CreateRequest()
        {            
            if (string.IsNullOrEmpty(RequestUri))
            {
                throw new ArgumentException($"Parametr {nameof(RequestUri)} is required.");
            }

            string requestUri = RequestHelper.CreateUrlUsingRequestModel(RequestUri, UrlRequestModel) ?? "/";

            return new HttpRequestMessage(HttpMethod, requestUri);
        }
    }
}