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
    [Collection("Test Collection")]
    public class NonCapitalItemTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        public NonCapitalItemTests(HttpServerFixture fixture)
        {
            // While this will get called each test, the Fixture will only create the factory a single time 
            // This means we use the same database instance for each test, so each test should be idempotent
            _factory = fixture.Factory;
            fixture.ResetDb();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task TestAddNonCapitalItem()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            // Create an expense item first (to be nested in the non capital item)
            var expenseMutation = new GraphQLHttpRequest
            {
                Query = @"mutation createExpenseItem($expenseItem: ExpenseItemInputType!) {
                            addExpenseItem(expenseItem : $expenseItem) {
                                name
                                unitCost
                                units
                                description
                            }
                        }",
                OperationName = "Add ExpenseItem",
                Variables = new
                {
                    expenseItem = new
                    {
                        name = "Postage",
                        unitCost = 5.67,
                        units = 5,
                        parentItemId = 10,
                        datePurchased = "07-15-2021"
                    }
                }
            };
            var mutationResponse = await graphClient.SendMutationAsync<AddExpenseItemType>(expenseMutation);
            // Now create a non capital item
            var nonCapitalItemMutation = new GraphQLHttpRequest
            {
                Query = @"mutation createExpenseItem($nonCapitalItem: NonCapitalItemInputType!) {
                            addNonCapitalItem(nonCapitalItem : $nonCapitalItem) {
                                name
                                unitCost
                                units
                                description
                            }
                        }",
                OperationName = "Add NonCapitalItem",
                Variables = new
                {
                    expenseItem = new
                    {
                        name = "Brake Cable",
                        unitCost = 5.67,
                        units = 5,
                        datePurchased = "07-15-2021",
                        expenseItemIds = new List<int>() { mutationResponse.Data.addExpenseItem.Id }
                    }
                }
            };
            var nonCapitalItemResponse = await graphClient.SendMutationAsync<AddNonCapitalItemType>(nonCapitalItemMutation);
            nonCapitalItemResponse.Data.addNonCapitalItem.Name.Should().Be("Brake Cable");
            // Now query for the item to ensure it is in the database
            var query = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                            nonCapitalItems {
                                name
                                expenseItems {
                                    name
                                }
                            }
                        }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<NonCapitalItemCollectionType>(query);
            queryResponse.Data.NonCapitalItems.Should().HaveCount(1);
            queryResponse.Data.NonCapitalItems[0].Name.Should().Be("Brake Cable");
            queryResponse.Data.NonCapitalItems[0].ExpenseItems.Should().HaveCount(1);
        }

        [Fact]
        public async Task TestRemoveNonCapitalItem()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            // Add a bike first
            var nonCapitalItemMutation = new GraphQLHttpRequest
            {
                Query = @"mutation createExpenseItem($nonCapitalItem: NonCapitalItemInputType!) {
                            addNonCapitalItem(nonCapitalItem : $nonCapitalItem) {
                                name
                                unitCost
                                units
                                description
                            }
                        }",
                OperationName = "Add NonCapitalItem",
                Variables = new
                {
                    expenseItem = new
                    {
                        name = "Brake Cable",
                        unitCost = 5.67,
                        units = 5,
                        datePurchased = "07-15-2021",
                    }
                }
            };
            var nonCapitalItemResponse = await graphClient.SendMutationAsync<AddNonCapitalItemType>(nonCapitalItemMutation);
            nonCapitalItemResponse.Data.addNonCapitalItem.Name.Should().Be("Brake Cable");
            // Now delete it
            var deleteMutation = new GraphQLHttpRequest
            {
                Query = @"mutation deleteNonCapitalItem($id: Int!) {
                            removeNonCapitalItem(id: $id) {
                                name
                            }
                        }",
                OperationName = "Remove non-capital item",
                Variables = new
                {
                    id = nonCapitalItemResponse.Data.addNonCapitalItem.Id
                }
            };
            var deleteMutationResponse = await graphClient.SendMutationAsync<RemoveNonCapitalItemType>(deleteMutation);
            deleteMutationResponse.Data.removeNonCapitalItem.Name.Should().Be("Brake Cable");
            // Now run a get query to ensure everything took hold in the database
            var query = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                            nonCapitalItems {
                                name
                                unitCost
                            }
                        }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<NonCapitalItemCollectionType>(query);
            queryResponse.Data.NonCapitalItems.Should().HaveCount(0);
        }

        [Fact]
        public async Task TestUpdateExpenseItem()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            // Add a non-capital item first
            var nonCapitalItemMutation = new GraphQLHttpRequest
            {
                Query = @"mutation createExpenseItem($nonCapitalItem: NonCapitalItemInputType!) {
                            addNonCapitalItem(nonCapitalItem : $nonCapitalItem) {
                                name
                                unitCost
                                units
                                description
                            }
                        }",
                OperationName = "Add NonCapitalItem",
                Variables = new
                {
                    expenseItem = new
                    {
                        name = "Brake Cable",
                        unitCost = 5.67,
                        units = 5,
                        datePurchased = "07-15-2021",
                    }
                }
            };
            var nonCapitalItemResponse = await graphClient.SendMutationAsync<AddNonCapitalItemType>(nonCapitalItemMutation);   
            // Now update it 
            var updateMutation = new GraphQLHttpRequest
            {
                Query = @"mutation updateNonCapitalItem($id: Int!, $nonCapitalItem: NonCapitalItemInputType!) {
                            updateExpenseItem(id : $id, nonCapitalItem : $nonCapitalItem) {
                                name
                                unitCost
                                description
                            }
                        }",
                OperationName = "Update non-capital item",
                Variables = new
                {
                    id = nonCapitalItemResponse.Data.addNonCapitalItem.Id,
                    expenseItem = new
                    {
                        name = "Tape",
                        unitCost = 1.25,
                        units = 2,
                        description = "A new, updated item!",
                        datePurchased = "07-15-2020"
                    }
                }
            };
            var updateMutationResponse = await graphClient.SendMutationAsync<UpdateNonCapitalItemType>(updateMutation);
            updateMutationResponse.Data.updateNonCapitalItem.Name.Should().Be("Tape");
            // Now run a get query to ensure everything took hold
            var query = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                            nonCapitalItems {
                                name
                                unitCost
                            }
                        }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<NonCapitalItemCollectionType>(query);
            queryResponse.Data.NonCapitalItems.Select(e => e.Name).Should().Contain("Tape");
            queryResponse.Data.NonCapitalItems.Select(e => e.Name).Should().NotContain("Brake Cable");
            queryResponse.Data.NonCapitalItems.Select(e => e.UnitCost).Should().Contain(1.25);
            queryResponse.Data.NonCapitalItems.Select(e => e.UnitCost).Should().NotContain(5.67);
        }
    }

    public class NonCapitalItemCollectionType
    {
        // This mocks a response that looks like
        // {
        //   "data": {
        //     "nonCapitalItems": [
        //       {
        //         "name": "Brake Cable",
        //         "units": 50,
        //         "unitCost": 15.67
        //       },
        //      ]
        //    }
        // }
        public IList<NonCapitalItem> NonCapitalItems { get; set; }
    }

    public class AddNonCapitalItemType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "addNonCapitalItem": {
        //       "name": "Postage"
        //     }
        //   }
        // }
        public NonCapitalItem addNonCapitalItem { get; set; }
    }

    public class RemoveNonCapitalItemType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "removeNonCapitalItem": {
        //       "name": "Postage"
        //     }
        //   }
        // }
        public NonCapitalItem removeNonCapitalItem { get; set; }
    }

    public class UpdateNonCapitalItemType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "updateNonCapitalItem": {
        //       "name": "Postage"
        //     }
        //   }
        // }
        public NonCapitalItem updateNonCapitalItem { get; set; }
    }
}