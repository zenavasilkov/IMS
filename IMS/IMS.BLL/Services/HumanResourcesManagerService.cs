using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 

namespace IMS.BLL.Services;

public class HumanResourcesManagerService(IUserRepository repository, IMapper mapper) 
    : Service<HumanResouncesManagerModel, User>(repository, mapper), IHumanResourcesManagerService 
{
}
