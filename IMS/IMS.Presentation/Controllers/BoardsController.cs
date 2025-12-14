using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.Presentation.Controllers;

[Authorize]
[ApiController]
[Route(ApiRoutes.Boards.Base)]
public class BoardsController(IBoardService boardService, IMapper mapper) : ControllerBase
{
    [Authorize("read:boards")]
    [HttpGet]
    public async Task<PagedList<BoardDto>> GetAll(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] BoardFilteringParameters filter,
        CancellationToken cancellationToken)
    {
        var boards = await boardService.GetAllAsync(paginationParameters, filter, false, cancellationToken); 

        var boardDtos = mapper.Map<PagedList<BoardDto>>(boards); 

        return boardDtos;
    }

    [Authorize("read:boards")]
    [HttpGet(ApiRoutes.Id)]
    public async Task<BoardDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var board = await boardService.GetByIdAsync(id, cancellationToken);

        var boardDto = mapper.Map<BoardDto>(board);

        return boardDto;
    }

    [Authorize("create:boards")]
    [HttpPost]
    public async Task<BoardDto> Create([FromBody] CreateBoardDto createBoardDto, CancellationToken cancellationToken)
    {
        var boardModel = mapper.Map<BoardModel>(createBoardDto);

        var createdBoardModel = await boardService.CreateAsync(boardModel, cancellationToken);

        var boardDto = mapper.Map<BoardDto>(createdBoardModel);

        return boardDto;
    }

    [Authorize("update:boards")]
    [HttpPut(ApiRoutes.Id)]
    public async Task<BoardDto> Update([FromRoute] Guid id, [FromBody] UpdateBoardDto updateBoardDto, CancellationToken cancellationToken)
    {
        var boardModel = mapper.Map<BoardModel>(updateBoardDto);

        var updatedBoardModel = await boardService.UpdateAsync(id, boardModel, cancellationToken);

        var updatedBoardDto = mapper.Map<BoardDto>(updatedBoardModel);

        return updatedBoardDto;
    }
}
