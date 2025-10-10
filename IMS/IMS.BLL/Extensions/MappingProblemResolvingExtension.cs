using AutoMapper;
using IMS.BLL.Models;

namespace IMS.BLL.Extensions;

public static class MappingProblemResolvingExtension
{
    public static InternshipModel FixCircularReferences(this InternshipModel internship, IMapper mapper)
    {
        var model = mapper.Map<InternshipModel>(internship);

        model.Intern.Mentor = model.Mentor;
        model.Mentor.Inters.Add(model.Intern);
        model.Intern.HRManager = model.HumanResourcesManager;
        model.Mentor.HRManager = model.HumanResourcesManager;
        model.HumanResourcesManager.Interships.Add(model);
        model.Intern.Internship = model;

        return model;
    }
}
