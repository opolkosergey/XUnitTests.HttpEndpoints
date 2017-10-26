using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace XUnitTests.EndPoints.GithubSearchEndpoint
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
