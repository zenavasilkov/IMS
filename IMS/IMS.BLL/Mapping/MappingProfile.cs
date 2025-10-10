using AutoMapper;
using AutoMapper.Internal;
using IMS.BLL.Models;
using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.BLL.Mapping;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<EntityBase, ModelBase>().IncludeAllDerived();

        CreateMap<User, InternModel>()
            .ForMember(d => d.Mentor, opt => opt.Ignore())
            .ForMember(d => d.Internship, opt => opt.Ignore())
            .ForMember(d => d.Board, opt => opt.Ignore())
            .ForMember(d => d.HRManager, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.Intern));

        CreateMap<User, MentorModel>()
            .ForMember(d => d.Internships, opt => opt.Ignore())
            .ForMember(d => d.Internships, opt => opt.Ignore())
            .ForMember(d => d.HRManager, opt => opt.Ignore())
            .ForMember(d => d.Boards, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.Mentor));

        CreateMap<User, HumanResouncesManagerModel>()
            .ForMember(d => d.Interships, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.Mentor));

        CreateMap<User, AdminModel>()
            .ReverseMap()
            .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.Admin));

        CreateMap<Internship, InternshipModel>()
            .ForMember(d => d.Intern, opt => opt.MapFrom(s => s.Intern))
            .ForMember(d => d.Mentor, opt => opt.MapFrom(s => s.Mentor))
            .ForMember(d => d.HumanResourcesManager, opt => opt.MapFrom(s => s.HumanResourcesManager))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Board, BoardModel>()
            .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.CreatedBy))
            .ForMember(d => d.CreatedTo, opt => opt.MapFrom(s => s.CreatedTo))
            .ForMember(d => d.Tickets, opt => opt.MapFrom(s => s.Tickets))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Ticket, TicketModel>()
            .ForMember(d => d.Board, opt => opt.MapFrom(s => s.Board))
            .ForMember(d => d.Feedbacks, opt => opt.MapFrom(s => s.Feedbacks))
            .PreserveReferences()
            .ReverseMap();

        CreateMap<Feedback, FeedbackModel>()
            .ForMember(d => d.SentBy, opt => opt.MapFrom(s => s.SentBy))
            .ForMember(d => d.AddressedTo, opt => opt.MapFrom(s => s.AddressedTo))
            .ForMember(d => d.Ticket, opt => opt.MapFrom(s => s.Ticket))
            .PreserveReferences()
            .ReverseMap();

        this.Internal().ForAllMaps((typeMap, mapExpr) =>
        {
            mapExpr.MaxDepth(2);
            mapExpr.PreserveReferences();
        });
    }
}
