namespace IMS.UnitTests.UserServiceTests;

public class UserServiceCreateTests : UserServiceTestsBase 
{
    [Fact]
    public async Task CreateAsync_ShouldReturnMappedModel_WhenCreationSucceeded()
    {
        //Arrange
        var inputModel = Fixture.Create<UserModel>();
        var mappedEntity = Fixture.Create<User>();
        var createdEntity = Fixture.Create<User>();
        var mappedBackModel = inputModel;

        MapperMock.Setup(m => m.Map<User>(inputModel)).Returns(mappedEntity);
        UserRepositoryMock.Setup(r => r.CreateAsync(mappedEntity, default)).ReturnsAsync(createdEntity);
        MapperMock.Setup(m => m.Map<UserModel>(createdEntity)).Returns(mappedBackModel);

        //Act
        var result = await UserService.CreateAsync(inputModel);

        //Assert
        result.Should().BeEquivalentTo(mappedBackModel);

        MapperMock.Verify(m => m.Map<User>(inputModel), Times.Once());
        UserRepositoryMock.Verify(r => r.CreateAsync(mappedEntity, default), Times.Once());
        MapperMock.Verify(m => m.Map<UserModel>(createdEntity), Times.Once());
    }
}
