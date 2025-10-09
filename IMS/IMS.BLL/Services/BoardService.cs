using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 

namespace IMS.BLL.Services;

public class BoardService(IBoardRepository repository, IMapper mapper) : Service<BoardModel, Board>(repository, mapper), IBoardService
{
}
