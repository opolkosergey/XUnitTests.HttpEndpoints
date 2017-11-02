﻿using System.Net.Http;
using XUnitTests.Core.Helpers;

namespace XUnitTests.Core.Base
{
    public abstract class HttpEndPointWithRequestBody<TRequestBodyModel, TReponseModel> : HttpEndPoint<TReponseModel> where TReponseModel : class
    {
        private TRequestBodyModel Body { get; set; }

        public HttpEndPointWithRequestBody<TRequestBodyModel, TReponseModel> WithRequestBodyModel(TRequestBodyModel requestBodyModel)
        {
            Body = requestBodyModel;

            return this;
        }

        protected override HttpRequestMessage CreateRequest()
        {
            var request = base.CreateRequest();
            
            RequestHelper.AddRequestBody(request, Body);

            return request;
        }
    }
}