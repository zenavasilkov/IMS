namespace IMS.UnitTests.TicketServiceTests;

public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TicketService _ticketService;

    public TicketServiceTests()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _ticketRepositoryMock = fixture.Freeze<Mock<ITicketRepository>>();
        var boardRepositoryMock = fixture.Freeze<Mock<IBoardRepository>>();
        _mapperMock = fixture.Freeze<Mock<IMapper>>();
        var messageServiceMock = fixture.Freeze<Mock<IMessageService>>();

        _ticketService = new TicketService(
            _ticketRepositoryMock.Object, 
            boardRepositoryMock.Object, 
            _mapperMock.Object,
            messageServiceMock.Object);
    }

    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldReturnMappedModel_WhenTicketExists(Guid id, TicketModel model, Ticket updatedTicket, 
        TicketModel updatedTicketModel, Ticket existingTicket, CancellationToken cancellationToken)
    {
        //Arrange
        _ticketRepositoryMock.Setup(r => r.GetByIdAsync(id, false, cancellationToken)).ReturnsAsync(existingTicket);
        _ticketRepositoryMock.Setup(r => r.UpdateAsync(existingTicket, cancellationToken)).ReturnsAsync(updatedTicket);
        _mapperMock.Setup(m => m.Map<TicketModel>(updatedTicket)).Returns(updatedTicketModel);

        //Act
        var result = await _ticketService.UpdateAsync(id, model, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(updatedTicketModel);

        _ticketRepositoryMock.Verify(r => r.GetByIdAsync(id, false, cancellationToken), Times.Once);
        _ticketRepositoryMock.Verify(r => r.UpdateAsync(existingTicket, cancellationToken), Times.Once);
        _mapperMock.Verify(m => m.Map<TicketModel>(updatedTicket), Times.Once());
    }

    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldThrowException_WhenTicketDoesNotExist(Guid id, 
        TicketModel model, CancellationToken cancellationToken)
    {
        //Arrange
        _ticketRepositoryMock.Setup(r => r.GetByIdAsync(id, false, cancellationToken)).ReturnsAsync((Ticket?)null);

        //Act
        var act = async() => await _ticketService.UpdateAsync(id, model, cancellationToken);

        //Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Ticket with ID {id} was not found");

        _ticketRepositoryMock.Verify(r => r.GetByIdAsync(id, false, cancellationToken), Times.Once());
    }
}
