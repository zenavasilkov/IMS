namespace IMS.Presentation.DTOs.GetDTO;

public class FeedbackDto
{
    public Guid Id { get; init; }
    public Guid TicketId { get; init; }
    public Guid SentById { get; init; }
    public Guid AddressedToId { get; init; }
    public required string Comment { get; init; }
}
