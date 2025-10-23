using IMS.Presentation.DTOs.UpdateDTO;
using System.Net.Http.Json;

namespace IMS.IntegrationTests.ControllersTests;

public class UsersControllerTests(CustomWebApplicationFactory factory) : TestHelperBase(factory)
{
    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        //Arrange
        var user = TestDataHelper.CreateUser();
        await AddEntityAsync(user);

        //Act
        var response = await Client.GetAsync($"{Users.Base}/{user.Id}");

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFoundStatus_WhenUserDoesNotExist()
    {
        //Arrange

        //Act
        var response = await Client.GetAsync($"{Users.Base}/{Guid.NewGuid()}");

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

        var url = $"{Users.Base}?PageNumber=1&PageSize=2";

        //Act
        var response = await Client.GetAsync(url);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var contentString = await response.Content.ReadAsStringAsync();

        var result = Deserialize<PagedList<UserDTO>>(contentString);
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(2);
        result.TotalCount.ShouldBe(3);
        result.Items.ShouldContain(u => u.Firstname == "John");
        result.Items.ShouldContain(u => u.Firstname == "Alice");
        result.Items.ShouldNotContain(u => u.Firstname == "Michael");
    }

    [Fact]
    public async Task GetAll_ShouldFilterByRole_WhenFiltersApplied()
    {
        // Arrange
        var intern1 = TestDataHelper.CreateUser(role: Role.Intern);
        var intern2 = TestDataHelper.CreateUser(role: Role.Intern);
        var mentor = TestDataHelper.CreateUser(role: Role.Mentor);
        var hrManager = TestDataHelper.CreateUser(role: Role.HRManager);

        await AddEntitiesAsync([intern1,intern2, mentor, hrManager]);

        var url = $"{Users.Base}?Role={(int)Role.Intern}";

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var contentString = await response.Content.ReadAsStringAsync();

        var result = Deserialize<PagedList<UserDTO>>(contentString);

        result.ShouldNotBeNull();

        result.Items.ShouldAllBe(u => u.Role == Role.Intern);
    }

    [Fact]
    public async Task GetAll_ShouldReturnUsersSortedByFirstNameAscending_WhenAccordingSortingApplied()
    {
        // Arrange
        var users = new[]
        {
            TestDataHelper.CreateUser(firstname: "Charlie"),
            TestDataHelper.CreateUser(firstname: "Alice"),
            TestDataHelper.CreateUser(firstname: "Bob"),
            TestDataHelper.CreateUser(firstname: "Mike")
        };

        await AddEntitiesAsync(users);

        var url = $"{Users.Base}?PageNumber=1&PageSize=4&Sorter={(int)UserSortingParameter.AscendingFirstName}";

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var contentString = await response.Content.ReadAsStringAsync();

        var result = Deserialize<PagedList<UserDTO>>(contentString);
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(4);

        var names = result.Items.Select(u => u.Firstname).ToList();
        names.ShouldBe(["Alice", "Bob", "Charlie", "Mike"]);
    }

    [Fact]
    public async Task GetAll_ShouldReturnUsersSortedByFirstNameDescending_WhenAccordingSortingApplied()
    {
        // Arrange
        var users = new[]
        {
            TestDataHelper.CreateUser(firstname: "Charlie"),
            TestDataHelper.CreateUser(firstname: "Alice"),
            TestDataHelper.CreateUser(firstname: "Bob"),
            TestDataHelper.CreateUser(firstname: "Mike")
        };

        await AddEntitiesAsync(users);

        var url = $"{Users.Base}?PageNumber=1&PageSize=4&Sorter={(int)UserSortingParameter.DescendingFirstName}";

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var contentString = await response.Content.ReadAsStringAsync();

        var result = Deserialize<PagedList<UserDTO>>(contentString);
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(4);

        var names = result.Items.Select(u => u.Firstname).ToList();
        names.ShouldBe(["Mike", "Charlie", "Bob", "Alice"]);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedUser()
    {
        // Arrange
        var createDto = new UserDTO
        {
            Firstname = "New",
            Lastname = "User",
            Patronymic = "Testovich",
            Email = "new.user@test.com",
            PhoneNumber = "+375-29-718-04-51",
            Role = Role.Intern
        };

        // Act
        var response = await Client.PostAsJsonAsync(Users.Base, createDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<UserDTO>();
        result.ShouldNotBeNull();
        result.Firstname.ShouldBe("New");
        result.Email.ShouldBe("new.user@test.com");
        result.Role.ShouldBe(Role.Intern);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdaterUser_WhenUserExists()
    {
        //Arrange
        var user = TestDataHelper.CreateUser();

        var updateDto = new UpdateUserDTO(
            Email : user.Email, 
            PhoneNumber: "+375-29-718-04-54", 
            Firstname: user.Firstname, 
            Lastname: user.Lastname, 
            Patronymic: user.Patronymic, 
            Role: user.Role);

        await AddEntityAsync(user);

        var url = $"{Users.Base}/{user.Id}";

        //Act
        var responce = await Client.PutAsJsonAsync(url, updateDto);

        //Assert
        responce.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await responce.Content.ReadFromJsonAsync<UpdateUserDTO>();
        result.ShouldNotBeNull();
        result.Email.ShouldBe(user.Email);
        result.PhoneNumber.ShouldBe("+375-29-718-04-54");
        result.Firstname.ShouldBe(user.Firstname);
        result.Lastname.ShouldBe(user.Lastname);
        result.Patronymic.ShouldBe(user.Patronymic);
        result.Role.ShouldBe(Role.Intern);
    }
}
