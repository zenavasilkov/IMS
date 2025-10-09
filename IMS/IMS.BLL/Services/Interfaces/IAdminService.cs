using IMS.BLL.Models;
using IMS.DAL.Entities; 

namespace IMS.BLL.Services.Interfaces;

public interface IAdminService : IService<AdminModel, User>
{
    Task<UserModel> CreateUserAsync(UserModel userModel, CancellationToken cancellationToken = default);

    Task<bool> DeleteUserAsync(UserModel userModel, CancellationToken cancellationToken = default);

    Task<UserModel> UpdateUserAsync(UserModel userModel, CancellationToken cancellationToken = default); 
}
