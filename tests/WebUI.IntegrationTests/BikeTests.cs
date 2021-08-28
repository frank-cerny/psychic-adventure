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
using System.Linq;

namespace bike_selling_app.WebUI.IntegrationTests
{
    public class BikeTests : IClassFixture<HttpServerFixture>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        public BikeTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
        {
            // While this will get called each test, the Fixture will only create the factory a single time 
            _factory = fixture.Factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task TestGetAllBikes()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            var request = new GraphQLHttpRequest
            {
                Query = @"query GetAllBikes {
                            bikes {
                                serialNumber
                                make
                                model
                                purchasePrice
                                purchasedFrom
                                datePurchased
                            }
                        }",
                OperationName = "Get All Bikes",
                Variables = new object { }
            };
            var response = await graphClient.SendQueryAsync<BikeCollectionType>(request);
            // Validate that there are two bikes (don't need to validate every field)
            response.Data.Bikes.Should().HaveCount(2);
            var bike1 = response.Data.Bikes.SingleOrDefault(b => b.SerialNumber.Equals("2124893"));
            bike1.Make.Should().Be("Schwinn");
            bike1.PurchasePrice.Should().Be(21.45);
            bike1.DatePurchased.Should().Be(System.DateTime.Today);
            var bike2 = response.Data.Bikes.SingleOrDefault(b => b.SerialNumber.Equals("1236721-882, 89293, 899921"));
            bike2.Model.Should().Be("International");
            bike2.PurchasedFrom.Should().Be("Ebay");
            bike2.DatePurchased.Should().Be(System.DateTime.Parse("2020-08-02"));
        }

        [Fact]
        public async Task TestAddBike()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            var mutation = new GraphQLHttpRequest
            {
                Query = @"mutation createBike($bike: BikeInputType!) {
                            addBike(bike : $bike) {
                                serialNumber
                                model
                            }
                        }",
                OperationName = "Add Bike",
                Variables = new {
                    bike = new {
                        serialNumber = "738272",
                        make = "Specialized",
                        model = "Hard Rock",
                        purchasedFrom = "FB Marketplace",
                        purchasePrice = 45.99,
                        datePurchased = System.DateTime.Parse("2019-07-19")
                    }
                }
            };
            var mutationResponse = await graphClient.SendMutationAsync<AddBikeType>(mutation);
            mutationResponse.Data.addBike.SerialNumber.Should().Be("738272");
            mutationResponse.Data.addBike.Model.Should().Be("Hard Rock");
            var query = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                            bikes {
                                serialNumber
                            }
                        }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<BikeCollectionType>(query);
            // Just validate that there are 3 bikes total, the application integration tests validate the database is fully correct
            queryResponse.Data.Bikes.Should().HaveCount(3);
        }
    }

    public class BikeCollectionType
    {
        // This mocks a response that looks like
        // {
        //   "data": {
        //     "bikes": [
        //       {
        //         "serialNumber": "2124893",
        //         "model": "Tempo",
        //         "make": "Schwinn"
        //       },
        //      ]
        //    }
        // }
        public IList<Bike> Bikes { get; set; }
    }

    public class AddBikeType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "addBike": {
        //       "serialNumber": "123456758493572987"
        //     }
        //   }
        // }
        public Bike addBike { get; set; }
    }
}