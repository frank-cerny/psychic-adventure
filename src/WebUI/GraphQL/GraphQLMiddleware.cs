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
using Microsoft.Extensions.Logging;
using bike_selling_app.Application.Common.Exceptions;
using GraphQL.Execution;

namespace bike_selling_app.WebUI.GraphQL
{
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDocumentWriter _writer;
        private readonly IDocumentExecuter _executor;
        private readonly GraphQLOptions _options;
        private readonly ILogger<GraphQLMiddleware> _logger;

        public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor, IOptions<GraphQLOptions> options, ILogger<GraphQLMiddleware> logger)
        {
            _next = next;
            _writer = writer;
            _executor = executor;
            _options = options.Value;
            _logger = logger;
        }

        // TODO - Potential way to make our handler better: https://github.com/graphql-dotnet/graphql-dotnet/issues/1439
        public async Task InvokeAsync(HttpContext httpContext, ISchema schema)
        {
            if (httpContext.Request.Path.StartsWithSegments(_options.EndPoint) && string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
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
                        // This overrides the default delegate behavior of ?
                        doc.UnhandledExceptionDelegate = context => 
                        {
                            try 
                            {
                                // Log the exceptions (all Serilog setup happens in Startup/Program.cs)
                                _logger.LogError($"GraphQL Error. Timestamp: {DateTime.UtcNow}, Message: {context.Exception.Message}, Details: {context.Exception.ToString()}");
                                if (context.Exception is ValidationException)
                                {
                                    // We can add more detail to the exception that will eventually be passed to the user 
                                    context.ErrorMessage = "Input Validation Error. See details field for more information";
                                    context.Exception.Data["details"] = context.Exception.Message;
                                    // This field can be used by clients to understand what to do with the details section on the error
                                    context.Exception.Data["errorType"] = "validation";
                                }
                                else 
                                {
                                    context.ErrorMessage = "Other Error. See details field for more information";
                                    context.Exception.Data["details"] = context.Exception.Message;
                                    context.Exception.Data["errorType"] = "unknown";
                                }
                            }
                            catch {}
                        };
                    }).ConfigureAwait(false);

                httpContext.Response.ContentType = "application/json";
                // If there are input errors, add additional fields to match that of custom errors (otherwise the client will never know that something is wrong)
                if (result?.Errors != null && result?.Errors.Count != 0)
                {
                    // Based on codes from: https://graphql-dotnet.github.io/docs/getting-started/errors
                    foreach (ExecutionError err in result.Errors)
                    {
                        // If the error is an input error (i.e. not a custom exception, add custom fields)
                        if (err is DocumentError)
                        {
                            err.Data["details"] = $"See documentation for error specifics.";
                            err.Data["errorType"] = "input";
                            // Log the error as well (all schema/processing errors are logged in the execution delegate above)
                            _logger.LogError($"GraphQL Error. Timestamp: {DateTime.UtcNow}, Message: {err.Message}, Details: {err.InnerException}");
                        }
                    }
                }

                httpContext.Response.StatusCode = 200;

                await _writer.WriteAsync(httpContext.Response.Body, result);
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}