using AutoMapper;
using Grpc.Core;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using Shared.Enums;

namespace IMS.Presentation.Grpc;

public sealed class GrpcService(IUserService service, IMapper mapper) : UserGrpcService.UserGrpcServiceBase
{
    public override async Task<CreateUserGrpcResponse> Create(CreateUserGrpcRequest request, ServerCallContext context)
    {
        var dto = new CreateUserDto(
            request.Email,
            request.PhoneNumber,
            request.Firstname,
            request.Lastname,
            string.IsNullOrWhiteSpace(request.Patronymic) ? null : request.Patronymic,
            (Role)request.Role
        );

        var model = mapper.Map<UserModel>(dto);

        var created = await service.CreateAsync(model, context.CancellationToken);

        return new CreateUserGrpcResponse
        {
            Id = created.Id.ToString()
        };
    }
}

