using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class InternshipService(IInternshipRepository repository, IMapper mapper) : 
    Service<InternshipModel, Internship>(repository, mapper), IInternshipService
{ 
    private readonly IMapper _mapper = mapper; 

    public InternshipModel ToModel(Internship internship)
    {
        var model = _mapper.Map<InternshipModel>(internship);

        model.Intern.Mentor = model.Mentor;
        model.Mentor.Inters.Add(model.Intern);
        model.Intern.HRManager = model.HumanResourcesManager;
        model.Mentor.HRManager = model.HumanResourcesManager;
        model.HumanResourcesManager.Interships.Add(model);
        model.Intern.Internship = model;

        return model;
    } 
}
