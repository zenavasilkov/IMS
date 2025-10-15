using AutoMapper;
using IMS.BLL.Models; 
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using Shared.Enums;

namespace IMS.Presentation.Mapping;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        //CreateMap<CreateUserDTO, AdminModel>();
        //CreateMap<CreateUserDTO, InternModel>();
        //CreateMap<CreateUserDTO, MentorModel>();
        //CreateMap<CreateUserDTO, HumanResouncesManagerModel>();
        CreateMap<CreateUserDTO, UserModel>()
            .ForMember(d => d.Internships, ops => ops.Ignore())
            .PreserveReferences();

        CreateMap<CreateFeedbackDTO, FeedbackModel>();
        CreateMap<CreateBoardDTO, BoardModel>();
        CreateMap<CreateInternshipDTO, InternshipModel>();
        CreateMap<CreateTicketDTO, TicketModel>();

        //CreateMap<UpdateUserDTO, AdminModel>();
        //CreateMap<UpdateUserDTO, InternModel>();
        //CreateMap<UpdateUserDTO, MentorModel>();
        //CreateMap<UpdateUserDTO, HumanResouncesManagerModel>();
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

        //CreateMap<AdminModel, UserDTO>()
        //    .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.Admin));

        //CreateMap<InternModel, UserDTO>()
        //    .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.Intern));

        //CreateMap<MentorModel, UserDTO>()
        //    .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.Mentor));

        //CreateMap<HumanResouncesManagerModel, UserDTO>()
        //    .ForMember(d => d.Role, opt => opt.MapFrom(_ => Role.HRManager)); 
    }
}
