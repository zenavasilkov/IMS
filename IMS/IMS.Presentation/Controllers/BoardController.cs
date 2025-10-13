using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardController(IBoardService boardService, ITicketService ticketService,  IMapper mapper) : ControllerBase
{
    private readonly IBoardService _boardService = boardService;
    private readonly ITicketService _ticketService = ticketService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IEnumerable<BoardDTO>> GetAll(CancellationToken cancellationToken)
    {
        var boards = await _boardService.GetAllAsync(null, false, cancellationToken);
        var boardDTO = _mapper.Map<IEnumerable<BoardDTO>>(boards);
        return boardDTO;
    }

    [HttpGet("{id:guid}")]
    public async Task<BoardDTO> GetById(Guid id, CancellationToken cancellationToken)
    {
        var board = await _boardService.GetByIdAsync(id, cancellationToken);
        var boardDTO = _mapper.Map<BoardDTO>(board);
        return boardDTO;
    }

    [HttpPost]
    public async Task<BoardDTO> Create([FromBody] CreateBoardDTO createBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = _mapper.Map<BoardModel>(createBoardDTO);
        var createdBoardModel = await _boardService.CreateAsync(boardModel, cancellationToken);
        var boardDTO = _mapper.Map<BoardDTO>(createdBoardModel);
        return boardDTO;
    }

    [HttpPut]
    public async Task<BoardDTO> Update(Guid id, [FromBody] UpdateBoardDTO updateBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = _mapper.Map<BoardModel>(updateBoardDTO);
        boardModel.Id = id;
        var updatedBoardModel = await _boardService.UpdateAsync(boardModel, cancellationToken);
        var updatedBoardDTO = _mapper.Map<BoardDTO>(updatedBoardModel);
        return updatedBoardDTO;
    }

    [HttpPatch]
    public async Task<IActionResult> AddTicketToBoardById(Guid boardId, Guid ticketId, CancellationToken cancellationToken)
    {
        var boardModel = await _boardService.GetByIdAsync(boardId, cancellationToken);

        if(boardModel is null)
        {
            return NotFound($"Board with id {boardId} not found");
        }

        var ticketModel = await _ticketService.GetByIdAsync(boardId, cancellationToken);

        if(ticketModel is null)
        {
            return NotFound($"Ticket with id {ticketId} not found");
        }

        boardModel.Tickets.Add(ticketModel);

        await _boardService.UpdateAsync(boardModel, cancellationToken);

        return Ok("Ticket have been added to board successefully");
    }
}
