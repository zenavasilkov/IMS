using IMS.BLL.Exceptions;

namespace IMS.UnitTests.BoardServiceTests;

public class BoardServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IBoardRepository> _boardRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BoardService _boardService;

    public BoardServiceTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _boardRepositoryMock = _fixture.Freeze<Mock<IBoardRepository>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _boardService = new BoardService(_boardRepositoryMock.Object, _mapperMock.Object);
    }

    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldReturnMappedModel_WhenBoardExists(Board existingBoard, 
        BoardModel inputModel, Board updatedBoard, BoardModel expectedModel)
    {
        //Arrange
        existingBoard.Id = Guid.NewGuid();
        inputModel.Title = "Updated Title";
        inputModel.Description = "Updated Description";

        _boardRepositoryMock.Setup(r => r.GetByIdAsync(existingBoard.Id, false, default)).ReturnsAsync(existingBoard);
        _boardRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Board>(), default)).ReturnsAsync(updatedBoard);
        _mapperMock.Setup(m => m.Map<BoardModel>(updatedBoard)).Returns(expectedModel);

        //Act 
        var result = await _boardService.UpdateAsync(existingBoard.Id, inputModel);

        //Assert
        result.Should().BeEquivalentTo(expectedModel);

        _boardRepositoryMock.Verify(r => r.GetByIdAsync(existingBoard.Id, false, default), Times.Once);
        _boardRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Board>(b => b.Title == inputModel.Title 
            && b.Description == inputModel.Description), default), Times.Once);
    }

    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldThrowException_WhenBoardDoesNotExist(Board existingBoard, BoardModel inputModel)
    {
        //Arrange
        existingBoard.Id = Guid.NewGuid();

        _boardRepositoryMock.Setup(r => r.GetByIdAsync(existingBoard.Id, false, default)).ReturnsAsync(existingBoard);

        //Act 
        var act = async() => await _boardService.UpdateAsync(existingBoard.Id, inputModel);

        //Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Board with ID {existingBoard.Id} was not found");

        _boardRepositoryMock.Verify(r => r.GetByIdAsync(existingBoard.Id, false, default), Times.Once);
    }
}
