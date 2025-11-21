using Application.Grpc;
using IMS.gRPC.Contracts.CreateUser;
using IMS.NotificationsCore.Extensions;
using MagicOnion.Client;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Net.Client;
using static Application.AssemblyReference;

namespace Application;

public static class DependencyInjection
{
    private const string GrpcAddressKey = "gRPC:Address";

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly))
            .AddNotifications(configuration)
            .AddMapping()
            .AddUserGrpc(configuration)
            .AddUserGrpcWithMagicOnion(configuration);

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
        var grpcAddress = configuration[GrpcAddressKey]
            ?? throw new InvalidOperationException($"Couldn't find gRPC address in {GrpcAddressKey}");

        services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(o => o.Address = new Uri(grpcAddress));

        return services;
    }

    private static IServiceCollection AddUserGrpcWithMagicOnion(this IServiceCollection services, IConfiguration configuration)
    {
        var grpcAddress = configuration[GrpcAddressKey]
            ?? throw new InvalidOperationException($"Couldn't find gRPC address in {GrpcAddressKey}");

        services.AddSingleton(_ =>
        {
            var channel = GrpcChannel.ForAddress(grpcAddress);
            return MagicOnionClient.Create<IUserService>(channel);
        });

        return services;
    }
}
