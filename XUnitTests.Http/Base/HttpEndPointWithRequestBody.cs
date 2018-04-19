using System.Net.Http;
using XUnitTests.Http.Helpers;
using XUnitTests.Http.Interfaces;

namespace XUnitTests.Http.Base
{
    public abstract class HttpEndPointWithRequestBody<TRequestBodyModel, TReponseModel> : HttpEndPoint<TReponseModel>
        where TRequestBodyModel : ISerializableBodyModel
        where TReponseModel : class
    {
        private TRequestBodyModel Body { get; set; }

        public HttpEndPointWithRequestBody<TRequestBodyModel, TReponseModel> WithRequestBodyModel(TRequestBodyModel requestBodyModel)
        {
            Body = requestBodyModel;
            return this;
        }

        protected override HttpRequestMessage CreateRequestMessage()
        {
            var requestMessage = base.CreateRequestMessage();
            
            RequestHelper.AddRequestBody(requestMessage, Body);

            return requestMessage;
        }
    }
}