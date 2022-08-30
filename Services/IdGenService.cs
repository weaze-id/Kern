using IdGen;

namespace Kern.Internal.Services;

public static class IdGenService
{
    public static IServiceCollection AddIdGenerator(this IServiceCollection services)
    {
        services.AddSingleton(_ =>
        {
            var structure = new IdStructure(45, 6, 12);
            var epoch = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var options = new IdGeneratorOptions(
                structure,
                new DefaultTimeSource(epoch),
                SequenceOverflowStrategy.SpinWait);

            return new IdGenerator(0, options);
        });

        return services;
    }
}