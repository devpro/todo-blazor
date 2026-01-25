using MongoDB.Driver;

namespace Devpro.Common.MongoDb;

/// <summary>
/// Factory to create MongoDB clients so that no "new" is made in application code.
/// </summary>
public interface IMongoClientFactory
{
    /// <summary>
    /// Creates MongoDB client from the connection string.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    MongoClient CreateClient(string connectionString);
}
