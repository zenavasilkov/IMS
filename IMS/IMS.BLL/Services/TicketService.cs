using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces; 

namespace IMS.BLL.Services;

public class TicketService(ITicketRepository repository, IMapper mapper) : Service<TicketModel, Ticket>(repository, mapper), ITicketService
{
}
