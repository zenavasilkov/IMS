using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Shared.Enums;

namespace IMS.BLL.Services;

public class UserService(IUserRepository repository, IMapper mapper) : Service<UserModel, User>(repository, mapper), IUserService
{
    private readonly IMapper _mapper = mapper;

    public async Task<UserModel?> GetUserByIdAndRoleAsync(Guid id, Role role, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new Exception($"User with ID {id} was not found"); // TODO: Add custom exception

        if (user is null || (user is not null && user.Role != role))
            throw new Exception($"User with ID {id} is not an {role}"); // TODO: Add custom exception

        var internModel = _mapper.Map<UserModel>(user);

        return internModel;  
    }

    public async Task<List<UserModel>> GetUsersByRoleAsync(Role role, bool trackCanges = false, CancellationToken cancellationToken = default)
    {
        var users = await GetAllAsync(u => u.Role == role, cancellationToken : cancellationToken);

        return users;
    }

    public override async Task<UserModel> UpdateAsync(Guid id, UserModel model, CancellationToken cancellationToken = default)
    {
        var existingUser = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new Exception($"User with ID {id} was not found");

        existingUser.Email = model.Email;
        existingUser.Firstname = model.Firstname;
        existingUser.Lastname = model.Lastname;
        existingUser.Patronymic = model.Patronymic;
        existingUser.PhoneNumber = model.PhoneNumber;
        existingUser.Role = model.Role;

        var updatedUser = await repository.UpdateAsync(existingUser, cancellationToken: cancellationToken);

        var updatedUserModel = _mapper.Map<UserModel>(updatedUser);

        return updatedUserModel;
    }
}
