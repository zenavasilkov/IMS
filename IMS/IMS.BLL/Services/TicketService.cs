using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Logging;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace IMS.BLL.Services;

public class TicketService(ITicketRepository repository, IBoardRepository boardRepository, IMapper mapper, 
    ILogger<TicketService> logger) : Service<TicketModel, Ticket>(repository, mapper, logger), ITicketService
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

        logger.LogInformation(LoggingConstants.RESOURCE_UPDATED, nameof(Ticket), id);

        var updatedTicketModel = _mapper.Map<TicketModel>(updatedTicket);

        return updatedTicketModel;
    }

    public override async Task<TicketModel> CreateAsync(TicketModel ticketModel, 
        CancellationToken cancellationToken = default)
    {
         if(await boardRepository.GetByIdAsync(ticketModel.BoardId, cancellationToken: cancellationToken) is null )  
            throw new NotFoundException($"Board with ID {ticketModel.BoardId} was not found");

        var createdTicket = await base.CreateAsync(ticketModel, cancellationToken);

        logger.LogInformation(LoggingConstants.RESOURCE_CREATED, nameof(Ticket), createdTicket.Id);

        return createdTicket;
    }

    public async Task<List<TicketModel>> GetTicketsByBoardIdAsync(Guid boardId, bool trackChanges = false, 
        CancellationToken cancellationToken = default)
    {
        if (await boardRepository.GetByIdAsync(boardId, cancellationToken: cancellationToken) is null)
            throw new NotFoundException($"Board with ID {boardId} was not found");

        var tickets = await repository.GetAllAsync(t => t.BoardId == boardId, false, cancellationToken);

        var ticketModels = _mapper.Map<List<TicketModel>>(tickets);

        return ticketModels;
    }
}
