using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace XUnitTests.Core.Helpers
{
    internal static class RequestHelper
    {
        public static void AddRequestBody(HttpRequestMessage httpRequestMessage, object body)
        {
            if (body != null)
            {
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }
        }

        public static string CreateUrlUsingRequestModel<T>(T requestModel)
        {
            if (requestModel == null)
            {
                return null;
            }
           
            var collectionValues = new List<string>();
            var properties = requestModel.GetType().GetProperties();
            
            foreach (var property in properties)
            {
                var value = property.GetValue(requestModel);

                if (value != null)
                {
                    var isIEnumerable = value is IEnumerable;

                    if (isIEnumerable)
                    {
                        var collection = value as IEnumerable;
                                                
                        foreach (var item in collection)
                        {
                            collectionValues.Add($"{property.Name}={item.ToString()}");
                        }
                    }
                    else
                    {
                        collectionValues.Add($"{property.Name}={value.ToString()}");
                    }
                }
            }
            
            return collectionValues.Any() ? string.Join("&", collectionValues) : null;
        }
    }
}