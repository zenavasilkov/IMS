namespace IMS.UnitTests.FeedbackServiceTests;

public class FeedbackServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock;
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FeedbackService _feedbackService;

    public FeedbackServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());

        // Prevent circular reference exceptions
        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _feedbackRepositoryMock = _fixture.Freeze<Mock<IFeedbackRepository>>();
        _ticketRepositoryMock = _fixture.Freeze<Mock<ITicketRepository>>();
        _userRepositoryMock = _fixture.Freeze<Mock<IUserRepository>>();

        _mapperMock = _fixture.Freeze<Mock<IMapper>>();

        _feedbackService = new FeedbackService(_feedbackRepositoryMock.Object, 
            _ticketRepositoryMock.Object, _userRepositoryMock.Object,  _mapperMock.Object);
    }

    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldReturnMappedModel_WhenFeedbackExists(
        Guid id,
        FeedbackModel inputModel,
        Feedback existingFeedback,
        Feedback updatedFeedback,
        FeedbackModel expectedFeedbackModel,
        CancellationToken cancellationToken)
    {
        // Arrange
        _feedbackRepositoryMock.Setup(r => r.GetByIdAsync(id, false, cancellationToken)).ReturnsAsync(existingFeedback);
        _feedbackRepositoryMock.Setup(r => r.UpdateAsync(existingFeedback, cancellationToken)).ReturnsAsync(updatedFeedback);
        _mapperMock.Setup(m => m.Map<FeedbackModel>(updatedFeedback)).Returns(expectedFeedbackModel);

        // Act
        var result = await _feedbackService.UpdateAsync(id, inputModel, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedFeedbackModel);

        _feedbackRepositoryMock.Verify(r => r.GetByIdAsync(id, false, cancellationToken), Times.Once);
        _feedbackRepositoryMock.Verify(r => r.UpdateAsync(existingFeedback, cancellationToken), Times.Once);
        _mapperMock.Verify(m => m.Map<FeedbackModel>(updatedFeedback), Times.Once);
    }

    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenFeedbackDoesNotExist(Guid id,
        FeedbackModel inputModel, CancellationToken cancellationToken)
    {
        // Arrange
        _feedbackRepositoryMock.Setup(r => r.GetByIdAsync(id, false, cancellationToken)).ReturnsAsync((Feedback?)null);

        // Act
        var act = async () => await _feedbackService.UpdateAsync(id, inputModel, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Feedback with ID {id} was not found");

        _feedbackRepositoryMock.Verify(r => r.GetByIdAsync(id, false, cancellationToken), Times.Once);
        _feedbackRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Feedback>(), cancellationToken), Times.Never);
        _mapperMock.Verify(m => m.Map<FeedbackModel>(It.IsAny<Feedback>()), Times.Never);
    }

    [Theory, CustomAutoData]
    public async Task GetFeedbacksByTicketIdAsync_ShouldReturnMappedList_WhenFeedbacksExist(
        Guid ticketId,
        List<Feedback> feedbackEntities,
        List<FeedbackModel> feedbackModels,
        CancellationToken cancellationToken)
    {
        // Arrange
        _feedbackRepositoryMock.Setup(r => r.GetAllAsync(f => f.TicketId == ticketId, false, cancellationToken))
            .ReturnsAsync(feedbackEntities);

        _mapperMock.Setup(m => m.Map<List<FeedbackModel>>(feedbackEntities)).Returns(feedbackModels);

        // Act
        var result = await _feedbackService.GetFeedbacksByTicketIdAsync(ticketId, false, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(feedbackModels);

        _feedbackRepositoryMock.Verify(r => r.GetAllAsync(f => f.TicketId == ticketId, false, cancellationToken), Times.Once);
        _mapperMock.Verify(m => m.Map<List<FeedbackModel>>(feedbackEntities), Times.Once);
    }
}
