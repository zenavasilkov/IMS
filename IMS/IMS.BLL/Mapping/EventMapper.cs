using IMS.BLL.Models;
using IMS.NotificationsCore.Messages;
using Shared.Enums;

namespace IMS.BLL.Mapping;

public static class EventMapper
{
    public static UserCreatedEvent ConvertToUserCreatedEvent(UserModel user)
    {
        return new UserCreatedEvent(
            user.Firstname, 
            user.Lastname, 
            user.Role.ToString(), 
            user.Email);
    }

    public static TicketCreatedEvent ConvertToTicketCreatedEvent(TicketModel ticket)
    {
        return new TicketCreatedEvent(
            ticket.Title,
            ticket.Description,
            ticket.DeadLine,
            ticket.Board.CreatedTo.Email);
    }

    public static TicketStatusChangedEvent ConvertToTicketStatusChangedEvent(
        TicketModel newTicket, TicketStatus oldStatus)
    {
        return new TicketStatusChangedEvent(
            newTicket.Title,
            newTicket.Description,
            oldStatus.ToString(),
            newTicket.Status.ToString(),
            newTicket.Board.CreatedBy.Email);
    }

    public static FeedbackCreatedEvent ConvertToFeedbackCreatedEvent(FeedbackModel feedback)
    {
        return new FeedbackCreatedEvent(
            feedback.Comment,
            feedback.AddressedTo.Email);
    }
}
