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

        if (user is null || (user is not null && user.Role != Role.Intern)) throw new Exception($"User with ID {id} is not an intern"); // TODO: Add custom exception

        var internModel = _mapper.Map<InternModel>(user);

        return internModel;  
    }

    public async Task<MentorModel?> GetMentorByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(id, cancellationToken);

        if (user is null || ( user is not null && user.Role != Role.Mentor)) throw new Exception($"User with ID {id} is not a mentor"); // TODO: Add custom exception

        var mentorModel = _mapper.Map<MentorModel>(user);

        return mentorModel;
    }

    public async Task<MentorModel?> AddInternToMentorById(Guid mentorId, Guid internId, CancellationToken cancellationToken)
    {
        var mentor = await repository.GetByIdAsync(mentorId, cancellationToken) ?? throw new Exception($"Mentor with ID {mentorId} was not found"); // TODO: Add custom exception

        var intern = await repository.GetByIdAsync(internId, cancellationToken) ?? throw new Exception($"Intern with ID {internId} was not found"); // TODO: Add custom exception

        if (mentor.Role != Role.Mentor) throw new Exception($"User with ID {mentorId} is not a mentor"); // TODO: Add custom exception

        if (intern.Role != Role.Intern) throw new Exception($"User with ID {internId} is not an intern"); // TODO: Add custom exception

        var mentorModel = _mapper.Map<MentorModel>(mentor);

        mentorModel.Interns ??= [];

        var internModel = _mapper.Map<InternModel>(intern);

        mentorModel.Interns.Add(internModel);

        var updatedMentor = await UpdateAsync(mentorModel, cancellationToken) as MentorModel;

        return updatedMentor;
    }
}
