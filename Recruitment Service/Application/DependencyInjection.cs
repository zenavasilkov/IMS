using Application.Grpc;
using IMS.NotificationsCore.Extensions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Application.AssemblyReference;

namespace Application;

public static class DependencyInjection
{
    const string grpcAddressKey = "gRPC:Address";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly))
            .AddNotifications(configuration)
            .AddMapping()
            .AddUserGrpc(configuration);

        return services;
    }

    private static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }

    private static IServiceCollection AddUserGrpc(this IServiceCollection services, IConfiguration configuration)
    {
        var grpcAddress = configuration[grpcAddressKey]
            ?? throw new InvalidOperationException($"Couldn't find gRPC address in {grpcAddressKey}");

        services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(o => o.Address = new Uri(grpcAddress));

        return services;
    }
}
