namespace IMS.IntegrationTests.ControllersTests;

public class BoardControllerTests(CustomWebApplicationFactory factory) : TestHelperBase(factory)
{
    [Fact]
    public async Task GetAll_ShouldReturnBoards_WhenBoardsExist()
    {
        // Arrange
        var board1 = TestDataHelper.CreateBoard(title: "Backend Board");
        var board2 = TestDataHelper.CreateBoard(title: "Frontend Board");

        await AddEntitiesAsync([board1, board2]);

        // Act
        var response = await Client.GetAsync(Boards.Base);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var boards = Deserialize<List<Board>>(content);

        boards.ShouldNotBeNull();
        boards.ShouldContain(b => b.Title == "Backend Board");
        boards.ShouldContain(b => b.Title == "Frontend Board");
    }

    [Fact]
    public async Task GetById_ShouldReturnBoard_WhenBoardExists()
    {
        // Arrange
        var board = TestDataHelper.CreateBoard();
        await AddEntityAsync(board);

        // Act
        var response = await Client.GetAsync($"{Boards.Base}/{board.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var result = Deserialize<Board>(content);

        result.ShouldNotBeNull();
        result!.Id.ShouldBe(board.Id);
        result.Title.ShouldBe(board.Title);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFoundStatus_WhenBoardDoesNotExist()
    {
        //Act
        var responce = await Client.GetAsync($"{Boards.Base}/{Guid.NewGuid()}");

        //Assert
        responce.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedBoard_WhenDataIsValid()
    {
        // Arrange
        var userMentor = TestDataHelper.CreateUser(role: Role.Mentor);
        var userIntern = TestDataHelper.CreateUser(role: Role.Intern);

        await AddEntitiesAsync([userMentor, userIntern]);

        var createBoardDTO = new CreateBoardDTO(userMentor.Id, userIntern.Id, "New Board", "Test board");

        // Act
        var response = await Client.PostAsJsonAsync(Boards.Base, createBoardDTO);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var created = await response.Content.ReadFromJsonAsync<BoardDTO>();

        created.ShouldNotBeNull();
        created!.Title.ShouldBe("New Board");
        created.Description.ShouldBe("Test board");
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedBoard_WhenDataIsValid()
    {
        // Arrange
        var board = TestDataHelper.CreateBoard(title: "Old Title");
        await AddEntityAsync(board);

        var updateBoardDTO = new UpdateBoardDTO("Updated Title", "Updated Description");

        // Act
        var response = await Client.PutAsJsonAsync($"{Boards.Base}/{board.Id}", updateBoardDTO);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<BoardDTO>();

        updated.ShouldNotBeNull();
        updated!.Title.ShouldBe("Updated Title");
        updated.Description.ShouldBe("Updated Description");
    }

    [Fact]
    public async Task Update_ShouldReturnNotFoundStatusCode()
    {
        //Arrange
        var updateBoardDTO = new UpdateBoardDTO("Updated Title", "Updated Description");

        //Act
        var response = await Client.PutAsJsonAsync($"{Boards.Base}/{Guid.NewGuid()}", updateBoardDTO);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
