using Application.Candidates.Queries.FindByEmail;
using Application.Candidates.Queries.FindById;
using Application.Departments.Queries.GetDepartmentById;
using Application.Employees.Queries.GetEmployeeById;
using Application.Grpc;
using Application.Interviews.Queries.GetInterviewById;
using Domain.Entities;
using IMS.gRPC.Contracts.CreateUser;
using Mapster;
using Pagination;

namespace Application;

internal class Mapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Interview, GetInterviewByIdQueryResponse>()
            .Map(dest => dest.CandidateEmail, src => src.Candidate!.Email)
            .Map(dest => dest.InterviewerEmail, src => src.Interviewer!.Email)
            .Map(dest => dest.DeparnmentName, src => src.Department!.Name)
            .Map(dest => dest.InterviewType, src => src.Type);
        
        config.NewConfig<Employee, GetEmployeeByIdQueryResponse>().Map(dest => dest, src => src.FullName);
        config.NewConfig<Candidate, CreateUserGrpcRequest>().Map(dest => dest, src => src.FullName);
        config.NewConfig<Candidate, CreateUserRequest>().Map(dest => dest, src => src.FullName);

        config.NewConfig<Candidate, FindCandidateByIdQueryResponse>()
            .Map(dest => dest, src => src.FullName)
            .Map(dest => dest.IsApplied, src => src.IsAcceptedToInternship);

        config.NewConfig<Candidate, FindCandidateByEmailQueryResponse>()
            .Map(dest => dest, src => src.FullName)
            .Map(dest => dest.IsApplied, src => src.IsAcceptedToInternship);

        config.NewConfig<PagedList<Interview>, PagedList<GetInterviewByIdQueryResponse>>()
            .MapWith(src => new PagedList<GetInterviewByIdQueryResponse>(
                src.Items.Adapt<List<GetInterviewByIdQueryResponse>>(),
                src.PageNumber,
                src.PageSize,
                src.TotalCount
            ));

        config.NewConfig<PagedList<Employee>, PagedList<GetEmployeeByIdQueryResponse>>()
            .MapWith(src => new PagedList<GetEmployeeByIdQueryResponse>(
                src.Items.Adapt<List<GetEmployeeByIdQueryResponse>>(),
                src.PageNumber,
                src.PageSize,
                src.TotalCount
            ));

        config.NewConfig<PagedList<Candidate>, PagedList<FindCandidateByIdQueryResponse>>()
            .MapWith(src => new PagedList<FindCandidateByIdQueryResponse>(
                src.Items.Adapt<List<FindCandidateByIdQueryResponse>>(),
                src.PageNumber,
                src.PageSize,
                src.TotalCount
            ));

        config.NewConfig<PagedList<Department>, PagedList<GetDepartmentByIdQueryResponse>>()
            .MapWith(src => new PagedList<GetDepartmentByIdQueryResponse>(
                src.Items.Adapt<List<GetDepartmentByIdQueryResponse>>(),
                src.PageNumber,
                src.PageSize,
                src.TotalCount
            ));
    }
}
