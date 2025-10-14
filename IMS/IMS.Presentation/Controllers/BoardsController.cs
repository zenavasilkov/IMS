using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Boards.Base)]
public class BoardsController(IBoardService boardService, ITicketService ticketService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<BoardDTO>> GetAll(CancellationToken cancellationToken)
    {
        var boards = await boardService.GetAllAsync(null, false, cancellationToken);

        if (boards.Count == 0) throw new Exception("No boards have been found");

        var boardDTOs = mapper.Map<IEnumerable<BoardDTO>>(boards); 

        return boardDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<BoardDTO> GetById(Guid id, CancellationToken cancellationToken)
    {
        var board = await boardService.GetByIdAsync(id, cancellationToken) ?? throw new Exception($"Board with ID {id} was not found." );

        var boardDTO = mapper.Map<BoardDTO>(board);

        return boardDTO;
    }

    [HttpPost]
    public async Task<BoardDTO> Create([FromBody] CreateBoardDTO createBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = mapper.Map<BoardModel>(createBoardDTO);

        var createdBoardModel = await boardService.CreateAsync(boardModel, cancellationToken);

        var boardDTO = mapper.Map<BoardDTO>(createdBoardModel);

        return boardDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<BoardDTO> Update(Guid id, [FromBody] UpdateBoardDTO updateBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = mapper.Map<BoardModel>(updateBoardDTO);

        boardModel.Id = id;

        var updatedBoardModel = await boardService.UpdateAsync(boardModel, cancellationToken) ?? throw new Exception($"Board with ID {id} was not found.");

        var updatedBoardDTO = mapper.Map<BoardDTO>(updatedBoardModel);

        return updatedBoardDTO;
    }

    [HttpPatch(ApiRoutes.Boards.AddTicket)]
    public async Task<BoardDTO> AddTicketById(Guid boardId, Guid ticketId, CancellationToken cancellationToken)
    {
        var updatedBoardModel = await boardService.AddTicketById(boardId, ticketId, ticketService, cancellationToken) ?? 
            throw new Exception($"Board with ID {boardId} or Ticket with ID {ticketId} was not found.");

        var updatedBoardDTO  = mapper.Map<BoardDTO>(updatedBoardModel);

        return updatedBoardDTO;
    }
}
