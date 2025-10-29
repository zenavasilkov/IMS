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
    public async Task<IEnumerable<BoardDto>> GetAll(CancellationToken cancellationToken)
    {
        var boards = await boardService.GetAllAsync(cancellationToken: cancellationToken); 

        var boardDTOs = mapper.Map<IEnumerable<BoardDto>>(boards); 

        return boardDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<BoardDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var board = await boardService.GetByIdAsync(id, cancellationToken);

        var boardDTO = mapper.Map<BoardDto>(board);

        return boardDTO;
    }

    [HttpPost]
    public async Task<BoardDto> Create([FromBody] CreateBoardDto createBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = mapper.Map<BoardModel>(createBoardDTO);

        var createdBoardModel = await boardService.CreateAsync(boardModel, cancellationToken);

        var boardDTO = mapper.Map<BoardDto>(createdBoardModel);

        return boardDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<BoardDto> Update([FromRoute] Guid id, [FromBody] UpdateBoardDto updateBoardDTO, CancellationToken cancellationToken)
    {
        var boardModel = mapper.Map<BoardModel>(updateBoardDTO);

        var updatedBoardModel = await boardService.UpdateAsync(id, boardModel, cancellationToken);

        var updatedBoardDTO = mapper.Map<BoardDto>(updatedBoardModel);

        return updatedBoardDTO;
    }
}
