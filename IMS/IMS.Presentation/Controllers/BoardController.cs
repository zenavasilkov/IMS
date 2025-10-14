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
    public async Task<ActionResult<IEnumerable<BoardDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var boards = await _boardService.GetAllAsync(null, false, cancellationToken);

        var boardDTO = _mapper.Map<IEnumerable<BoardDTO>>(boards); 

        return Ok(boardDTO);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BoardDTO>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var board = await _boardService.GetByIdAsync(id, cancellationToken);

        if (board is null) return NotFound(new { message = $"Board with ID {id} was not found." });

        var boardDTO = _mapper.Map<BoardDTO>(board);

        return Ok(boardDTO);
    }

    [HttpPost]
    public async Task<ActionResult<BoardDTO>> Create([FromBody] CreateBoardDTO createBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = _mapper.Map<BoardModel>(createBoardDTO);

        var createdBoardModel = await _boardService.CreateAsync(boardModel, cancellationToken);

        var boardDTO = _mapper.Map<BoardDTO>(createdBoardModel);

        return CreatedAtAction(nameof(GetById), new { id = boardDTO.Id }, boardDTO);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BoardDTO>> Update(Guid id, [FromBody] UpdateBoardDTO updateBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = _mapper.Map<BoardModel>(updateBoardDTO);

        boardModel.Id = id;

        var updatedBoardModel = await _boardService.UpdateAsync(boardModel, cancellationToken);

        if (updatedBoardModel is null) return NotFound(new { message = $"Board with ID {id} was not found." });

        var updatedBoardDTO = _mapper.Map<BoardDTO>(updatedBoardModel);

        return Ok(updatedBoardDTO);
    }

    [HttpPatch("{boardId:guid}/tickets/{ticketId:guid}")]
    public async Task<IActionResult> AddTicketToBoardById(Guid boardId, Guid ticketId, CancellationToken cancellationToken)
    {
        var boardModel = await _boardService.GetByIdAsync(boardId, cancellationToken);

        if(boardModel is null)
        {
            return NotFound(new { message = $"The board with id {boardId} was not found" });
        }

        var ticketModel = await _ticketService.GetByIdAsync(ticketId, cancellationToken);

        if(ticketModel is null)
        {
            return NotFound(new { message = $"The ticket with id {ticketId} was not found" });
        }

        boardModel.Tickets ??= [];

        boardModel.Tickets.Add(ticketModel);

        var updateBoardModel = await _boardService.UpdateAsync(boardModel, cancellationToken);

        var updateBoardDTO  = _mapper.Map<BoardDTO>(updateBoardModel);

        return Ok(updateBoardDTO);
    }
}
