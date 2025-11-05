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

        var result = Deserialize<PagedList<UserDto>>(contentString);

        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(2);
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

        var result = Deserialize<PagedList<UserDto>>(contentString);

        result.ShouldNotBeNull();

        result.Items.ShouldAllBe(u => u.Role == Role.Intern);
    }

    [Fact]
    public async Task GetAll_ShouldReturnUsersSortedByFirstNameAscending_WhenAccordingSortingApplied()
    {
        // Arrange
        var prefix = Guid.NewGuid().ToString();

        var users = new[]
        {
            TestDataHelper.CreateUser(firstname: $"{prefix}_Charlie"),
            TestDataHelper.CreateUser(firstname: $"{prefix}_Alice"),
            TestDataHelper.CreateUser(firstname: $"{prefix}_Bob"),
            TestDataHelper.CreateUser(firstname: $"{prefix}_Mike")
        };

        await AddEntitiesAsync(users);

        var url = $"{Users.Base}?PageNumber=1&PageSize=100&Sorter={(int)UserSortingParameter.AscendingFirstName}";

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var contentString = await response.Content.ReadAsStringAsync();

        var result = Deserialize<PagedList<UserDto>>(contentString);

        result.ShouldNotBeNull();

        var names = result.Items
            .Where(u => u.Firstname.StartsWith(prefix))
            .Select(u => u.Firstname.Replace($"{prefix}_", ""))
            .ToList();

        names.ShouldBe(["Alice", "Bob", "Charlie", "Mike"]);
    }

    [Fact]
    public async Task GetAll_ShouldReturnUsersSortedByFirstNameDescending_WhenAccordingSortingApplied()
    {
        // Arrange
        var prefix = Guid.NewGuid().ToString();

        var users = new[]
        {
            TestDataHelper.CreateUser(firstname: $"{prefix}_Charlie"),
            TestDataHelper.CreateUser(firstname: $"{prefix}_Alice"),
            TestDataHelper.CreateUser(firstname: $"{prefix}_Bob"),
            TestDataHelper.CreateUser(firstname: $"{prefix}_Mike")
        };

        await AddEntitiesAsync(users);

        var url = $"{Users.Base}?PageNumber=1&PageSize=100&Sorter={(int)UserSortingParameter.DescendingFirstName}";

        // Act
        var response = await Client.GetAsync(url);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var contentString = await response.Content.ReadAsStringAsync();

        var result = Deserialize<PagedList<UserDto>>(contentString);

        result.ShouldNotBeNull();

        var names = result.Items
            .Where(u => u.Firstname.StartsWith(prefix))
            .Select(u => u.Firstname.Replace($"{prefix}_", ""))
            .ToList();

        names.ShouldBe(["Mike", "Charlie", "Bob", "Alice"]);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedUser()
    {
        // Arrange
        var createDto = new UserDto
        {
            Firstname = "New",
            Lastname = "User",
            Patronymic = "Testovich",
            Email = "new.user@test.com",
            PhoneNumber = "+375297180451",
            Role = Role.Intern
        };

        // Act
        var response = await Client.PostAsJsonAsync(Users.Base, createDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<UserDto>();

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

        var updateDto = new UpdateUserDto(
            Email : user.Email, 
            PhoneNumber: "+375297180454", 
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

        var result = await responce.Content.ReadFromJsonAsync<UpdateUserDto>();

        result.ShouldNotBeNull();
        result.Email.ShouldBe(user.Email);
        result.PhoneNumber.ShouldBe("+375297180454");
        result.Firstname.ShouldBe(user.Firstname);
        result.Lastname.ShouldBe(user.Lastname);
        result.Patronymic.ShouldBe(user.Patronymic);
        result.Role.ShouldBe(Role.Intern);
    }
}
