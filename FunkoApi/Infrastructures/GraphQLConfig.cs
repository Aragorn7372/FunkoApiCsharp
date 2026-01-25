using FunkoApi.Graphql.Mutations;
using FunkoApi.Graphql.Queries;
using FunkoApi.Graphql.Subscriptions;
using FunkoApi.Graphql.Types;
using HotChocolate.Execution.Configuration;
using Serilog;

namespace FunkoApi.Infrastructures;

/// <summary>
/// Extensiones de configuración de GraphQL con HotChocolate.
/// </summary>
public static class GraphQLConfig
{
    /// <summary>
    /// Configura GraphQL con queries de productos y categorías.
    /// </summary>
    public static IRequestExecutorBuilder AddGraphQL(this IServiceCollection services, IWebHostEnvironment environment)
    {
        Log.Information("🔍 Configurando GraphQL con HotChocolate...");
        return services
            .AddGraphQLServer()
            .AddQueryType<FunkoQuery>()
            .AddMutationType<FunkoMutation>()
            .AddSubscriptionType<FunkoSubscription>()
            .AddInMemorySubscriptions()
            .AddType<FunkoType>()
            .AddType<CategoryType>()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = environment.IsDevelopment());
    }
}