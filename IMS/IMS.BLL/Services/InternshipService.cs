using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Shared.Enums; 

namespace IMS.BLL.Services;

public class InternshipService(IInternshipRepository repository, IMapper mapper)
    : Service<InternshipModel, Internship>(repository, mapper), IInternshipService
{
    private readonly IMapper _mapper = mapper;

    public async Task<InternshipModel> CreateInternshipAsync(InternshipModel model, CancellationToken cancellationToken = default)
    {
        //TODO: Intemenent custom exception
        if (model.Intern.Role != Role.Intern) throw new Exception("User assigned to role intern is not an intern");
        if (model.Mentor.Role != Role.Mentor) throw new Exception("User assigned to role mentor is not a mentor");
        if (model.HumanResourcesManager.Role != Role.HRManager) throw new Exception("User assigned to role HRManager is not a HRManager");

        var internship = _mapper.Map<Internship>(model);

        var createdInternship = await repository.CreateAsync(internship, cancellationToken);

        var createdInternshipModel = _mapper.Map<InternshipModel>(createdInternship);

        return createdInternshipModel;
    }
}
