using IMS.Presentation.DTOs.GetDTO;
using Shared.Pagination;

namespace IMS.IntegrationTests.ControllersTests;

public class UsersControllerTests(CustomWebApplicationFactory factory) : TestHelperBase(factory)
{
    private readonly CustomWebApplicationFactory _factory = factory;

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        //Arrange
        var user = TestDataHelper.CreateUser();
        await AddEntityAsync(user);

        //Act
        var response = await Client.GetAsync($"{ApiRoutes.Users.Base}/{user.Id}");

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFoundStatus_WhenUserDoesNotExist()
    {
        //Arrange

        //Act
        var response = await Client.GetAsync($"{ApiRoutes.Users.Base}/{Guid.NewGuid()}");

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_ShouldReturnPaginatedList()
    {
        //Arrange
        var users = new[]
        {
            TestDataHelper.CreateUser(firstname: "John", lastname: "Smith", role: Role.Mentor),
            TestDataHelper.CreateUser(firstname: "Alice", lastname: "Brown", role: Role.Intern),
            TestDataHelper.CreateUser(firstname: "Michael", lastname: "Davis", role: Role.HRManager)
        };

        await AddEntitiesAsync(users);

        var url = $"{ApiRoutes.Users.Base}?PageNumber=1&PageSize=2";

        //Act
        var response = await Client.GetAsync(url);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = Deserialize<PagedList<UserDTO>>(await response.Content.ReadAsStringAsync());
        result.ShouldNotBeNull();

        result.Items.Count.ShouldBe(3);
        result.TotalCount.ShouldBe(3);
    }
}
