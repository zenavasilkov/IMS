using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.IntegrationTests;

public static class TestDataHelper
{
    public static User CreateUser(
        Guid? id = null,
        string? firstname = null,
        string? lastname = null,
        Role role = Role.Intern)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Email = $"{(firstname ?? "user").ToLower()}.{(lastname ?? "test").ToLower()}" +
                $"{Guid.NewGuid().ToString("N")[..6]}@test.com",
            PhoneNumber = "123-456-7890",
            Firstname = firstname ?? "John",
            Lastname = lastname ?? "Doe",
            Patronymic = "Testovich",
            Role = role
        };
    }

    public static Board CreateBoard(
        Guid? id = null,
        string title = "Default Board",
        string description = "Default Description",
        User? createdBy = null,
        User? createdTo = null)
    {
        var creator = createdBy ?? CreateUser(role: Role.Mentor);
        var assignee = createdTo ?? CreateUser(role: Role.Intern);

        return new Board
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            Description = description,
            CreatedById = creator.Id,
            CreatedBy = creator,
            CreatedToId = assignee.Id,
            CreatedTo = assignee,
            Tickets = []
        };
    }

    public static Ticket CreateTicket(
        Guid? id = null,
        string title = "Default Ticket",
        string description = "Ticket Description",
        Board? board = null,
        TicketStatus status = TicketStatus.Unassigned)
    {
        var boardEntity = board ?? CreateBoard();

        return new Ticket
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            Description = description,
            BoardId = boardEntity.Id,
            Board = boardEntity,
            Status = status,
            DeadLine = DateTime.UtcNow.AddDays(7)
        };
    }

    public static Feedback CreateFeedback(
        Guid? id = null,
        Ticket? ticket = null,
        User? sentBy = null,
        User? addressedTo = null,
        string comment = "Good job!")
    {
        var ticketEntity = ticket ?? CreateTicket();
        var sender = sentBy ?? CreateUser(role: Role.Mentor);
        var receiver = addressedTo ?? CreateUser(role: Role.Intern);

        return new Feedback
        {
            Id = id ?? Guid.NewGuid(),
            TicketId = ticketEntity.Id,
            Ticket = ticketEntity,
            SentById = sender.Id,
            SentBy = sender,
            AddressedToId = receiver.Id,
            AddressedTo = receiver,
            Comment = comment
        };
    }

    public static Internship CreateInternship(
        Guid? id = null,
        User? intern = null,
        User? mentor = null,
        User? hrManager = null,
        InternshipStatus status = InternshipStatus.NotStarted)
    {
        var internUser = intern ?? CreateUser(role: Role.Intern);
        var mentorUser = mentor ?? CreateUser(role: Role.Mentor);
        var hr = hrManager ?? CreateUser(role: Role.HRManager);

        return new Internship
        {
            Id = id ?? Guid.NewGuid(),
            InternId = internUser.Id,
            Intern = internUser,
            MentorId = mentorUser.Id,
            Mentor = mentorUser,
            HumanResourcesManagerId = hr.Id,
            HumanResourcesManager = hr,
            StartDate = DateTime.UtcNow,
            EndDate = null,
            Status = status
        };
    }
}