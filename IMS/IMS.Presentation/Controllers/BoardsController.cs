using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Boards.Base)]
public class BoardsController(IService<BoardModel, Board> boardService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<BoardDTO>> GetAll(CancellationToken cancellationToken)
    {
        var boards = await boardService.GetAllAsync(cancellationToken: cancellationToken); 

        var boardDTOs = mapper.Map<IEnumerable<BoardDTO>>(boards); 

        return boardDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<BoardDTO> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var board = await boardService.GetByIdAsync(id, cancellationToken);

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
    public async Task<BoardDTO> Update([FromRoute] Guid id, [FromBody] UpdateBoardDTO updateBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = mapper.Map<BoardModel>(updateBoardDTO);

        var updatedBoardModel = await boardService.UpdateAsync(id, boardModel, cancellationToken);

        var updatedBoardDTO = mapper.Map<BoardDTO>(updatedBoardModel);

        return updatedBoardDTO;
    }
}
