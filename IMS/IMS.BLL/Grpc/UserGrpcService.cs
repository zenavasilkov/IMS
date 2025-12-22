using Grpc.Core;
using IMS.BLL.Grpc;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using Shared.Enums;

namespace IMS.Presentation.Grpc;

public class UserGRpcService(IUserService service) : UserGrpcService.UserGrpcServiceBase
{
    public override async Task<CreateUserGrpcResponse> Create(CreateUserGrpcRequest request, ServerCallContext context)
    { 
        var model = new UserModel() {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Firstname = request.FirstName,
            Lastname = request.LastName,
            Patronymic = request.Patronymic,
            Role = Role.Intern
        };

        var created = await service.CreateAsync(model, context.CancellationToken);

        return new CreateUserGrpcResponse
        {
            Id = created.Id.ToString()
        };
    }
}

