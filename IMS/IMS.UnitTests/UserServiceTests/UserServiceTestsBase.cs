namespace IMS.UnitTests.UserServiceTests;

public class UserServiceTestsBase
{
    protected readonly IFixture Fixture;
    protected readonly Mock<IUserRepository> UserRepositoryMock;
    protected readonly Mock<IMapper> MapperMock;
    protected readonly Mock<IMessageService> MessageServiceMock;
    protected readonly UserService UserService;

    protected UserServiceTestsBase()
    {
        Fixture = new Fixture();

        Fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));

        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        UserRepositoryMock = Fixture.Freeze<Mock<IUserRepository>>();
        MapperMock = Fixture.Freeze<Mock<IMapper>>();

        MessageServiceMock = new Mock<IMessageService>();
        MessageServiceMock.Setup(m => m.NotifyUserCreated(
            It.IsAny<UserCreatedEvent>(), 
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        UserService = new UserService(
            UserRepositoryMock.Object, 
            MapperMock.Object,
            MessageServiceMock.Object);
    }
}
