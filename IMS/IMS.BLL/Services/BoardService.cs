using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Models;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class BoardService(IBoardRepository repository, IMapper mapper) : Service<BoardModel, Board>(repository, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<BoardModel> UpdateAsync(Guid id, BoardModel model, CancellationToken cancellationToken = default)
    {
        var existingBoard = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Board with ID {id} was not found");

        existingBoard.Title = model.Title;
        existingBoard.Description = model.Description;

        var updatedBoard = await repository.UpdateAsync(existingBoard, cancellationToken: cancellationToken);

        var updatedBoardModel = _mapper.Map<BoardModel>(updatedBoard);

        return updatedBoardModel;
    }
}
