using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace HttpEndpointsTests.EndPoints.Base
{
    public abstract class HttpEndPointWithRequestBody<TRequestBodyModel, TReponseModel> : HttpEndPoint<TReponseModel> where TReponseModel : class
    {
        public TRequestBodyModel Body { get; set; }

        public HttpEndPointWithRequestBody<TRequestBodyModel, TReponseModel> WithRequestBodyModel(TRequestBodyModel requestBodyModel)
        {
            Body = requestBodyModel;

            return this;
        }

        protected override HttpRequestMessage CreateRequest()
        {
            var request = base.CreateRequest();

            if (Body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(Body), Encoding.UTF8, "application/json");
            }

            return request;
        }
    }
}