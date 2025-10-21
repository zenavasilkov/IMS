namespace IMS.UnitTests.UserServiceTests;

public class UserServiceGetByIdTests : UserServiceTestsBase
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedModel_WhenUserExists()
    {
        //Arrange
        var id = Guid.NewGuid();
        var entity = Fixture.Build<User>().With(u => u.Id, id).Create();
        var model = Fixture.Build<UserModel>().With(u => u.Id, id).Create();

        UserRepositoryMock.Setup(r => r.GetByIdAsync(id, false, default)).ReturnsAsync(entity); 
        MapperMock.Setup(m => m.Map<UserModel>(entity)).Returns(model);

        //Act
        var result = await UserService.GetByIdAsync(id);

        //Assert
        result.Should().BeEquivalentTo(model);
        UserRepositoryMock.Verify(r => r.GetByIdAsync(id, false, default), Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowException_WhenUserDoNotExists()
    {
        //Arrange
        var id = Guid.NewGuid();

        UserRepositoryMock.Setup(r => r.GetByIdAsync(id, false, default)).ReturnsAsync((User?)null);

        //Act 
        var act = async() => await UserService.GetByIdAsync(id);

        //Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No entity has been found by given ID"); //TODO: Change to custom exception after merging

        UserRepositoryMock.Verify(r => r.GetByIdAsync(id, false, default), Times.Once);
    }
}
