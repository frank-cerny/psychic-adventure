using System;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using bike_selling_app.Application.Common.GraphQL.Types;
using System.Net.Http;
using System.Threading.Tasks;
using bike_selling_app.Domain.Entities;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace drivers
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient 
            {
                BaseAddress = new Uri("https://localhost:5001/graphql")
            };
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), client);
            var graphqlRequest = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                    bikes {
                            serialNumber
                        }
                    }",
                OperationName = "Test Operation",
                Variables = new Object{ }
            };
            var graphQLResponse = await graphClient.SendQueryAsync<BikeCollectionType>(graphqlRequest);
            // Console.WriteLine(graphQLResponse);
            Console.WriteLine(graphQLResponse.Data.Bikes[0].SerialNumber);
            // TODO - Setup a normal http request without GraphQL client (see what the response looks like)
            // https://stackoverflow.com/questions/6117101/posting-jsonobject-with-httpclient-from-web-api
            // var query = @"query TestQuery {
            //         bikes {
            //                 all {
            //                     serialNumber
            //                 }
            //             }
            //         }";
            // var d = new Dictionary<string, string>();
            // string myJson = "{ query:" + query + ", variables: {} }";
            // d["query"] = query;
            var queryObject = new 
            {
                query = @"query TestQuery {
                    bikes {
                            serialNumber
                        }
                    }",
                    variables = new { }
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(queryObject), Encoding.UTF8, "application/json")
            };
            // var response = await client.PostAsync("https://localhost:5001/graphql", new StringContent(myJson, Encoding.UTF8, "application/json"));
            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }
    }

    public class BikeCollectionType
    {
        public IList<Bike> Bikes { get; set; }
    }

    public class DataCollectionType<T>
    {
        public T Bikes { get; set; }
    }
}
