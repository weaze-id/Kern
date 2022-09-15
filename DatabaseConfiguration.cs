using Microsoft.Extensions.Configuration;

namespace Kern;

public static class DatabaseConfiguration
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
        var applicationName = configuration["App:Title"] + " " + configuration["App:Version"];

        return $@"
            Host={host};
            Port={port};
            Database={database};
            Username={username};
            Password={password};
            Minimum Pool Size={minimumPoolSize};
            Maximum Pool Size={maximumPoolSize};
            Application Name={applicationName};
            Include Error Detail=true;";
    }
}