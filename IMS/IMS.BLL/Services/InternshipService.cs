using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Shared.Enums; 

namespace IMS.BLL.Services;

public class InternshipService(IInternshipRepository repository, IUserRepository userRepository, IMapper mapper)
    : Service<InternshipModel, Internship>(repository, mapper), IInternshipService
{
    private readonly IMapper _mapper = mapper;

    public async Task<InternshipModel> CreateInternshipAsync(InternshipModel model, CancellationToken cancellationToken = default)
    {
        var intern = await userRepository.GetByIdAsync(model.InternId, cancellationToken: cancellationToken) ??
            throw new Exception($"User with ID {model.InternId} was not found");

        var mentor = await userRepository.GetByIdAsync(model.MentorId, cancellationToken: cancellationToken) ??
            throw new Exception($"User with ID {model.MentorId} was not found");

        var hrManager = await userRepository.GetByIdAsync(model.HumanResourcesManagerId, cancellationToken: cancellationToken) ??
            throw new Exception($"User with ID {model.HumanResourcesManagerId} was not found");

        if (intern.Role != Role.Intern) throw new Exception("User assigned to role intern is not an intern");

        if (mentor.Role != Role.Mentor) throw new Exception("User assigned to role mentor is not a mentor");

        if (hrManager.Role != Role.HRManager) throw new Exception("User assigned to role HRManager is not a HRManager");

        var internship = _mapper.Map<Internship>(model);

        var createdInternship = await repository.CreateAsync(internship, cancellationToken);

        var createdInternshipModel = _mapper.Map<InternshipModel>(createdInternship);

        return createdInternshipModel;
    }
}
