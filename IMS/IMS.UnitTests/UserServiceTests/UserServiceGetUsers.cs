namespace IMS.UnitTests.UserServiceTests;

public class UserServiceGetUsers : UserServiceTestsBase
{
    [Fact]
    public async Task GetUsersAsync_ShouldReturnMappedPagedList_WhenUsersExist()
    {
        //Arrange
        var paginationParameters = Fixture.Create<PaginationParameters>();
        var filter = Fixture.Create<UserFilteringParameters>();
        var sorter = Fixture.Create<UserSortingParameter>();

        var users = Fixture.CreateMany<User>(3).ToList();
        var pagedUsers = new PagedList<User>(users, 1, 10, 3);

        var mappedUsers = Fixture.CreateMany<UserModel>(3).ToList();
        var mappedPagedUsers = new PagedList<UserModel>(mappedUsers, 1, 10, 3);

        UserRepositoryMock.Setup(r => r.GetAllAsync(paginationParameters, filter, sorter, false, default))
            .ReturnsAsync(pagedUsers);

        MapperMock.Setup(m => m.Map<PagedList<UserModel>>(pagedUsers)).Returns(mappedPagedUsers);

        //Act
        var result = await UserService.GetUsersAsync(paginationParameters, filter, sorter);

        //Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(mappedPagedUsers.Items);
        result.PageNumber.Should().Be(mappedPagedUsers.PageNumber);
        result.PageSize.Should().Be(mappedPagedUsers.PageSize);
        result.TotalCount.Should().Be(mappedPagedUsers.TotalCount);

        UserRepositoryMock.Verify(r => r.GetAllAsync(paginationParameters, filter, sorter, false, default), Times.Once());
        MapperMock.Verify(m => m.Map<PagedList<UserModel>>(pagedUsers), Times.Once());
    }
}
