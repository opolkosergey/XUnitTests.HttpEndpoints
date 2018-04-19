using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using XUnitTests.Http.Interfaces;

namespace XUnitTests.Http.Helpers
{
    internal static class RequestHelper
    {
        public static void AddRequestBody(HttpRequestMessage httpRequestMessage, ISerializableBodyModel requestBodyModel)
        {
            if (requestBodyModel != null)
            {
                httpRequestMessage.Content = new StringContent(requestBodyModel.Serialize(), Encoding.UTF8, requestBodyModel.GetContentType());
            }
        }

        public static string CreateUrlUsingRequestModel<T>(string requestUri, T requestModel)
        {
            if (requestModel == null)
            {
                return requestUri;
            }
            
            var uriSegments = GetUriSegments(requestUri);

            return GetQueryString(requestUri, requestModel, uriSegments);
        }

        private static string GetQueryString<T>(string requestUri, T requestModel, List<string> uriSegments)
        {
            var keyValueParameters = new List<string>();
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
                        AddValueToUrlParameters<T>(property.Name, value, keyValueParameters);
                    }
                }
            }

            return keyValueParameters.Any() ? $"{requestUri}?{string.Join("&", keyValueParameters)}" : requestUri;
        }

        private static void AddValueToUrlParameters<T>(string propertyName, object value, List<string> keyValueUrlParameters)
        {
            if (value is IEnumerable)
            {
                var collection = value as IEnumerable;
                foreach (var item in collection)
                {
                    keyValueUrlParameters.Add($"{propertyName}={item.ToString()}");
                }
            }
            else
            {
                keyValueUrlParameters.Add($"{propertyName}={value.ToString()}");
            }
        }

        private static List<string> GetUriSegments(string requestUri)
        {
            var uriSegments = new List<string>();

            while (true)
            {
                var uriSegment = GetUriSegment(requestUri);
                if (uriSegment != null)
                {
                    uriSegments.Add(uriSegment.Item1.ToLower());                    
                    requestUri = requestUri.Substring(uriSegment.Item3 + 1);
                }
                else
                {
                    break;
                }                
            }            

            return uriSegments;
        }

        private static Tuple<string, int, int> GetUriSegment(string requestUri)
        {
            var segmentStartIndex = requestUri.IndexOf('{');
            var segmentEndIndex = requestUri.IndexOf('}');

            return (segmentStartIndex != -1 && segmentEndIndex != -1)
                ? Tuple.Create(requestUri.Substring(segmentStartIndex + 1, segmentEndIndex - segmentStartIndex - 1), segmentStartIndex, segmentEndIndex)
                : null;            
        }
    }
}