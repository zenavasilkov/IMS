using AutoMapper;
using IMS.BLL.Models; 
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;

namespace IMS.Presentation.Mapping;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<CreateUserDTO, UserModel>()
            .ForMember(d => d.Internships, ops => ops.Ignore())
            .PreserveReferences();

        CreateMap<CreateFeedbackDTO, FeedbackModel>();

        CreateMap<CreateBoardDTO, BoardModel>();

        CreateMap<CreateInternshipDTO, InternshipModel>()
            .ForMember(d => d.Intern, ops => ops.Ignore())
            .ForMember(d => d.Mentor, ops => ops.Ignore())
            .ForMember(d => d.HumanResourcesManager, ops => ops.Ignore());

        CreateMap<CreateTicketDTO, TicketModel>();

        CreateMap<UpdateUserDTO, UserModel>()
            .ForMember(d => d.Internships, ops => ops.Ignore());

        CreateMap<UpdateFeedbackDTO, FeedbackModel>();
        CreateMap<UpdateBoardDTO, BoardModel>();
        CreateMap<UpdateInternshipDTO, InternshipModel>();
        CreateMap<UpdateTicketDTO, TicketModel>();

        CreateMap<BoardModel, BoardDTO>();
        CreateMap<FeedbackModel, FeedbackDTO>();
        CreateMap<InternshipModel, InternshipDTO>();
        CreateMap<TicketModel, TicketDTO>();
        CreateMap<UserModel, UserDTO>();
    }
}
