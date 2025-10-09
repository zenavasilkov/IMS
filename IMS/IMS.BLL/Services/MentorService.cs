using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 

namespace IMS.BLL.Services;

public class MentorService(IUserRepository repository, IMapper mapper) : Service<MentorModel, User>(repository, mapper), IMentorService
{
}
