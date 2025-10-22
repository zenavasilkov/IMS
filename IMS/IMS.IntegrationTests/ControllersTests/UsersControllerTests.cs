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
}
