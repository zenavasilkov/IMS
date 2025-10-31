namespace IMS.IntegrationTests.ControllersTests;

public class FeedbacksControllerTests(CustomWebApplicationFactory factory) : TestHelperBase(factory)
{
    [Fact]
    public async Task GetAll_ShouldReturnListOfFeedbacks_WhenFeedbacksExist()
    {
        // Arrange
        var sender = TestDataHelper.CreateUser();
        var recipient = TestDataHelper.CreateUser();
        var ticket = TestDataHelper.CreateTicket();
        var feedback1 = TestDataHelper.CreateFeedback(ticket: ticket, sentBy: sender, addressedTo: recipient);
        var feedback2 = TestDataHelper.CreateFeedback(ticket: ticket, sentBy: sender, addressedTo: recipient);

        await AddEntitiesAsync([feedback1, feedback2]);

        // Act
        var response = await Client.GetAsync(Feedbacks.Base);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<FeedbackDto>>();

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldContain(f => f.SentById == sender.Id);
        result.ShouldContain(f => f.AddressedToId == recipient.Id);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoFeedbacksExist()
    {
        // Act
        var response = await Client.GetAsync(Feedbacks.Base);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<FeedbackDto>>();

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetById_ShouldReturnFeedback_WhenFeedbackExists()
    {
        // Arrange
        var ticket = TestDataHelper.CreateTicket();
        var feedback = TestDataHelper.CreateFeedback(ticket.Id);

        await AddEntityAsync(feedback);

        // Act
        var response = await Client.GetAsync($"{Feedbacks.Base}/{feedback.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<FeedbackDto>();

        result.ShouldNotBeNull();
        result.Id.ShouldBe(feedback.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenFeedbackDoesNotExist()
    {
        // Act
        var response = await Client.GetAsync($"{Feedbacks.Base}/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetFeedbacksByTicketId_ShouldReturnList_WhenFeedbacksExist()
    {
        // Arrange
        var ticket = TestDataHelper.CreateTicket();
        var feedback1 = TestDataHelper.CreateFeedback(ticket : ticket);
        var feedback2 = TestDataHelper.CreateFeedback(ticket : ticket);

        await AddEntitiesAsync([feedback1, feedback2]);

        // Act
        var response = await Client.GetAsync($"{Feedbacks.Base}/by-ticket/{ticket.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<FeedbackDto>>();

        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.All(f => f.TicketId == ticket.Id).ShouldBeTrue();
    }

    [Fact]
    public async Task GetFeedbacksByTicketId_ShouldReturnEmptyList_WhenNoFeedbacksExist()
    {
        // Arrange
        var ticket = TestDataHelper.CreateTicket();
        await AddEntityAsync(ticket);

        // Act
        var response = await Client.GetAsync($"{Feedbacks.Base}/by-ticket/{ticket.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<FeedbackDto>>();

        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedFeedback_WhenDataIsValid()
    {
        // Arrange
        var ticket = TestDataHelper.CreateTicket();
        var sender = TestDataHelper.CreateUser();
        var recipient = TestDataHelper.CreateUser();

        await AddEntityAsync(ticket);
        await AddEntitiesAsync([sender, recipient]);

        var dto = new CreateFeedbackDto(ticket.Id, sender.Id, recipient.Id, "Excellent work!");

        // Act
        var response = await Client.PostAsJsonAsync(Feedbacks.Base, dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<FeedbackDto>();

        result.ShouldNotBeNull();
        result.SentById.ShouldBe(sender.Id);
        result.AddressedToId.ShouldBe(recipient.Id);
        result.TicketId.ShouldBe(ticket.Id);
        result.Comment.ShouldBe("Excellent work!");
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        var dto = new CreateFeedbackDto(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Invalid ticket");

        // Act
        var response = await Client.PostAsJsonAsync(Feedbacks.Base, dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedFeedback_WhenFeedbackExists()
    {
        // Arrange
        var feedback = TestDataHelper.CreateFeedback();

        await AddEntityAsync(feedback);

        var updateDto = new UpdateFeedbackDto("Updated comment");

        // Act
        var response = await Client.PutAsJsonAsync($"{Feedbacks.Base}/{feedback.Id}", updateDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<FeedbackDto>();

        result.ShouldNotBeNull();
        result.Comment.ShouldBe("Updated comment");
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenFeedbackDoesNotExist()
    {
        // Arrange
        var updateDto = new UpdateFeedbackDto("Does not exist");

        // Act
        var response = await Client.PutAsJsonAsync($"{Feedbacks.Base}/{Guid.NewGuid()}", updateDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
