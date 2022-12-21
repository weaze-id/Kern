using Microsoft.Extensions.Configuration;

namespace Kern.Database.Extensions;

public static class IConfigurationExtensions
{
    public static string GetDatabaseConfiguration(this IConfiguration configuration)
    {
        var host = configuration["Database:Host"];
        var port = configuration["Database:Port"];
        var database = configuration["Database:Database"];
        var username = configuration["Database:Username"];
        var password = configuration["Database:Password"];
        var minimumPoolSize = configuration["Database:MinimumPoolSize"];
        var maximumPoolSize = configuration["Database:MaximumPoolSize"];
        var includeErrorDetail = configuration["Database:IncludeErrorDetail"];

        return $@"
            Host={host};
            Port={port};
            Database={database};
            Username={username};
            Password={password};
            Minimum Pool Size={minimumPoolSize};
            Maximum Pool Size={maximumPoolSize};
            Include Error Detail={includeErrorDetail};";
    }
}