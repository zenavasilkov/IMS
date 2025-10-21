namespace IMS.UnitTests.UserServiceTests;

public class UserServiceUpdateTests : UserServiceTestsBase
{
    [Fact]
    public async Task UpdateAsync_ShouldUpdateAndReturnMappedUser_WhenUserExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingUser = Fixture.Build<User>().With(u => u.Id, id).Create();
        var model = Fixture.Build<UserModel>().With(m => m.Id, id).Create();
        var updatedUser = Fixture.Build<User>().With(u => u.Id, id).Create();
        var updatedModel = Fixture.Build<UserModel>().With(m => m.Id, id).Create();

        UserRepositoryMock.Setup(r => r.GetByIdAsync(id, false, default))
            .ReturnsAsync(existingUser);

        UserRepositoryMock.Setup(r => r.UpdateAsync(existingUser, default))
            .ReturnsAsync(updatedUser);

        MapperMock.Setup(m => m.Map<UserModel>(updatedUser))
            .Returns(updatedModel);

        // Act
        var result = await UserService.UpdateAsync(id, model);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(updatedModel);

        existingUser.Email.Should().Be(model.Email);
        existingUser.Firstname.Should().Be(model.Firstname);
        existingUser.Lastname.Should().Be(model.Lastname);
        existingUser.PhoneNumber.Should().Be(model.PhoneNumber);
        existingUser.Role.Should().Be(model.Role);

        UserRepositoryMock.Verify(r => r.GetByIdAsync(id, false, default), Times.Once);
        UserRepositoryMock.Verify(r => r.UpdateAsync(existingUser, default), Times.Once);
        MapperMock.Verify(m => m.Map<UserModel>(updatedUser), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = Fixture.Build<UserModel>().With(m => m.Id, id).Create();

        UserRepositoryMock.Setup(r => r.GetByIdAsync(id, false, default))
            .ReturnsAsync((User?)null);

        // Act
        var act = async() => await UserService.UpdateAsync(id, model);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"User with ID {id} was not found"); //TODO: Change to custom exception

        UserRepositoryMock.Verify(r => r.GetByIdAsync(id, false, default), Times.Once);
        UserRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>(), default), Times.Never);
        MapperMock.Verify(m => m.Map<UserModel>(It.IsAny<User>()), Times.Never);
    }
}
