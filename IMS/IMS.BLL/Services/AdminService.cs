using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 

namespace IMS.BLL.Services;

public class AdminService(IUserRepository repository, IMapper mapper) : Service<AdminModel, User>(repository, mapper), IAdminService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<User> _repository = repository;

    public async Task<UserModel> CreateUserAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(userModel);
        var createdUser = await _repository.CreateAsync(user, cancellationToken);
        var createdUserModel = _mapper.Map<UserModel>(createdUser);

        return createdUserModel;
    }

    public async Task<bool> DeleteUserAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(userModel);
        var isDeleted = await _repository.DeleteAsync(user, cancellationToken); 

        return isDeleted;
    }

    public async Task<UserModel> UpdateUserAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(userModel);
        var updatedUser = await _repository.UpdateAsync(user, cancellationToken);
        var updatedUserModel = _mapper.Map<UserModel>(updatedUser);

        return updatedUserModel;
    }
}
