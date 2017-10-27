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
    }
}
