namespace IMS.UnitTests.UserServiceTests;

public class UserServiceTestsBase
{
    protected readonly IFixture Fixture;
    protected readonly Mock<IUserRepository> UserRepositoryMock;
    protected readonly Mock<IMapper> MapperMock;
    private readonly Mock<ILogger<UserService>> LoggerMock;
    protected readonly UserService UserService;

    protected UserServiceTestsBase()
    {
        Fixture = new Fixture().Customize(new AutoMoqCustomization()); ;

        Fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));

        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        UserRepositoryMock = Fixture.Freeze<Mock<IUserRepository>>();
        MapperMock = Fixture.Freeze<Mock<IMapper>>();
        LoggerMock = Fixture.Freeze<Mock<ILogger<UserService>>>();

        UserService = new UserService(UserRepositoryMock.Object, 
            MapperMock.Object, LoggerMock.Object);
    }
}
