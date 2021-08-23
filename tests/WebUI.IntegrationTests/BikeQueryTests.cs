using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GraphQL.SystemTextJson;
using GraphQL.Client.Serializer.SystemTextJson;
using GraphQL.Client.Http;
using bike_selling_app.Application.Common.GraphQL.Types;
using FluentAssertions;

namespace bike_selling_app.WebUI.IntegrationTests
{
    public class BikeQueryTests
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public BikeQueryTests()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestGetAllBikes()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql")}, new SystemTextJsonSerializer(), _client);
            var request = new GraphQLHttpRequest 
            {
                Query = @"query TestQuery {
                    bikes {
                            all {
                                serialNumber
                            }
                        }
                    }",
                OperationName = "Test Operation"
            };
            var response = await graphClient.SendQueryAsync<BikeType>(request);
            // HttpRequestMessage message = new HttpRequestMessage();
            // var response = await _client.PostAsync("/graphql", message.Content);
            // response.EnsureSuccessStatusCode();
            response.Should().Be("hey");
            Assert.Pass();
        }
    }
}