using System.Net.Http;
using System.Text;
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
            var requestUri = new StringBuilder();

            if (!string.IsNullOrEmpty(RequestUri))
            {
                requestUri.Append(RequestUri);
            }

            var requestModelUri = RequestHelper.CreateUrlUsingRequestModel(UrlRequestModel);

            if (requestModelUri == null)
            {
                requestUri.Append("/");                
            }
            else
            {
                requestUri.Append($"?{requestModelUri}");
            }
            
            return new HttpRequestMessage(HttpMethod, requestUri.ToString());
        }
    }
}