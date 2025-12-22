namespace IMS.BLL.Models;

public class FeedbackModel : ModelBase
{
    public required Guid TicketId { get; init; }

    public required TicketModel Ticket { get; init; }

    public required Guid SentById { get; init; }

    public required UserModel SentBy { get; init; }

    public required Guid AddressedToId { get; init; }

    public required UserModel AddressedTo { get; init; }

    public required string Comment { get; set; }
}
