namespace IMS.UnitTests.Service;

public class UserServiceCreateTests : UserServiceTestsBase 
{
    [Fact]
    public async Task CreateAsync_ShouldReturnMappedModel_WhenCreationSucceeded()
    {
        //Arrange
        var inputModel = Fixture.Create<UserModel>();

        var mappedEntity = Fixture.Build<User>()
            .With(u => u.Email, inputModel.Email)
            .With(u => u.Firstname, inputModel.Firstname)
            .With(u => u.Lastname, inputModel.Lastname)
            .With(u => u.PhoneNumber, inputModel.PhoneNumber)
            .Create();

        var createdEntity = Fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .With(u => u.Email, mappedEntity.Email)
            .With(u => u.Firstname, mappedEntity.Firstname)
            .With(u => u.Lastname, mappedEntity.Lastname)
            .With(u => u.PhoneNumber, mappedEntity.PhoneNumber)
            .Create();

        var mappedBackModel = Fixture.Build<UserModel>()
            .With(m => m.Id, createdEntity.Id)
            .With(m => m.Email, createdEntity.Email)
            .With(m => m.Firstname, createdEntity.Firstname)
            .With(m => m.Lastname, createdEntity.Lastname)
            .With(m => m.PhoneNumber, createdEntity.PhoneNumber)
            .Create();

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
