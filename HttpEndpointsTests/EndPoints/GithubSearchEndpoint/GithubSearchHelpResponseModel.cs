using System.Collections.Generic;
using Newtonsoft.Json;

namespace HttpEndpointsTests.EndPoints.GithubSearchEndpoint
{
    public class GithubSearchHelpResponseModel
    {
        public IEnumerable<Entity> Entries { get; set; }

        public class Entity
        {
            public string Body { get; set; }

            public string Category { get; set; }

            [JsonProperty("category_url")]
            public string CategoryUrl { get; set; }

            public string Excerpt { get; set; }

            public string Title { get; set; }

            public string Url { get; set; }
        }
    }
}
