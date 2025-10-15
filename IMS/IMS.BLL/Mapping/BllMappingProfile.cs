using AutoMapper;
using AutoMapper.Internal;
using IMS.BLL.Models;
using IMS.DAL.Entities;

namespace IMS.BLL.Mapping;

public class BllMappingProfile : Profile 
{
    public BllMappingProfile()
    {
        CreateMap<User, UserModel>().ReverseMap();

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
