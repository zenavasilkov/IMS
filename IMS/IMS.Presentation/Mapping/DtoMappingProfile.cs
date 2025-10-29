using AutoMapper;
using IMS.BLL.Models; 
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using Shared.Pagination;

namespace IMS.Presentation.Mapping;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<CreateUserDto, UserModel>()
            .ForMember(d => d.Internships, ops => ops.Ignore())
            .PreserveReferences();

        CreateMap<CreateFeedbackDto, FeedbackModel>();

        CreateMap<CreateBoardDto, BoardModel>();

        CreateMap<CreateInternshipDto, InternshipModel>()
            .ForMember(d => d.Intern, ops => ops.Ignore())
            .ForMember(d => d.Mentor, ops => ops.Ignore())
            .ForMember(d => d.HumanResourcesManager, ops => ops.Ignore());

        CreateMap<CreateTicketDto, TicketModel>();

        CreateMap<UpdateUserDto, UserModel>()
            .ForMember(d => d.Internships, ops => ops.Ignore());

        CreateMap<UpdateFeedbackDto, FeedbackModel>();

        CreateMap<UpdateBoardDto, BoardModel>();

        CreateMap<UpdateInternshipDto, InternshipModel>()
            .ForMember(d => d.InternId, ops => ops.Ignore())
            .ForMember(d => d.Intern, ops => ops.Ignore())
            .ForMember(d => d.Mentor, ops => ops.Ignore())
            .ForMember(d => d.HumanResourcesManager, ops => ops.Ignore());

        CreateMap<UpdateTicketDto, TicketModel>();

        CreateMap<BoardModel, BoardDto>();

        CreateMap<FeedbackModel, FeedbackDto>();

        CreateMap<InternshipModel, InternshipDto>();

        CreateMap<TicketModel, TicketDto>();

        CreateMap<UserModel, UserDto>();

        CreateMap<PagedList<UserModel>, PagedList<UserDto>>();
    }
}
