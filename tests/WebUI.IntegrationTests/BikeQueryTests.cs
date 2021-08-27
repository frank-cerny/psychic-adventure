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
    public class BikeQueryTests : IClassFixture<HttpServerFixture>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        public BikeQueryTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
        {
            // While this will get called each test, the Fixture will only create the factory a single time 
            _factory = fixture.Factory;
            _client = _factory.CreateClient();
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
        }
    }

    public class BikeCollectionType
    {
        public IList<Bike> Bikes { get; set; }
    }
}