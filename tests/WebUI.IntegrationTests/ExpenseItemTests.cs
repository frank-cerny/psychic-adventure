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
    public class ExpenseItemTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        // TODO - Add , ITestOutputHelper outputHelper back to constructor?
        public ExpenseItemTests(HttpServerFixture fixture)
        {
            // While this will get called each test, the Fixture will only create the factory a single time 
            // This means we use the same database instance for each test, so each test should be idempotent
            _factory = fixture.Factory;
            fixture.ResetDb();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task TestAddExpenseItem()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            var mutation = new GraphQLHttpRequest
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
            var mutationResponse = await graphClient.SendMutationAsync<AddExpenseItemType>(mutation);
            mutationResponse.Data.addExpenseItem.Name.Should().Be("Postage");
            mutationResponse.Data.addExpenseItem.Units.Should().Be(5);
            var query = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                            expenseItems {
                                name
                            }
                        }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<ExpenseItemCollectionType>(query);
            // Just validate that there are 3 bikes total, the application integration tests validate the database is fully correct
            queryResponse.Data.ExpenseItems.Should().HaveCount(1);
            queryResponse.Data.ExpenseItems[0].Name.Should().Be("Postage");
        }

        [Fact]
        public async Task TestRemoveExpenseItem()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            // Add a bike first
            var createMutation = new GraphQLHttpRequest
            {
                Query = @"mutation createExpenseItem($expenseItem: ExpenseItemInputType!) {
                            addExpenseItem(expenseItem : $expenseItem) {
                                id
                                name
                                unitCost
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
            var createMutationResponse = await graphClient.SendMutationAsync<AddExpenseItemType>(createMutation);
            createMutationResponse.Data.addExpenseItem.Name.Should().Be("Postage");
            var deleteMutation = new GraphQLHttpRequest
            {
                Query = @"mutation deleteExpenseItem($id: Int!) {
                            removeExpenseItem(id: $id) {
                                name
                            }
                        }",
                OperationName = "Remove expense item",
                Variables = new
                {
                    id = createMutationResponse.Data.addExpenseItem.Id
                }
            };
            var deleteMutationResponse = await graphClient.SendMutationAsync<RemoveExpenseItemType>(deleteMutation);
            deleteMutationResponse.Data.removeExpenseItem.Name.Should().Be("Postage");
            // Now run a get query to ensure everything took hold
            var query = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                            expenseItems {
                                name
                                unitCost
                            }
                        }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<ExpenseItemCollectionType>(query);
            queryResponse.Data.ExpenseItems.Should().HaveCount(0);
        }

        [Fact]
        public async Task TestUpdateExpenseItem()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            // Add an expense item first
            var createMutation = new GraphQLHttpRequest
            {
                Query = @"mutation createExpenseItem($expenseItem: ExpenseItemInputType!) {
                            addExpenseItem(expenseItem : $expenseItem) {
                                id
                                name
                                unitCost
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
            var createMutationResponse = await graphClient.SendMutationAsync<AddExpenseItemType>(createMutation);
            createMutationResponse.Data.addExpenseItem.Name.Should().Be("Postage");
            var updateMutation = new GraphQLHttpRequest
            {
                Query = @"mutation updateExpenseItem($id: Int!, $expenseItem: ExpenseItemInputType!) {
                            updateExpenseItem(id : $id, expenseItem : $expenseItem) {
                                name
                                unitCost
                                description
                            }
                        }",
                OperationName = "Update expense item",
                Variables = new
                {
                    id = createMutationResponse.Data.addExpenseItem.Id,
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
            var updateMutationResponse = await graphClient.SendMutationAsync<UpdateExpenseItemType>(updateMutation);
            updateMutationResponse.Data.updateExpenseItem.Name.Should().Be("Tape");
            // Now run a get query to ensure everything took hold
            var query = new GraphQLHttpRequest
            {
                Query = @"query TestQuery {
                            expenseItems {
                                name
                                unitCost
                            }
                        }",
                OperationName = "Test Operation",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<ExpenseItemCollectionType>(query);
            queryResponse.Data.ExpenseItems.Select(e => e.Name).Should().Contain("Tape");
            queryResponse.Data.ExpenseItems.Select(e => e.Name).Should().NotContain("Postage");
            queryResponse.Data.ExpenseItems.Select(e => e.UnitCost).Should().Contain(1.25);
            queryResponse.Data.ExpenseItems.Select(e => e.UnitCost).Should().NotContain(5.67);
        }
    }

    public class ExpenseItemCollectionType
    {
        // This mocks a response that looks like
        // {
        //   "data": {
        //     "expenseItems": [
        //       {
        //         "name": "Postage",
        //         "description": "Schwinn Debutante",
        //         "datePurchased": "07-15-2021"
        //       },
        //      ]
        //    }
        // }
        public IList<ExpenseItem> ExpenseItems { get; set; }
    }

    public class AddExpenseItemType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "addExpenseItem": {
        //       "name": "Postage"
        //     }
        //   }
        // }
        public ExpenseItem addExpenseItem { get; set; }
    }

    public class RemoveExpenseItemType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "removeExpenseItem": {
        //       "Name": "Postage"
        //     }
        //   }
        // }
        public ExpenseItem removeExpenseItem { get; set; }
    }

    public class UpdateExpenseItemType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "updateExpenseItem": {
        //       "Name": "Postage"
        //     }
        //   }
        // }
        public ExpenseItem updateExpenseItem { get; set; }
    }
}