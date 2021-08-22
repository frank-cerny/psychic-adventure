using System.Collections.Generic;
using System.Text.Json.Serialization;
using GraphQL.SystemTextJson;

namespace bike_selling_app.Domain.GraphQL
{
    public class GraphQLRequest
    {
        public string Query { get; set; }

        // TODO - ObjectDictionaryConverter is deprecated, update it
        [JsonConverter(typeof(ObjectDictionaryConverter))]
        public Dictionary<string, object> Variables
        {
            get; set;
        }
    }
}