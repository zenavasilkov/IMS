using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class TicketService(ITicketRepository repository, IMapper mapper) 
    : Service<TicketModel, Ticket>(repository, mapper), ITicketService
{
    private readonly IMapper _mapper = mapper;

    public override async Task<TicketModel> UpdateAsync(Guid id, 
        TicketModel model, CancellationToken cancellationToken = default)
    {
        var existingTicket = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Ticket with ID {id} was not found");

        existingTicket.Title = model.Title;
        existingTicket.Description = model.Description;
        existingTicket.DeadLine = model.DeadLine;
        existingTicket.Status = model.Status;

        var updatedTicket = await repository.UpdateAsync(existingTicket, cancellationToken: cancellationToken);

        var updatedTicketModel = _mapper.Map<TicketModel>(updatedTicket);

        return updatedTicketModel;
    }

    public async Task<List<TicketModel>> GetTicketsByBoardIdAsync(Guid boardId, bool trackChanges = false, 
        CancellationToken cancellationToken = default)
    {
        var tickets = await GetAllAsync(t => t.BoardId == boardId, cancellationToken: cancellationToken);

        return tickets;
    }
}
