using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GraphQL.SystemTextJson;
using GraphQL.Client.Serializer.SystemTextJson;
using GraphQL.Client.Http;
using bike_selling_app.Application.Common.GraphQL.Types;
using FluentAssertions;
using Xunit;
using System;
using Xunit.Abstractions;
using bike_selling_app.Domain.Entities;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace bike_selling_app.WebUI.IntegrationTests
{
    public class BikeQueryTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        // private HttpClient _secondClient;
        // private HttpServerFixture _fixture;
        // private HttpClient _client;
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        // public BikeQueryTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
        // {
        //     // _fixture = fixture;
        //     // _fixture.OutputHelper = outputHelper;
        //     // _client = _fixture.CreateClient();
        // }

        public BikeQueryTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        // TODO - Look at this: https://github.com/mattjhosking/graphql-dotnet-execution-strategy
        // TODO - Let's start over with the WebApplicationFactory, but let it use a new database or something, no logging needed right now
        [Fact]
        public async Task TestGetAllBikes()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            var request = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                    bikes {
                            serialNumber
                        }
                    }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var response = await graphClient.SendQueryAsync<BikeCollectionType>(request);
            response.Data.Bikes[0].SerialNumber.Should().Be("1");
            // HttpRequestMessage message = new HttpRequestMessage();
            // var response = await _client.PostAsync("/graphql", message.Content);
            // response.EnsureSuccessStatusCode();
            // response.Should().Be("hey");
            var queryObject = new
            {
                query = @"query TestQuery {
                    bikes {
                            serialNumber
                        }
                    }",
                variables = new { }
            };
            var normalRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(queryObject), Encoding.UTF8, "application/json")
            };
            // var response = await client.PostAsync("https://localhost:5001/graphql", new StringContent(myJson, Encoding.UTF8, "application/json"));
            var normalResponse = await _client.SendAsync(normalRequest);
            var responseString = await normalResponse.Content.ReadAsStringAsync();
            responseString.Should().Be("hey");
        }
    }

    public class BikeCollectionType
    {
        public IList<Bike> Bikes { get; set; }
    }
}