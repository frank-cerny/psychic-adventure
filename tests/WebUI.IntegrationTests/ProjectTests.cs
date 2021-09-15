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
    public class ProjectTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;

        public ProjectTests(HttpServerFixture fixture)
        {
            // While this will get called each test, the Fixture will only create the factory a single time 
            // This means we use the same database instance for each test, so each test should be idempotent
            _factory = fixture.Factory;
            fixture.ResetDb();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task TestGetAllProjects()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            var request = new GraphQLHttpRequest
            {
                Query = @"query GetAllProjects {
                            projects {
                                title
                                description
                                dateStarted
                                dateEnded
                                bikes {
                                    make
                                }
                            }
                        }",
                OperationName = "Get All Projects",
                Variables = new object { }
            };
            var response = await graphClient.SendQueryAsync<ProjectCollectionType>(request);
            // Validate that there is a single project
            response.Data.Projects.Should().HaveCount(1);
            response.Data.Projects[0].Description.Should().Be("A simple test project!");
            response.Data.Projects[0].Title.Should().Be("Test Project");
            response.Data.Projects[0].DateStarted.Should().Be(DateTime.Parse("2020-09-15"));
            // Check that the project contains a single bike
            response.Data.Projects[0].Bikes.Should().HaveCount(1);
            response.Data.Projects[0].Bikes[0].Make.Should().Be("Schwinn");
        }

        [Fact]
        public async Task TestAddProject()
        {
            var graphClient = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new System.Uri("https://localhost:5001/graphql") }, new SystemTextJsonSerializer(), _client);
            // Query the bikes for ids to put in the project
            var getAllBikesRequest = new GraphQLHttpRequest
            {
                Query = @"query GetAllBikes {
                            bikes {
                                id
                            }
                        }",
                OperationName = "Get All Bikes",
                Variables = new object { }
            };
            var bikeResponse = await graphClient.SendQueryAsync<BikeCollectionType>(getAllBikesRequest);
            bikeResponse.Data.Bikes.Should().HaveCount(2);
            // Now create the project (with the 2 bikes from above)
            var createProjectMutation = new GraphQLHttpRequest
            {
                Query = @"mutation createProject($project: ProjectInputType!) {
                            addProject(project : $project) {
                                title
                                bikes {
                                    id
                                }
                            }
                        }",
                OperationName = "Add Project",
                Variables = new
                {
                    project = new
                    {
                        title = "A new project!",
                        description = "I can create a project!",
                        bikeIds = bikeResponse.Data.Bikes.Select(b => b.Id).ToList()
                    }
                }
            };
            var projectMutationResponse = await graphClient.SendMutationAsync<AddProjectType>(createProjectMutation);
            projectMutationResponse.Data.addProject.Title.Equals("A new project!");
            projectMutationResponse.Data.addProject.Bikes.Select(b => b.Id).ToList().Should().BeEquivalentTo(bikeResponse.Data.Bikes.Select(b => b.Id).ToList());
            // Now verify that the project has been added by querying for it
            var newProjectQuery = new GraphQLHttpRequest
            {
                Query = @"query GetProjects {
                            projects {
                                id
                                title
                            }
                        }",
                OperationName = "Get new project",
                Variables = new object { }
            };
            var queryResponse = await graphClient.SendQueryAsync<ProjectCollectionType>(newProjectQuery);
            queryResponse.Data.Projects.Should().HaveCount(2);
            queryResponse.Data.Projects.Select(p => p.Title).ToList().Should().Contain("A new project!");
        }
    }

    public class ProjectCollectionType
    {
        // This mocks a response that looks like
        // {
        //   "data": {
        //     "projects": [
        //       {
        //         "title": "Garfield Heights Project"
        //         "description": "4 bikes, not bad!",
        //       },
        //      ]
        //    }
        // }
        public IList<Project> Projects { get; set; }
    }

    public class AddProjectType
    {
        // This mocks a reponse that looks like
        // {
        //   "data": {
        //     "addProject": {
        //       "description": "Garfield Heights Project"
        //     }
        //   }
        // }
        public Project addProject { get; set; }
    }
}