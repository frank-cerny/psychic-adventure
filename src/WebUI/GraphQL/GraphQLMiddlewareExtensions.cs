using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.WebUI.GraphQL
{
    // This class is used to make using GraphQL within Startup.cs as easy as a single call
    public static class GraphQLMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphQL(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GraphQLMiddleware>();
        }
        public static IServiceCollection AddGraphQL(this IServiceCollection services, Action<GraphQLOptions> action)
        {
            return services.Configure(action);
        }
    }
}