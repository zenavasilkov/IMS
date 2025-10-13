using IMS.BLL.Models;
using IMS.DAL.Entities;

namespace IMS.BLL.Services.Interfaces;

public interface IUserService : IService<UserModel, User>
{
    Task<InternModel?> GetInternByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<MentorModel?> GetMentorByIdAsync(Guid id, CancellationToken cancellationToken);
}
