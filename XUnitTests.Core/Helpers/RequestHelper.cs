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
        // TODO add parameter for content type.
        public static void AddRequestBody(HttpRequestMessage httpRequestMessage, object body)
        {
            if (body != null)
            {
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }
        }

        public static string CreateUrlUsingRequestModel<T>(string requestUri, T requestModel)
        {
            if (requestModel == null)
            {
                return requestUri;
            }

            var keyValueParameters = new List<string>();
            var uriSegments = GetUriSegments(requestUri);
            
            var properties = requestModel.GetType().GetProperties();            
            foreach (var property in properties)
            {
                var value = property.GetValue(requestModel);

                if (value != null)
                {
                    var uriSegment = uriSegments.SingleOrDefault(x => x == property.Name.ToLower());                    
                    if (uriSegment != null)
                    {
                        requestUri = requestUri.Replace("{" + uriSegment + "}", value.ToString());
                    }
                    else
                    {
                        AddModelValueAsParameter<T>(value, keyValueParameters, property);
                    }
                }
            }
            
            return keyValueParameters.Any() ? $"{requestUri}?{string.Join("&", keyValueParameters)}" : requestUri;
        }

        private static void AddModelValueAsParameter<T>(object value, List<string> keyValueParameters, PropertyInfo property)
        {
            if (value is IEnumerable)
            {
                var collection = value as IEnumerable;
                foreach (var item in collection)
                {
                    keyValueParameters.Add($"{property.Name}={item.ToString()}");
                }
            }
            else
            {
                keyValueParameters.Add($"{property.Name}={value.ToString()}");
            }
        }

        private static List<string> GetUriSegments(string requestUri)
        {
            var uriSegments = new List<string>();

            while (true)
            {
                var startIndex = requestUri.IndexOf('{');
                var endIndex = requestUri.IndexOf('}');

                if (startIndex != -1 && endIndex != -1)
                {
                    uriSegments.Add(requestUri.Substring(startIndex + 1, endIndex - startIndex - 1).ToLower());                    
                    requestUri = requestUri.Substring(endIndex + 1);
                }
                else
                {
                    break;
                }                
            }            

            return uriSegments;
        }
    }
}