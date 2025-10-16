using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.BLL.Services.Interfaces;

public interface IUserService : IService<UserModel, User>
{
    Task<UserModel?> GetUserByIdAndRoleAsync(Guid id, Role role, CancellationToken cancellationToken);
}
