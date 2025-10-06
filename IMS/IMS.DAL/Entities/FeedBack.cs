namespace IMS.DAL.Entities;

public class Feedback : EntityBase
{   
    public required Guid TicketId { get; init; }

    public required Ticket Ticket { get; init; }

    public required Guid SentById { get; init; }

    public required User SentBy { get; init; }

    public required Guid AddressedToId { get; init; }

    public required User AddressedTo { get; init; }

    public required string Comment { get; set; }
}
