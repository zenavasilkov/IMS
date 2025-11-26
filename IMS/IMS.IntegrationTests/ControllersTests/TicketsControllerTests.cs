namespace IMS.IntegrationTests.ControllersTests;

public class TicketsControllerTests(CustomWebApplicationFactory factory) : TestHelperBase(factory)
{
    [Fact]
    public async Task GetAll_ShouldReturnListOfTickets_WhenTicketsExist()
    {
        //Arrange
        var board = TestDataHelper.CreateBoard();
        var ticket1 = TestDataHelper.CreateTicket(board: board, status: TicketStatus.ToDo);
        var ticket2 = TestDataHelper.CreateTicket(board: board, status: TicketStatus.Done);

        await AddEntitiesAsync([ticket1, ticket2]);

        //Act
        var response = await Client.GetAsync(Tickets.Base);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadAsStringAsync();

        var tickets = Deserialize<List<TicketDto>>(result);

        tickets.ShouldNotBeNull();
        tickets.ShouldContain(t => t.Status == TicketStatus.Done);
        tickets.ShouldContain(t => t.Status == TicketStatus.ToDo);
    }

    [Fact]
    public async Task GetById_ShouldReturnTicket_WhenTicketExists()
    {
        //Arrange
        var board = TestDataHelper.CreateBoard();
        var ticket = TestDataHelper.CreateTicket(board: board, status: TicketStatus.Done);

        await AddEntityAsync(ticket);

        //Act
        var response = await Client.GetAsync($"{Tickets.Base}/{ticket.Id}");

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadAsStringAsync();

        var ticketDto = Deserialize<TicketDto>(result);

        ticketDto.ShouldNotBeNull();
        ticketDto.Status.ShouldBe(TicketStatus.Done);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFoundStatus_WhenTicketDoesNotExist()
    {
        //Act
        var response = await Client.GetAsync($"{Tickets.Base}/{Guid.NewGuid()}");

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedTicket_WhenDataIsValid()
    {
        // Arrange
        var board = TestDataHelper.CreateBoard();

        var createdTicketDto = new CreateTicketDto(board.Id, "New ticket", 
            "New description", TicketStatus.ToDo, DateTime.UtcNow.AddDays(10));

        await AddEntityAsync(board);

        // Act
        var response = await Client.PostAsJsonAsync(Tickets.Base, createdTicketDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var created = await response.Content.ReadFromJsonAsync<TicketDto>();

        created.ShouldNotBeNull();
        created.Title.ShouldBe("New ticket");
        created.Description.ShouldBe("New description");
    }

    [Fact]
    public async Task Create_ShouldReturnNotFoundStatus_WhenBoarDoesNotExist()
    {
        // Arrange
        var createdTicketDto = new CreateTicketDto(Guid.NewGuid(), "New ticket",
            "New description", TicketStatus.ToDo, DateTime.UtcNow.AddDays(10));

        // Act
        var response = await Client.PostAsJsonAsync(Tickets.Base, createdTicketDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedTicket_WhenDataIsValid()
    {
        // Arrange
        var board = TestDataHelper.CreateBoard();
        var ticket = TestDataHelper.CreateTicket(board: board, status: TicketStatus.ToDo);

        await AddEntityAsync(ticket);

        var updateTicketDto = new UpdateTicketDto(
            "Updated title",
            "Updated description",
            TicketStatus.Done,
            DateTime.UtcNow.AddDays(3)
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{Tickets.Base}/{ticket.Id}", updateTicketDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<TicketDto>();

        updated.ShouldNotBeNull();
        updated.Title.ShouldBe("Updated title");
        updated.Description.ShouldBe("Updated description");
        updated.Status.ShouldBe(TicketStatus.Done);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFoundStatus_WhenTicketDoesNotExist()
    {
        // Arrange
        var updateTicketDto = new UpdateTicketDto(
            "Non-existent ticket",
            "Does not exist",
            TicketStatus.Done,
            DateTime.UtcNow.AddDays(5)
        );

        // Act
        var response = await Client.PutAsJsonAsync($"{Tickets.Base}/{Guid.NewGuid()}", updateTicketDto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTicketsByBoardId_ShouldReturnTickets_WhenBoardHasTickets()
    {
        // Arrange
        var board = TestDataHelper.CreateBoard();
        var ticket1 = TestDataHelper.CreateTicket(board: board, status: TicketStatus.ToDo);
        var ticket2 = TestDataHelper.CreateTicket(board: board, status: TicketStatus.Done);

        await AddEntitiesAsync([ticket1, ticket2]);

        // Act
        var response = await Client.GetAsync($"{Tickets.Base}/by-board/{board.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var tickets = Deserialize<List<TicketDto>>(content);

        tickets.ShouldNotBeNull();
        tickets.Count.ShouldBe(2);
        tickets.ShouldContain(t => t.Status == TicketStatus.ToDo);
        tickets.ShouldContain(t => t.Status == TicketStatus.Done);
    }

    [Fact]
    public async Task GetTicketsByBoardId_ShouldReturnEmptyList_WhenBoardHasNoTickets()
    {
        // Arrange
        var board = TestDataHelper.CreateBoard();
        await AddEntityAsync(board);

        // Act
        var response = await Client.GetAsync($"{Tickets.Base}/by-board/{board.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var tickets = Deserialize<List<TicketDto>>(content);

        tickets.ShouldNotBeNull();
        tickets.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetTicketsByBoardId_ShouldReturnNotFound_WhenBoardDoesNotExist()
    {
        // Act
        var response = await Client.GetAsync($"{Tickets.Base}/by-board/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

}
