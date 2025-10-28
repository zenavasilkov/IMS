namespace IMS.UnitTests.InternshipServiceTests;

public class InternshipServiceTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IInternshipRepository> _internshipRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<InternshipService>> _loggerMock;
    private readonly InternshipService _internshipService;

    public InternshipServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _internshipRepositoryMock = _fixture.Freeze<Mock<IInternshipRepository>>();
        _userRepositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
        _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        _loggerMock = _fixture.Freeze<Mock<ILogger<InternshipService>>>();

        _internshipService = new InternshipService(
            _internshipRepositoryMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }
     
    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldReturnMappedModel_WhenInternshipExists(
        Guid id,
        InternshipModel inputModel,
        Internship existingInternship,
        Internship updatedInternship,
        InternshipModel expectedModel,
        CancellationToken cancellationToken)
    {
        // Arrange
        _internshipRepositoryMock.Setup(r => r.GetByIdAsync(id, false, cancellationToken)).ReturnsAsync(existingInternship);
        _internshipRepositoryMock.Setup(r => r.UpdateAsync(existingInternship, cancellationToken)).ReturnsAsync(updatedInternship);
        _mapperMock .Setup(m => m.Map<InternshipModel>(updatedInternship)).Returns(expectedModel);

        // Act
        var result = await _internshipService.UpdateAsync(id, inputModel, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedModel);

        _internshipRepositoryMock.Verify(r => r.GetByIdAsync(id, false, cancellationToken), Times.Once);
        _internshipRepositoryMock.Verify(r => r.UpdateAsync(existingInternship, cancellationToken), Times.Once);
        _mapperMock.Verify(m => m.Map<InternshipModel>(updatedInternship), Times.Once);
    }

    [Theory, CustomAutoData]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenInternshipDoesNotExist(Guid id,
        InternshipModel inputModel, CancellationToken cancellationToken)
    {
        // Arrange
        _internshipRepositoryMock.Setup(r => r.GetByIdAsync(id, false, cancellationToken)).ReturnsAsync((Internship?)null);

        // Act
        var act = async () => await _internshipService.UpdateAsync(id, inputModel, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Internship with ID {id} was not found");

        _internshipRepositoryMock.Verify(r => r.GetByIdAsync(id, false, cancellationToken), Times.Once);
        _internshipRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Internship>(), cancellationToken), Times.Never);
    }

    [Theory, CustomAutoData]
    public async Task CreateInternshipAsync_ShouldReturnMappedModel_WhenAllUsersAreValid(
        InternshipModel model,
        Internship entity,
        Internship createdEntity,
        InternshipModel expectedModel,
        CancellationToken cancellationToken)
    {
        // Arrange
        var intern = _fixture.Build<User>().With(u => u.Role, Role.Intern).Create();
        var mentor = _fixture.Build<User>().With(u => u.Role, Role.Mentor).Create();
        var hrManager = _fixture.Build<User>().With(u => u.Role, Role.HRManager).Create();

        _userRepositoryMock.Setup(r => r.GetByIdAsync(model.InternId, false, cancellationToken)).ReturnsAsync(intern);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(model.MentorId, false, cancellationToken)).ReturnsAsync(mentor);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(model.HumanResourcesManagerId, false, cancellationToken)).ReturnsAsync(hrManager);

        _mapperMock.Setup(m => m.Map<Internship>(model)).Returns(entity);
        _internshipRepositoryMock.Setup(r => r.CreateAsync(entity, cancellationToken)).ReturnsAsync(createdEntity);
        _mapperMock.Setup(m => m.Map<InternshipModel>(createdEntity)).Returns(expectedModel);

        // Act
        var result = await _internshipService.CreateInternshipAsync(model, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedModel);

        _userRepositoryMock.Verify(r => r.GetByIdAsync(model.InternId, false, cancellationToken), Times.Once);
        _userRepositoryMock.Verify(r => r.GetByIdAsync(model.MentorId, false, cancellationToken), Times.Once);
        _userRepositoryMock.Verify(r => r.GetByIdAsync(model.HumanResourcesManagerId, false, cancellationToken), Times.Once);
        _internshipRepositoryMock.Verify(r => r.CreateAsync(entity, cancellationToken), Times.Once);
    }

    [Theory, CustomAutoData]
    public async Task CreateInternshipAsync_ShouldThrowNotFound_WhenInternDoesNotExist(
        InternshipModel model, CancellationToken cancellationToken)
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.GetByIdAsync(model.InternId, false, cancellationToken)).ReturnsAsync((User?)null);

        // Act
        var act = async () => await _internshipService.CreateInternshipAsync(model, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"User with ID {model.InternId} was not found");

        _userRepositoryMock.Verify(r => r.GetByIdAsync(model.InternId, false, cancellationToken), Times.Once);
    }

    [Theory, CustomAutoData]
    public async Task CreateInternshipAsync_ShouldThrowIncorrectAssignment_WhenMentorHasWrongRole(
        InternshipModel model, CancellationToken cancellationToken)
    {
        // Arrange
        var intern = _fixture.Build<User>().With(u => u.Role, Role.Intern).Create();
        var mentor = _fixture.Build<User>().With(u => u.Role, Role.HRManager).Create(); // Wrong role
        var hrManager = _fixture.Build<User>().With(u => u.Role, Role.HRManager).Create();

        _userRepositoryMock.Setup(r => r.GetByIdAsync(model.InternId, false, cancellationToken)).ReturnsAsync(intern);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(model.MentorId, false, cancellationToken)).ReturnsAsync(mentor);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(model.HumanResourcesManagerId, false, cancellationToken)).ReturnsAsync(hrManager);

        // Act
        var act = async () => await _internshipService.CreateInternshipAsync(model, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<IncorrectAssignmentException>().WithMessage("User assigned to role mentor is not a mentor");
    }

    [Theory, CustomAutoData]
    public async Task GetInternshipsByMentorIdAsync_ShouldReturnListOfInternshipModels_WhenMentorExists(Guid mentorId,
       List<Internship> internships, List<InternshipModel> internshipModels,CancellationToken cancellationToken)
    {
        // Arrange
        _internshipRepositoryMock .Setup(r => r.GetAllAsync(i => i.MentorId == mentorId, false, cancellationToken))
            .ReturnsAsync(internships);

        _mapperMock.Setup(m => m.Map<List<InternshipModel>>(internships)) .Returns(internshipModels);

        // Act
        var result = await _internshipService.GetInternshipsByMentorIdAsync(mentorId, false, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(internshipModels);

        _internshipRepositoryMock.Verify(r => r.GetAllAsync(i => i.MentorId == mentorId, false, cancellationToken), Times.Once);
        _mapperMock.Verify(m => m.Map<List<InternshipModel>>(internships), Times.Once);
    }
}
