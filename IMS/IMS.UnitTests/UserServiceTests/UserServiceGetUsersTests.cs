namespace IMS.UnitTests.UserServiceTests;

public class UserServiceGetUsersTests : UserServiceTestsBase
{
    [Fact]
    public async Task GetUsersAsync_ShouldReturnEmptyMappedPagedList_WhenRepositoryReturnsEmptyPagedList()
    {
        //Arrange
        var paginationParameters = Fixture.Create<PaginationParameters>();
        var filter = Fixture.Create<UserFilteringParameters>();
        var sorter = Fixture.Create<UserSortingParameter>();

        var emptyPagedUsers = new PagedList<User>([], 1, 10, 0);
        var emptyMappedPagedUsers = new PagedList<UserModel>([], 1, 10, 0);

        UserRepositoryMock.Setup(r => r.GetAllAsync(paginationParameters, filter, sorter, false, default))
            .ReturnsAsync(emptyPagedUsers);

        MapperMock.Setup(m => m.Map<PagedList<UserModel>>(emptyPagedUsers)).Returns(emptyMappedPagedUsers);

        //Act
        var result = await UserService.GetUsersAsync(paginationParameters, filter, sorter);

        //Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(emptyMappedPagedUsers.Items);
        result.PageNumber.Should().Be(emptyMappedPagedUsers.PageNumber);
        result.PageSize.Should().Be(emptyMappedPagedUsers.PageSize);
        result.TotalCount.Should().Be(emptyMappedPagedUsers.TotalCount);

        UserRepositoryMock.Verify(r => r.GetAllAsync(paginationParameters, filter, sorter, false, default), Times.Once());
        MapperMock.Verify(m => m.Map<PagedList<UserModel>>(emptyPagedUsers), Times.Once());
    }

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
