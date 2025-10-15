using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class BoardService(IBoardRepository repository, IMapper mapper) : Service<BoardModel, Board>(repository, mapper), IBoardService
{
    public async Task<BoardModel?> AddTicketToBoard(Guid boardId, Guid ticketId,
        IService<TicketModel, Ticket> ticketService, CancellationToken cancellationToken = default)
    {
        var board = await GetByIdAsync(boardId, cancellationToken)
            ?? throw new Exception($"Board with ID {boardId} was not found"); // TODO: Add custom exception

        var ticket = await ticketService.GetByIdAsync(ticketId, cancellationToken)
            ?? throw new Exception($"Ticket with ID {ticketId} was not found"); // TODO: Add custom exception

        board.Tickets ??= [];

        board.Tickets.Add(ticket);

        var updatedBoard = await UpdateAsync(boardId, board, cancellationToken);

        return updatedBoard;
    }
}
