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

    public async Task<InternModel?> GetInternByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);

        if (user is not null && user.Role == Role.Intern)
        {
            var internModel = _mapper.Map<InternModel>(user);
            return internModel;
        }

        return null;
    }

    public async Task<MentorModel?> GetMentorByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);

        if (user is not null && user.Role == Role.Mentor)
        {
            var mentorModel = _mapper.Map<MentorModel>(user);
            return mentorModel;
        }

        return null;
    }
}
