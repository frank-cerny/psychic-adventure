using System;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using GraphQL.NewtonsoftJson;
using System.Text.Json;
using Microsoft.Extensions.Options;
using bike_selling_app.Domain.GraphQL;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace bike_selling_app.WebUI.GraphQL
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDocumentWriter _writer;
        private readonly IDocumentExecuter _executor;
        private readonly GraphQLOptions _options;

        public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor, IOptions<GraphQLOptions> options)
        {
            _next = next;
            _writer = writer;
            _executor = executor;
            _options = options.Value;
        }

        // TODO - Potential way to make our handler better: https://github.com/graphql-dotnet/graphql-dotnet/issues/1439
        public async Task InvokeAsync(HttpContext httpContext, ISchema schema)
        {
            if (httpContext.Request.Path.StartsWithSegments(_options.EndPoint) && string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var request = await JsonSerializer
                        .DeserializeAsync<GraphQLRequest>(
                            httpContext.Request.Body,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                    var result = await _executor
                        .ExecuteAsync(doc =>
                        {
                            doc.Schema = schema;
                            doc.Query = request.Query;
                            doc.Inputs = request.Variables.ToInputs();
                            doc.ThrowOnUnhandledException = true;
                        }).ConfigureAwait(false);

                    // Proper error handling if we want to use middleware: https://graphql-dotnet.github.io/docs/getting-started/errors
                    // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-5.0
                    // If we go the controller approach, this becomes easier
                    if (result?.Errors?.Count > 0)
                    {
                        string[] lines =
                        {   
                            result.Errors.ToString()
                        };
                        File.WriteAllLines("output.txt", lines);
                    }

                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = 200;

                    await _writer.WriteAsync(httpContext.Response.Body, result);
                }
                catch (System.Exception ex)
                {
                    string[] lines =
                    {
                        ex.Message, "help!"
                    };
                    File.WriteAllLines("output.txt", lines);
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}