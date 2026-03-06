using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Devpro.TodoList.BlazorApp.DependencyInjection;

internal static class InfrastructureServiceCollectionExtensions
{
    internal static void AddMongoDbInfrastructure(this IServiceCollection services, DatabaseSettings databaseSettings)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfNullConvention(true)
            };
            ConventionRegistry.Register("Conventions", pack, t => true);
            return new MongoClient(databaseSettings.ConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp
            => sp.GetRequiredService<IMongoClient>().GetDatabase(databaseSettings.DatabaseName));

        services.AddScoped<TodoItemRepository>();
    }
}
