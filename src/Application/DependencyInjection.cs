using AutoMapper;
using bike_selling_app.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using GraphQL.Types;
using bike_selling_app.Application.Common.GraphQL;
using bike_selling_app.Application.Common.GraphQL.Types;
using bike_selling_app.Application.Common.GraphQL.Queries;
using bike_selling_app.Application.Common.GraphQL.Mutations;

namespace bike_selling_app.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            // Add GraphQL related services
            services.AddSingleton<ISchema, BikeAppSchema>();
            services.AddTransient<RootQuery>();
            services.AddTransient<RootMutation>();
            services.AddTransient<BikeType>();
            services.AddTransient<BikeInputType>();

            return services;
        }
    }
}
