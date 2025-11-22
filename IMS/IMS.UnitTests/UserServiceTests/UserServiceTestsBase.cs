using IMS.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace IMS.UnitTests.UserServiceTests;

public class UserServiceTestsBase
{
    protected readonly IFixture Fixture;
    protected readonly Mock<IUserRepository> UserRepositoryMock;
    protected readonly Mock<IMapper> MapperMock;
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

        var messageServiceMock = new Mock<IMessageService>();
        messageServiceMock.Setup(m => m.NotifyUserCreated(
            It.IsAny<UserCreatedEvent>(), 
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        var environmentMock = new Mock<IWebHostEnvironment>();
        var auth0ClientFactoryMock = new Mock<IAuth0ClientFactory>();
        
        UserService = new UserService(
            UserRepositoryMock.Object, 
            MapperMock.Object,
            messageServiceMock.Object,
            auth0ClientFactoryMock.Object,
            environmentMock.Object);
    }
}
