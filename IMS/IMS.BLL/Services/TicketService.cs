using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Mapping;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using IMS.NotificationsCore.Services;
using Shared.Filters;
using Shared.Pagination;

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

        if (updatedTicketModel.Status != oldStatus)
        {
            var message = EventMapper.ConvertToTicketStatusChangedEvent(updatedTicket, oldStatus);

            await messageService.NotifyTicketStatusChanged(message, cancellationToken);
        }

        return updatedTicketModel;
    }

    public async Task<PagedList<TicketModel>> GetAllAsync(
        PaginationParameters paginationParameters,
        TicketFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        var tickets = await repository.GetAllAsync(paginationParameters, filter, trackChanges, cancellationToken);
        
        var ticketModels = _mapper.Map<PagedList<TicketModel>>(tickets);
        
        return ticketModels;
    }

    public override async Task<TicketModel> CreateAsync(TicketModel ticketModel,
        CancellationToken cancellationToken = default)
    {
        if (await boardRepository.GetByIdAsync(ticketModel.BoardId, cancellationToken: cancellationToken) is null)
            throw new NotFoundException($"Board with ID {ticketModel.BoardId} was not found");

        var ticket = _mapper.Map<Ticket>(ticketModel);

        var createdTicket = await repository.CreateAsync(ticket, cancellationToken);

        var createdTicketModel = _mapper.Map<TicketModel>(createdTicket);

        var message = EventMapper.ConvertToTicketCreatedEvent(createdTicketModel);

        await messageService.NotifyTicketCreated(message, cancellationToken);

        return createdTicketModel;
    }
}
