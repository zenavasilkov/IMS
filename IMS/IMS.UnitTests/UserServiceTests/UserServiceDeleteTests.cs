namespace IMS.UnitTests.UserServiceTests;

public class UserServiceDeleteTests : UserServiceTestsBase
{
    [Fact]
    public async Task DeleteAsync_DeletesUser_WhenUserExists()
    {
        //Arrange
        var userModel = Fixture.Create<UserModel>();
        var mappedUserEntity = Fixture.Create<User>();

        MapperMock.Setup(m => m.Map<User>(userModel)).Returns(mappedUserEntity);
        UserRepositoryMock.Setup(r => r.DeleteAsync(mappedUserEntity, default));

        //Act
        await UserService.DeleteAsync(userModel);

        //Assert
        MapperMock.Verify(m => m.Map<User>(userModel), Times.Once());
        UserRepositoryMock.Verify(r => r.DeleteAsync(mappedUserEntity, default), Times.Once());
    }
}
