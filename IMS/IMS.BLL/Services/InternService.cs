using AutoMapper; 
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 

namespace IMS.BLL.Services;

public class InternService(IUserRepository repository, IMapper mapper) : Service<InternModel, User>(repository, mapper), IInternService
{
}
