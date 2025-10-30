using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Mapping;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using IMS.NotificationsCore.Services;

namespace IMS.BLL.Services;

public class TicketService(
    ITicketRepository repository,
    IBoardRepository boardRepository,
    IMapper mapper,
    IMessageService messageService)
    : Service<TicketModel, Ticket>(repository, mapper), ITicketService
{
    private readonly IMapper _mapper = mapper;

    public override async Task<TicketModel> UpdateAsync(Guid id,
        TicketModel model, CancellationToken cancellationToken = default)
    {
        var existingTicket = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Ticket with ID {id} was not found");

        var oldStatus = existingTicket.Status;

        existingTicket.Title = model.Title;
        existingTicket.Description = model.Description;
        existingTicket.DeadLine = model.DeadLine;
        existingTicket.Status = model.Status;

        var updatedTicket = await repository.UpdateAsync(existingTicket, cancellationToken: cancellationToken);

        var updatedTicketModel = _mapper.Map<TicketModel>(updatedTicket);

        if (updatedTicketModel.Status != model.Status)
        {
            var message = EventMapper.ConvertToTicketStatusChangedEvent(updatedTicketModel, oldStatus);

            await messageService.NotifyTicketStatusChanged(message, cancellationToken);
        }

        return updatedTicketModel;
    }

    public override async Task<TicketModel> CreateAsync(TicketModel ticketModel,
        CancellationToken cancellationToken = default)
    {
        if (await boardRepository.GetByIdAsync(ticketModel.BoardId, cancellationToken: cancellationToken) is null)
            throw new NotFoundException($"Board with ID {ticketModel.BoardId} was not found");

        var createdTicket = await base.CreateAsync(ticketModel, cancellationToken);

        var message = EventMapper.ConvertToTicketCreatedEvent(ticketModel);

        await messageService.NotifyTicketCreated(message, cancellationToken);

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
