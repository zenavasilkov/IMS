using IMS.BLL.Models;
using IMS.gRPC.Contracts.CreateUser;
using MagicOnion;
using MagicOnion.Server;
using Shared.Enums;
using IUserService = IMS.gRPC.Contracts.CreateUser.IUserService;
using IUserBllService = IMS.BLL.Services.Interfaces.IUserService;

namespace IMS.BLL.Grpc.MagicOnionImplementation;

public class UserService(IUserBllService service) : ServiceBase<IUserService>, IUserService
{
    public async UnaryResult<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        var model = new UserModel()
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Firstname = request.FirstName,
            Lastname = request.LastName,
            Patronymic = request.Patronymic,
            Role = Role.Intern
        };

        var createdModel = await service.CreateAsync(model);

        return new CreateUserResponse{Id = createdModel.Id};
    }
}
