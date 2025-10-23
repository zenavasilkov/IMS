namespace IMS.IntegrationTests.ControllersTests;

public class InternshipsControllerTests(CustomWebApplicationFactory factory) : TestHelperBase(factory)
{
    [Fact]
    public async Task GetAll_ShouldReturnAllInternships()
    {
        // Arrange
        var internship1 = TestDataHelper.CreateInternship();
        var internship2 = TestDataHelper.CreateInternship();

        await AddEntitiesAsync([internship1, internship2]);

        // Act
        var response = await Client.GetAsync(Internships.Base);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var contentString = await response.Content.ReadAsStringAsync();

        var internships = Deserialize<List<InternshipDTO>>(contentString);

        internships.ShouldNotBeNull();
        internships.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetById_ShouldReturnInternship_WhenExists()
    {
        // Arrange
        var internship = TestDataHelper.CreateInternship();

        await AddEntityAsync(internship);

        // Act
        var response = await Client.GetAsync($"{Internships.Base}/{internship.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<InternshipDTO>();

        result.ShouldNotBeNull();
        result.Id.ShouldBe(internship.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenInternshipDoesNotExist()
    {
        // Act
        var response = await Client.GetAsync($"{Internships.Base}/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetInternshipsByMentorId_ShouldReturnInternshipsForMentor()
    {
        // Arrange
        var mentor = TestDataHelper.CreateUser(role: Role.Mentor);
        var internship1 = TestDataHelper.CreateInternship(mentor: mentor);
        var internship2 = TestDataHelper.CreateInternship(mentor: mentor);

        await AddEntitiesAsync([internship1, internship2]);

        // Act
        var response = await Client.GetAsync($"{Internships.Base}/by-mentor/{mentor.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<InternshipDTO>>();

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedInternship_WhenValidData()
    {
        // Arrange
        var intern = TestDataHelper.CreateUser(role: Role.Intern);
        var mentor = TestDataHelper.CreateUser(role: Role.Mentor);
        var hr = TestDataHelper.CreateUser(role: Role.HRManager);

        await AddEntitiesAsync([intern, mentor, hr]);

        var createDto = new CreateInternshipDTO(intern.Id, mentor.Id, 
            hr.Id, DateTime.UtcNow, new DateTime(), InternshipStatus.Ongoing);

        // Act
        var response = await Client.PostAsJsonAsync(Internships.Base, createDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<InternshipDTO>();

        result.ShouldNotBeNull();
        result.InternId.ShouldBe(intern.Id);
        result.MentorId.ShouldBe(mentor.Id);
        result.HumanResourcesManagerId.ShouldBe(hr.Id);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenInternNotFound()
    {
        // Arrange
        var mentor = TestDataHelper.CreateUser(role: Role.Mentor);
        var hr = TestDataHelper.CreateUser(role: Role.HRManager);

        await AddEntitiesAsync([mentor, hr]);

        var createDto = new
        {
            InternId = Guid.NewGuid(),
            MentorId = mentor.Id,
            HumanResourcesManagerId = hr.Id,
            StartDate = DateTime.UtcNow,
            Status = InternshipStatus.NotStarted
        };

        // Act
        var response = await Client.PostAsJsonAsync(Internships.Base, createDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenMentorNotFound()
    {
        // Arrange
        var intern = TestDataHelper.CreateUser(role: Role.Intern);
        var hr = TestDataHelper.CreateUser(role: Role.HRManager);

        await AddEntitiesAsync([intern, hr]);

        var createDto = new
        {
            InternId = intern.Id,
            MentorId = Guid.NewGuid(), // invalid
            HumanResourcesManagerId = hr.Id,
            StartDate = DateTime.UtcNow,
            Status = InternshipStatus.NotStarted
        };

        // Act
        var response = await Client.PostAsJsonAsync(Internships.Base, createDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnIncorrectAssignment_WhenInternHasWrongRole()
    {
        // Arrange
        var intern = TestDataHelper.CreateUser(role: Role.Mentor);
        var mentor = TestDataHelper.CreateUser(role: Role.Mentor);
        var hr = TestDataHelper.CreateUser(role: Role.HRManager);

        await AddEntitiesAsync([intern, mentor, hr]);

        var createDto = new
        {
            InternId = intern.Id,
            MentorId = mentor.Id,
            HumanResourcesManagerId = hr.Id,
            StartDate = DateTime.UtcNow,
            Status = InternshipStatus.NotStarted
        };

        // Act
        var response = await Client.PostAsJsonAsync(Internships.Base, createDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_ShouldModifyInternship_WhenValid()
    {
        // Arrange
        var internship = TestDataHelper.CreateInternship(status: InternshipStatus.NotStarted);
        await AddEntityAsync(internship);

        var updateDto = new
        {
            InternshipId = internship.Id, 
            EndDate = DateTime.UtcNow.AddDays(10),
            Status = InternshipStatus.Completed, 
        };

        // Act
        var response = await Client.PutAsJsonAsync($"{Internships.Base}/{internship.Id}", updateDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<InternshipDTO>();

        updated.ShouldNotBeNull();
        updated.Status.ShouldBe(InternshipStatus.Completed);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenInternshipDoesNotExist()
    {
        // Arrange
        var updateDto = new
        {
            InternshipId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(5),
            Status = InternshipStatus.Completed
        };

        // Act
        var response = await Client.PutAsJsonAsync($"{Internships.Base}/{Guid.NewGuid()}", updateDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}